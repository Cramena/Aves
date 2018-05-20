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

    // Lense variables
    List<string> lensesList = new List<string>() { "noLense" };
    private string currentLens;
    private int lensCounter;

    [Header("Lense variables")]
    [Tooltip("Toggle if the player has the black and white lense")]
    public bool hasBawLense;
    [Tooltip("Toggle if the player has the night lense")]
    public bool hasNightLense;

    // Transition variables
    Animator blackImageAnimator;
    private bool noLensTransition;
    private float noLensTransitionTimer;

    void Start ()
	{
        // Getting the gamepad
		manager = GamepadManager.Instance;
		gamepad=manager.GetGamepad(playerIndex);

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

        // Adding the appropriate lense to the list if toggled in the inspector
        if (hasBawLense == true)
        {
            lensesList.Add("bawLense");
        }

        if (hasBawLense == true)
        {
            lensesList.Add("nightLense");
        }

        // Setting no lense as the initial lense
        lensCounter = 0;
        currentLens = lensesList[lensCounter];
    }
    
	void Update ()
	{
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

        if (noLensTransition == true)
        {
            noLensTransitionTimer += Time.deltaTime;
            if (noLensTransitionTimer >= 0.2)
            {
                bawShader.enabled = false;
                postProcess.profile = noLenseProfile;

                cineCamera.m_Lens.FarClipPlane = noLenseDepthOfView;
                Camera.main.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;

                noLensTransition = false;
                noLensTransitionTimer = 0;
            }
        }
    }

    void LensSwitcher(string whichLense)
    {
        if (whichLense == "noLense")
        {
            Debug.Log(whichLense);

            blackImageAnimator.SetTrigger("Transition");
            noLensTransition = true;

            /*
            bawShader.enabled = false;
            postProcess.profile = noLenseProfile;

            cineCamera.m_Lens.FarClipPlane = noLenseDepthOfView;
            Camera.main.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
            */
        }

        // Black and white lens
        if (whichLense == "bawLense")
        {
            Debug.Log(whichLense);

            // Transition : flash
            glowFlash.enabled = true;
            glowFlash.Activated();

            // Shader component on camera and black and white post process profile activated
            bawShader.enabled = true;
            postProcess.profile = bawProfile;

            // Attributing a very low depth of view to the camera
            cineCamera.m_Lens.FarClipPlane = bawDepthOfView;
            Camera.main.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
        }

        if (whichLense == "nightLense")
        {
            Debug.Log(whichLense);

            bawShader.enabled = false;
            // Will be changed to its own profile once created
            postProcess.profile = noLenseProfile;
            cineCamera.m_Lens.FarClipPlane = noLenseDepthOfView;
        }
    }
}