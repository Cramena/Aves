using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class LensesManager : MonoBehaviour {

	public PostProcessingProfile postProcess;

	#region
	[HideInInspector]
	public x360_Gamepad gamepad;
	private GamepadManager manager;
	#endregion

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

	public int playerIndex;


	// Use this for initialization
	void Start ()
	{
		manager = GamepadManager.Instance;
		gamepad=manager.GetGamepad(playerIndex);

		InitializeSettings ();
	}

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

	// Update is called once per frame
	void Update ()
	{
		if (gamepad.GetButtonDown ("A"))
		{
			ToggleBlackAndWhite ();
		}
		if (gamepad.GetButtonDown ("B"))
		{
			ToggleNight ();
		}

		if (bawTimer > 0)
		{
			UpdateBlackAndWhite ();
		}

		if (nightTimer > 0)
		{
			UpdateNight ();
		}
	}

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
