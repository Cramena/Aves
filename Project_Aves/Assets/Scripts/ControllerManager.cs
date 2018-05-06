using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.PostProcessing;

public class ControllerManager : MonoBehaviour {
	
	#region
	[HideInInspector]
	public x360_Gamepad gamepad;
	private GamepadManager manager;
	#endregion


	public CinemachineFreeLook mainCam;
	public PostProcessingProfile postProcess;
	public Transform lookAt;
	Rigidbody rigidbody;

	[Space]

	public int playerIndex;
	[Tooltip("How much the player must move the stick to activate it. Too low and it will be activated when the player isn't touching it. Too high and  the player won't be able to make subtle moves.")]
	[Range(0.1f, 0.9f)]
	public float deadzone = 0.2f;

	[Space]
	[Header("Speed variables")]
	[Space]

	[SerializeField]
    float speed = 5.0f;
	[Tooltip("The speed at which speed is sped up.")]
	[Range(0.01f, 100)]
	public float acceleration = 15.0f;
	[Tooltip("The minimum speed value. Must be lower than maximum speed.")]
	[Range(1f, 100)]
	public float minimumSpeed = 10;
	[Tooltip("The maximum speed value. Must be higher than minimum speed.")]
	[Range(1f, 100)]
	public float maximumSpeed = 30;
	[Tooltip("The minimum speed value while turning. Must be lower than maximum speed while turning.")]
	[Range(1f, 100)]
	public float minimumSpeedTurning = 10;
	[Tooltip("The maximum speed value while turning. Must be higher than minimum speed while turning.")]
	[Range(1f, 100)]
	public float maximumSpeedTurning = 30;
	[Tooltip("The value by which speed is slowed down when turning.")]
	[Range(0.01f, 100)]
	public float turningSlowingAmount = 3;
	float previousSpeed = 5.0f;
	float accelerationModifiyer;
	float speedModifiyer;

	[Space]
	[Header("Fov variables")]
	[Space]

	[Tooltip("The speed at which the fov is increased.")]
	[Range(0.01f, 10)]
	public float fovSpeed = .5f;
	[Tooltip("The minimum value of the fov while going up. Must be lower than minimum Fov.")]
	[Range(1, 180)]
	public float minimumUpFov = 20;
	[Tooltip("The minimum value of the fov. Must be lower than maximum Fov.")]
	[Range(1, 180)]
	public float minimumFov = 40;
	[Tooltip("The maximum value of the fov. Must be higher than minimum Fov.")]
	[Range(1, 180)]
	public float maximumFov = 70;

	float currentMinimumFov;
	float myFieldOfView;

	[Space]
	[Header("Chromatic Aberration variables")]
	[Space]
	[Tooltip("The maximum chromatic aberration.")]
	[Range(0.1f, 10)]
	public float maximumAberration = 2;

	[Space]
	[Header("Rotation variables")]
	[Space]

	//Rotation Variables
	[Tooltip("The speed at which the bird turns.")]
	public float turnSpeed = 2;
	[Tooltip("The speed at which the bird's Z axis resets to zero.")]
	public float resettingSpeed = 2;

	[Space]
	[Header("2D Rotation variables")]
	[Space]

	//Rotation Variables
	[Tooltip("The speed at which the bird turns in 2D.")]
	public float turnSpeed2D = 2;

	float zRotation;
	float loopingTimer;
	float resetLoopingTimer;
	Quaternion initialRotation;
	bool isResetting;
    float eulerAngZ;
	[SerializeField]
	bool is2D;
	bool isTransitionning;



    void Start ()
    {
		manager = GamepadManager.Instance;
		gamepad=manager.GetGamepad(playerIndex);
		rigidbody = GetComponent<Rigidbody> ();
//		mainCam.m_LookAt = transform;
//		mainCam.m_Follow = transform;

		InitializeVariables ();
	}

	void InitializeVariables()
	{
		myFieldOfView = minimumFov;
		mainCam.m_Lens.FieldOfView = myFieldOfView;
	}

    void Update ()
    {
		UpdateSpeed ();

		AccelerateOrDecelerate ();

		if (!is2D)
		{
			UpdateFOV ();

			UpdateChromaticAberration ();

//			mainCam.m_LookAt = transform;
//			mainCam.m_Follow = transform;
		}

//		CheckGround ();

		CheckRecenter ();

		CheckFor2D ();

		UpdateRecenterHeading ();

    }

	void FixedUpdate()
	{
		if (!isTransitionning && !is2D)
		{
			UpdateRotation ();
		}
		else if (is2D)
		{
			UpdateRotation2D ();
		}

		Move ();
	}

	void UpdateRotation2D()
	{
		if (Mathf.Abs (gamepad.GetStick_L ().X) >= deadzone || Mathf.Abs (gamepad.GetStick_L ().Y) >= deadzone)
		{
			Vector3 direction = new Vector3(0, Mathf.Atan2(-gamepad.GetStick_L ().Y, gamepad.GetStick_L ().X) * 180 / Mathf.PI, 0);

			float step = turnSpeed2D * Time.deltaTime;
			Quaternion turnRotation = Quaternion.Euler(direction.y, 0, 0f);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, step);
		}
	}

	void UpdateRotation()
	{
		eulerAngZ = transform.localEulerAngles.z;
		if (Mathf.Abs (gamepad.GetStick_L ().X) >= deadzone)
		{
//			if (Mathf.Sign (gamepad.GetStick_L ().X) == -1) {
//				zRotation = ((-gamepad.GetStick_L ().X/(1-deadzone)) * 45) - eulerAngZ;
//			} else {
//				zRotation = (360 + (-gamepad.GetStick_L ().X/(1-deadzone)) * 45) - eulerAngZ;
//			}
			if ((eulerAngZ < 180 && eulerAngZ > 45 && Mathf.Sign (gamepad.GetStick_L ().X) == -1) || (eulerAngZ > 180 && eulerAngZ < 320 && Mathf.Sign (gamepad.GetStick_L ().X) == 1))
			{
				zRotation = 0;
			}
			else
			{
				zRotation = -gamepad.GetStick_L ().X * turnSpeed * 2;
			}

			Quaternion yRotator = Quaternion.AngleAxis ((gamepad.GetStick_L ().X - deadzone * Mathf.Sign(gamepad.GetStick_L ().X)) * turnSpeed, Vector3.up);
			transform.rotation = yRotator * transform.rotation;
			Quaternion zRotator = Quaternion.AngleAxis (zRotation, transform.forward);
			transform.rotation = zRotator * transform.rotation;
		}

		if (Mathf.Abs (gamepad.GetStick_L ().Y) >= deadzone)
		{
			if (loopingTimer != 0)
			{
				loopingTimer = 0f;
			}

			Quaternion xRotator = Quaternion.AngleAxis (gamepad.GetStick_L ().Y * turnSpeed, transform.right);
			Quaternion rotation = Quaternion.LookRotation (xRotator * transform.forward, transform.up);
			transform.rotation = rotation;
		}
		if (transform.up != Vector3.up && Mathf.Abs (gamepad.GetStick_L ().X) < deadzone && Mathf.Abs (gamepad.GetStick_L ().Y) < deadzone)
		{
			CheckResetZAngle ();
		}
		if (isResetting)
		{
			ResetZAngle ();
		}
	}

	void CheckResetZAngle()
	{
		if (loopingTimer >= 1)
		{
			print ("Resetting");
			resetLoopingTimer = 0;
			initialRotation = transform.rotation;
			isResetting = true;
		}
		else
		{
			loopingTimer += Time.fixedDeltaTime * resettingSpeed;
		}
	}

	void ResetZAngle()
	{
		if (resetLoopingTimer < 1 && Mathf.Abs (gamepad.GetStick_L ().Y) < deadzone && Mathf.Abs (gamepad.GetStick_L ().X) < deadzone)
		{
			loopingTimer = 0;
			resetLoopingTimer += 1.0f * Time.fixedDeltaTime;
			Quaternion rotation;
//			if (Mathf.Abs (transform.forward.y) > 0.3f) {
//				rotation = Quaternion.LookRotation (new Vector3 (transform.forward.x, 0, transform.forward.z), Vector3.up);
//			} else {
				rotation = Quaternion.LookRotation (transform.forward, Vector3.up);
//			}
			transform.rotation = Quaternion.Slerp (initialRotation, rotation, resetLoopingTimer);
		}
		else
		{
			isResetting = false;
			resetLoopingTimer = 0;
		}
	}

	void UpdateRecenterHeading()
	{
		if (Mathf.Abs (gamepad.GetStick_L ().X) > deadzone || Mathf.Abs (gamepad.GetStick_L ().Y) > deadzone)
		{
			mainCam.m_RecenterToTargetHeading.m_enabled = true;
		}
		else
		{
			mainCam.m_RecenterToTargetHeading.m_enabled = false;
		}
		
	}
		
	void UpdateSpeed()
	{
		previousSpeed = speed;
		if (transform.forward.y < -0.1)
		{
			accelerationModifiyer = 1;
		}
		else
		{
			accelerationModifiyer = 2;
		}

//		if (Mathf.Abs (gamepad.GetStick_L ().X) > .7f) {
//			speed += -transform.forward.y * Time.deltaTime * acceleration / accelerationModifiyer - turningSlowingAmount * Time.deltaTime;
//			speed = Mathf.Clamp (speed, minimumSpeedTurning, maximumSpeed);
//		} else {
		speed += (-transform.forward.y + speedModifiyer) * Time.deltaTime * acceleration / accelerationModifiyer;
			speed = Mathf.Clamp(speed, minimumSpeed, maximumSpeed);
//		}

	}

	void AccelerateOrDecelerate()
	{
		if (gamepad.GetTrigger_L () > deadzone)
		{
			print ("Left Trigger");
			speedModifiyer =- 5f;
		}
		else if (gamepad.GetTrigger_R () > deadzone)
		{
			print ("Right Trigger");
			speedModifiyer = 5f;
		}
		else
		{
			speedModifiyer = 0;
		}
	}

	void Move()
	{
		rigidbody.velocity =  transform.forward * speed;
	}

	void UpdateFOV()
	{
		myFieldOfView = mainCam.m_Lens.FieldOfView;
		if (transform.forward.y > 0.5f)
		{
			currentMinimumFov = minimumUpFov;
		}
		else if (myFieldOfView >= 40)
		{
			currentMinimumFov = minimumFov;
		}
		if (accelerationModifiyer == 1)
		{
			myFieldOfView += Time.deltaTime * fovSpeed * speed;
		}
		else
		{
			myFieldOfView -= Time.deltaTime * fovSpeed * speed;
		}
		myFieldOfView = Mathf.Clamp(myFieldOfView, currentMinimumFov, maximumFov);
		mainCam.m_Lens.FieldOfView = myFieldOfView;
	}

	void UpdateChromaticAberration()
	{
		ChromaticAberrationModel.Settings chromaticAberration = postProcess.chromaticAberration.settings;
		/*if (accelerationModifiyer == 1)
		{*/
		chromaticAberration.intensity = ((speed - minimumSpeed) / (maximumSpeed - minimumSpeed)) * maximumAberration;
		/*}
		else
		{
			chromaticAberration.intensity -= Time.deltaTime;
		}*/
		postProcess.chromaticAberration.settings = chromaticAberration;
	}

	void CheckGround()
	{
		float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position); 

		if (terrainHeight > transform.position.y)
		{
			transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z); 
		}
	}

	void CheckRecenter()
	{
		if (Mathf.Abs (gamepad.GetStick_L ().X) >= deadzone || Mathf.Abs (gamepad.GetStick_L ().Y) >= deadzone)
		{
			mainCam.m_RecenterToTargetHeading.m_enabled = false;
		}
		else
		{
			mainCam.m_RecenterToTargetHeading.m_enabled = true;
		}
	}

	void CheckFor2D()
	{
		if (gamepad.GetButtonDown ("X"))
		{
			if (is2D)
			{
				Initialize3D ();
			}
			else
			{
				Initialize2D ();
			}
		}
	}

	void Initialize2D()
	{
		is2D = true;
		mainCam.m_LookAt = null;
		mainCam.m_Follow = null;
		StartCoroutine(TransitionTo2D (mainCam.m_Lens.FieldOfView));
	}

	IEnumerator TransitionTo2D(float initialFOV)
	{
		isTransitionning = true;
		ChromaticAberrationModel.Settings chromaticAberration = postProcess.chromaticAberration.settings;
		float initialAberration = chromaticAberration.intensity;
		myFieldOfView = mainCam.m_Lens.FieldOfView;

		Vector3 rightNoY = new Vector3 (transform.right.x, 0, transform.right.z);
		Quaternion final2DRotation = Quaternion.LookRotation (rightNoY, Vector2.up);
		Quaternion initialRotation = transform.rotation;

		float counter = 0;
		while (counter < 1)
		{
			chromaticAberration.intensity = Mathf.Lerp (initialAberration, 0, counter);
			postProcess.chromaticAberration.settings = chromaticAberration;

//			myFieldOfView -= Time.deltaTime * fovSpeed * speed;
//			myFieldOfView = Mathf.Clamp(myFieldOfView, currentMinimumFov, maximumFov);
			myFieldOfView = Mathf.Lerp(initialFOV, minimumFov, counter);
			mainCam.m_Lens.FieldOfView = myFieldOfView;

			transform.rotation = Quaternion.Slerp (initialRotation, final2DRotation, counter);

			counter += Time.deltaTime * 4;
			counter = Mathf.Clamp (counter, 0, 1);
			yield return null;
		}
		isTransitionning = false;
	}

	void Initialize3D()
	{
		is2D = false;
	}

}