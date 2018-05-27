using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using Cinemachine;
using UnityEngine.UI;


public class LensesManager : MonoBehaviour
{
    // Gamepad Variables
    #region
    [HideInInspector]
    public x360_Gamepad gamepad;
    private GamepadManager manager;
    #endregion

    public int playerIndex;
    [Header("Owl or Crane?")]
    public bool isCrane;

    [Header("Camera variables")]
    public CinemachineFreeLook cineCamera;
    public float noLenseDepthOfView;
    public float bawDepthOfView;

    // Hidden because they are gotten in void start
    [HideInInspector]
    public ReplacementShaderEffect bawShader;
    [HideInInspector]
    public PostProcessingBehaviour postProcess;
    [HideInInspector]
    public QuickGlow glowFlash;

    [Header("Shaders and Post Process")]
    public PostProcessingProfile noLenseProfile;
    public PostProcessingProfile bawProfile;
    public PostProcessingProfile nightProfile;

    // Lense variables
    List<string> lensesList = new List<string>() { "noLense" };
    private string currentLens;
    private int lensCounter;

    [Header("Skyboxes")]
    public Material daySkybox;
    public Material nightSkybox;

    // Will be found during the event start function
    [HideInInspector]
    public LightManager lightManager;

    // Transition variables
    Animator blackImageAnimator;
    private bool noLensTransition;
    private float noLensTransitionTimer;

    Animator whiteImageAnimator;
    private bool nightTransition;
    private float nightTransitionTimer;

    void Start()
    {
        // Getting the gamepad
        manager = GamepadManager.Instance;
        gamepad = manager.GetGamepad(playerIndex);

        // Getting the shader scripts from the camera
        bawShader = Camera.main.GetComponent<ReplacementShaderEffect>();
        postProcess = Camera.main.GetComponent<PostProcessingBehaviour>();
        glowFlash = Camera.main.GetComponent<QuickGlow>();

        // Finding the black image animator inside the scene
        Animator[] _allAnimator = FindObjectsOfType<Animator>();
        foreach (Animator anim in _allAnimator)
        {
            if (anim.gameObject.name == "BlackImage")
            {
                blackImageAnimator = anim;
            }
        }

        // Finding the white image animator inside the scene
        foreach (Animator anim in _allAnimator)
        {
            if (anim.gameObject.name == "WhiteImage")
            {
                whiteImageAnimator = anim;
            }
        }

        lightManager = Camera.main.GetComponent<LightManager>();

        // Setting the light rendering settings in the camera
        if (isCrane == true)
        {
            lightManager.hasCraneLights = true;
        }

        // Adding the appropriate lense to the list if toggled in the inspector
        if (isCrane == false)
        {
            lensesList.Add("bawLense");
        }

        if (isCrane == true)
        {
            // later on : the nightLense should be added to the lenseList if the crane bird takes it
            lensesList.Add("nightLense");
        }

        // Setting no lense as the initial lense
        lensCounter = 0;
        currentLens = lensesList[lensCounter];
    }

    void Update()
    {
        // For the start: enabling day light and disabling night light for the crane bird
        if (lightManager.startingInDayLight == true)
        {
            lightManager.EnableNightLights();
            lightManager.startingInDayLight = false;
        }

        // Pressing LB or RB switches the lens on the list and triggers the LensSwitcher function
        if (gamepad.GetButtonDown("LB"))
        {
            if (lensCounter <= 0)
            {
                lensCounter = lensesList.Count - 1;
            }
            else
            {
                lensCounter = lensCounter - 1;
            }

            currentLens = lensesList[lensCounter];

            LensSwitcher(currentLens);
        }

        else if (gamepad.GetButtonDown("RB"))
        {
            if (lensCounter >= lensesList.Count - 1)
            {
                lensCounter = 0;
            }
            else
            {
                lensCounter = lensCounter + 1;
            }

            currentLens = lensesList[lensCounter];

            LensSwitcher(currentLens);
        }

        // Special transition made when coming back to normal
        if (noLensTransition == true)
        {
            noLensTransitionTimer += Time.deltaTime;
            if (noLensTransitionTimer >= 0.2)
            {
                bawShader.enabled = false;
                postProcess.profile = noLenseProfile;

                cineCamera.m_Lens.FarClipPlane = noLenseDepthOfView;
                Camera.main.GetComponent<Skybox>().material = daySkybox;
                Camera.main.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;

                noLensTransition = false;
                noLensTransitionTimer = 0;

                // Enables Day Light and disables Night Light in case the player is a crane and comes back from the night lense
                if (isCrane == true)
                {
                    lightManager.DisableNightLights();
                }
            }
        }

        // White transition for the night, maybe I can find a better idea...
        if (nightTransition == true)
        {
            nightTransitionTimer += Time.deltaTime;
            if (nightTransitionTimer >= 0.2)
            {
                bawShader.enabled = false;
                postProcess.profile = nightProfile;
                cineCamera.m_Lens.FarClipPlane = noLenseDepthOfView;

                Camera.main.GetComponent<Skybox>().material = nightSkybox;
                Camera.main.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;

                lightManager.EnableNightLights();

                nightTransition = false;
                nightTransitionTimer = 0;

                lightManager.EnableNightLights();
            }
        }
    }


    void LensSwitcher(string whichLense)
    {
        if (whichLense == "noLense")
        {
            blackImageAnimator.SetTrigger("Transition");
            // Triggers the transition in the update
            noLensTransition = true;
        }

        // Black and white lens
        if (whichLense == "bawLense")
        {
            // Transition : flash
            glowFlash.enabled = true;
            glowFlash.Activated();

            // Shader component on camera and black and white post process profile activated
            bawShader.enabled = true;
            postProcess.profile = bawProfile;

            // Attributing a very low depth of view to the camera
            cineCamera.m_Lens.FarClipPlane = bawDepthOfView;
            Camera.main.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;

            lightManager.DisableNightLights();
        }

        if (whichLense == "nightLense")
        {
            whiteImageAnimator.SetTrigger("Transition");
            nightTransition = true;

            /*
             * Currently, I'm using the white transition but I keep those in case I find something better
             * 
            bawShader.enabled = false;
            postProcess.profile = nightProfile;
            cineCamera.m_Lens.FarClipPlane = noLenseDepthOfView;

            Camera.main.GetComponent<Skybox>().material = nightSkybox;
            Camera.main.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;

            lightManager.EnableNightLights();
            */
        }
    }
}