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

<<<<<<< HEAD
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
=======
		colorGrading.channelMixer.blue.x = 0;
		colorGrading.channelMixer.blue.y = 0;
		colorGrading.channelMixer.blue.z = 1;

		colorGrading.channelMixer.green.x = 0;
		colorGrading.channelMixer.green.y = 1;
		colorGrading.channelMixer.green.z = 0;

		colorGrading.channelMixer.red.x = 1;
		colorGrading.channelMixer.red.y = 0;
		colorGrading.channelMixer.red.z = 0;

		colorGrading.basic.saturation = 1;

		colorGrading.basic.contrast = 1;

		postProcess.chromaticAberration.settings = chromaticAberration;
		postProcess.colorGrading.settings = colorGrading;
	}

	// Update is called once per frame
//	void Update ()
//	{
//		if (gamepad.GetButtonDown ("A"))
//		{
//			ToggleBlackAndWhite ();
//		}
//		if (gamepad.GetButtonDown ("B"))
//		{
//			ToggleNight ();
//		}
//
//		if (bawTimer > 0)
//		{
//			UpdateBlackAndWhite ();
//		}
//
//		if (nightTimer > 0)
//		{
//			UpdateNight ();
//		}
//	}

	void ToggleBlackAndWhite()
	{
		if (blackAndWhite)
		{
			blackAndWhite = false;
		}
		else
		{
			blackAndWhite = true;
		}
		bawTimer = 1f;

	}

	void ToggleNight()
	{
		if (night)
		{
//			ColorGradingModel.Settings colorGrading = postProcess.colorGrading.settings;
//
//			colorGrading.basic.temperature = Mathf.SmoothDamp (colorGrading.basic.temperature, 0, ref velocityFloat, 1);
//
//			colorGrading.channelMixer.blue = Vector3.SmoothDamp (colorGrading.channelMixer.blue, new Vector3(0, 0, 1), ref velocity, 1);
//			colorGrading.channelMixer.green = Vector3.SmoothDamp (colorGrading.channelMixer.green, new Vector3(0, 1, 0), ref velocity, 1);
//			colorGrading.channelMixer.red = Vector3.SmoothDamp (colorGrading.channelMixer.red, new Vector3(1, 0, 0), ref velocity, 1);
//
//			colorGrading.basic.saturation = Mathf.SmoothDamp (colorGrading.basic.saturation, 1, ref velocityFloat, 1);
//
//			postProcess.colorGrading.settings = colorGrading;

			night = false;
		}
		else
		{
//			ColorGradingModel.Settings colorGrading = postProcess.colorGrading.settings;
//
//			colorGrading.basic.temperature = Mathf.SmoothDamp (colorGrading.basic.temperature, 100, ref velocityFloat, 1);
//
//			colorGrading.channelMixer.blue = Vector3.SmoothDamp (colorGrading.channelMixer.blue, new Vector3(0.2f, 0.2f, 0), ref velocity, 1);
//			colorGrading.channelMixer.green = Vector3.SmoothDamp (colorGrading.channelMixer.green, new Vector3(0, 2, 0), ref velocity, 1);
//			colorGrading.channelMixer.red = Vector3.SmoothDamp (colorGrading.channelMixer.red, new Vector3(2, 0, 0), ref velocity, 1);
//
//			colorGrading.basic.saturation = Mathf.SmoothDamp (colorGrading.basic.saturation, 2, ref velocityFloat, 1);
//
//			postProcess.colorGrading.settings = colorGrading;

			night = true;
		}
		nightTimer = 1f;

	}

	void UpdateBlackAndWhite()
	{
		ColorGradingModel.Settings colorGrading = postProcess.colorGrading.settings;

		if (!blackAndWhite)
		{
			colorGrading.channelMixer.blue.x = Mathf.Clamp (colorGrading.channelMixer.blue.x - Time.deltaTime, 0, 1);
			colorGrading.channelMixer.blue.y = Mathf.Clamp (colorGrading.channelMixer.blue.y - Time.deltaTime, 0, 1);
			colorGrading.channelMixer.blue.z = Mathf.Clamp (colorGrading.channelMixer.blue.z + Time.deltaTime, 0, 1);

			colorGrading.channelMixer.green.x = Mathf.Clamp (colorGrading.channelMixer.green.x - Time.deltaTime, 0, 1);
			colorGrading.channelMixer.green.y = Mathf.Clamp (colorGrading.channelMixer.green.y + Time.deltaTime, 0, 1);
			colorGrading.channelMixer.green.z = Mathf.Clamp (colorGrading.channelMixer.green.z - Time.deltaTime, 0, 1);

			colorGrading.channelMixer.red.x = Mathf.Clamp (colorGrading.channelMixer.red.x + Time.deltaTime, 0, 1);
			colorGrading.channelMixer.red.y = Mathf.Clamp (colorGrading.channelMixer.red.y - Time.deltaTime, 0, 1);
			colorGrading.channelMixer.red.z = Mathf.Clamp (colorGrading.channelMixer.red.z - Time.deltaTime, 0, 1);

//			colorGrading.basic.saturation = Mathf.Clamp (colorGrading.basic.saturation + Time.deltaTime, 0.5f, 1);

			colorGrading.basic.contrast = Mathf.Clamp(colorGrading.basic.contrast - Time.deltaTime, 1, 2);
		}
		else
		{
			colorGrading.channelMixer.blue.x = Mathf.Clamp (colorGrading.channelMixer.blue.x + Time.deltaTime, 0, greyValue);
			colorGrading.channelMixer.blue.y = Mathf.Clamp (colorGrading.channelMixer.blue.y + Time.deltaTime, 0, greyValue);
			colorGrading.channelMixer.blue.z = Mathf.Clamp (colorGrading.channelMixer.blue.z - Time.deltaTime, greyValue, 1);

			colorGrading.channelMixer.green.x = Mathf.Clamp (colorGrading.channelMixer.green.x + Time.deltaTime, 0, greyValue);
			colorGrading.channelMixer.green.y = Mathf.Clamp (colorGrading.channelMixer.green.y - Time.deltaTime, greyValue, 1);
			colorGrading.channelMixer.green.z = Mathf.Clamp (colorGrading.channelMixer.green.z + Time.deltaTime, 0, greyValue);

			colorGrading.channelMixer.red.x = Mathf.Clamp (colorGrading.channelMixer.red.x - Time.deltaTime, greyValue, 1);
			colorGrading.channelMixer.red.y = Mathf.Clamp (colorGrading.channelMixer.red.y + Time.deltaTime, 0, greyValue);
			colorGrading.channelMixer.red.z = Mathf.Clamp (colorGrading.channelMixer.red.z + Time.deltaTime, 0, greyValue);

//			colorGrading.basic.saturation = Mathf.Clamp (colorGrading.basic.saturation - Time.deltaTime, 0.5f, 1);

			colorGrading.basic.contrast = Mathf.Clamp(colorGrading.basic.contrast + Time.deltaTime, 1, 2);
		}
		bawTimer -= Time.deltaTime;

		postProcess.colorGrading.settings = colorGrading;
	}

	void UpdateNight()
	{
		ColorGradingModel.Settings colorGrading = postProcess.colorGrading.settings;

		if (!night)
		{
			colorGrading.basic.temperature = Mathf.Clamp (colorGrading.basic.temperature - Time.deltaTime * 100, 0, 100);

			colorGrading.channelMixer.blue.x = Mathf.Clamp (colorGrading.channelMixer.blue.x - Time.deltaTime, 0, .2f);
			colorGrading.channelMixer.blue.y = Mathf.Clamp (colorGrading.channelMixer.blue.y - Time.deltaTime, 0, .2f);
			colorGrading.channelMixer.blue.z = Mathf.Clamp (colorGrading.channelMixer.blue.z + Time.deltaTime, 0, 1);

			colorGrading.channelMixer.green.y = Mathf.Clamp (colorGrading.channelMixer.green.y + Time.deltaTime, 0, 1);

			colorGrading.channelMixer.red.x = Mathf.Clamp (colorGrading.channelMixer.red.x + Time.deltaTime, 0, 1);

			colorGrading.basic.saturation = Mathf.Clamp(colorGrading.basic.contrast - Time.deltaTime, 1, 2);
		}
		else
		{
			colorGrading.basic.temperature = Mathf.Clamp (colorGrading.basic.temperature + Time.deltaTime * 100, 0, 100);

			colorGrading.channelMixer.blue.x = Mathf.Clamp (colorGrading.channelMixer.blue.x + Time.deltaTime, 0, .2f);
			colorGrading.channelMixer.blue.y = Mathf.Clamp (colorGrading.channelMixer.blue.y + Time.deltaTime, 0, .2f);
			colorGrading.channelMixer.blue.z = Mathf.Clamp (colorGrading.channelMixer.blue.z - Time.deltaTime, 0, 1);

			colorGrading.channelMixer.green.y = Mathf.Clamp (colorGrading.channelMixer.green.y - Time.deltaTime, 0, 1);

			colorGrading.channelMixer.red.x = Mathf.Clamp (colorGrading.channelMixer.red.x - Time.deltaTime, 0, 1);

			colorGrading.basic.saturation = Mathf.Clamp(colorGrading.basic.contrast + Time.deltaTime, 1, 2);
		}
		nightTimer -= Time.deltaTime;

		postProcess.colorGrading.settings = colorGrading;
	}

}
>>>>>>> master
