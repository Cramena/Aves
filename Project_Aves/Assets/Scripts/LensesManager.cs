using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class LensesManager : MonoBehaviour
{
	// public PostProcessingProfile postProcess;

    /*
	[SerializeField]
	bool blackAndWhite;
	float bawTimer;
	[Range(0, 1)]
	public float greyValue = 0.5f;

	[SerializeField]
	bool night;
	float nightTimer;

	private Vector3 velocity;
	private float velocityFloat;
    */

    // Gamepad Variables
	#region
	[HideInInspector]
	public x360_Gamepad gamepad;
	private GamepadManager manager;
    #endregion

    public int playerIndex;

    // Lense variables
    List<string> lensesList = new List<string>() {"noLense", "bawLense", "nightLense"};
    private string currentLens;
    private int lensCounter;

    void Start ()
	{
        // Getting the gamepad
		manager = GamepadManager.Instance;
		gamepad=manager.GetGamepad(playerIndex);

		// InitializeSettings ();

        // Setting no lense as the initial lense
        lensCounter = 0;
        currentLens = lensesList[lensCounter];
        Debug.Log(lensesList.Count);
    }

    /*
	[ContextMenu("Reset post process values")]
	public void InitializeSettings()
	{
		ColorGradingModel.Settings colorGrading = postProcess.colorGrading.settings;
		ChromaticAberrationModel.Settings chromaticAberration = postProcess.chromaticAberration.settings;

		chromaticAberration.intensity = 0;

		colorGrading.basic.temperature = 0;

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
    */

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

        /*
        if (gamepad.GetButtonDown ("A"))
        {
            ToggleBlackAndWhite ();
		}
        if (gamepad.GetButtonDown ("B"))
        {
			ToggleNight ();
		}

        // black and white timer (from 0 to 1)
		if (bawTimer > 0)
		{
			UpdateBlackAndWhite ();
		}

		if (nightTimer > 0)
		{
			UpdateNight ();
		}
        */
	}

    void LensSwitcher(string whichLense)
    {
        if (whichLense == "noLense")
        {
            Debug.Log(whichLense);
        }

        if (whichLense == "bawLense")
        {

            Debug.Log(whichLense);
        }

        if (whichLense == "nightLense")
        {
            Debug.Log(whichLense);
        }
    }
    
    /*
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
			night = false;
		}
		else
		{
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
    */
}