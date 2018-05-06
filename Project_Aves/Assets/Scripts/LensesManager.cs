using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using Cinemachine;

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

    [Header("Shaders and Post Process")]

    [HideInInspector]
    // Hidden because they are gotten in void start
    public ReplacementShaderEffect bawShader;
    [HideInInspector]
    public PostProcessingBehaviour postProcess;

    public PostProcessingProfile noLenseProfile;
    public PostProcessingProfile bawProfile;

    [Header("Lense variables")]
    List<string> lensesList = new List<string>() { "noLense" };
    private string currentLens;
    private int lensCounter;

    [Tooltip("Toggle if the player has the black and white lense")]
    public bool hasBawLense;
    [Tooltip("Toggle if the player has the night lense")]
    public bool hasNightLense;

    void Start ()
	{
        // Getting the gamepad
		manager = GamepadManager.Instance;
		gamepad=manager.GetGamepad(playerIndex);

        // InitializeSettings ();

        bawShader = Camera.main.GetComponent<ReplacementShaderEffect>();
        postProcess = Camera.main.GetComponent<PostProcessingBehaviour>();

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
	}

    void LensSwitcher(string whichLense)
    {
        if (whichLense == "noLense")
        {
            Debug.Log(whichLense);

            bawShader.enabled = false;
            postProcess.profile = noLenseProfile;

            cineCamera.m_Lens.FarClipPlane = noLenseDepthOfView;
        }

        if (whichLense == "bawLense")
        {
            Debug.Log(whichLense);

            bawShader.enabled = true;
            postProcess.profile = bawProfile;

            cineCamera.m_Lens.FarClipPlane = bawDepthOfView;
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