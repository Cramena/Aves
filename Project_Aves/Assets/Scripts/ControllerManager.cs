﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.PostProcessing;
using UnityEngine.Networking;

public class ControllerManager : NetworkBehaviour
{

	public x360_Gamepad gamepad;
	private GamepadManager manager;

	public SetUpNetwork network;

	public SetUpNetwork player;
	public CinemachineFreeLook mainCam;
	public CinemachineVirtualCamera secondCam;
	public PostProcessingProfile postProcess;
	public Transform lookAt;
	public GameObject song;
	public GameManager gameManager;
	Rigidbody rigidbody;

	[Space]

	public int playerIndex;
	[Tooltip("How much the player must move the stick to activate it. Too low and it will be activated when the player isn't touching it. Too high and  the player won't be able to make subtle moves.")]
	[Range(0.1f, 0.9f)]
	public float deadzone = 0.25f;

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

	[Space]
	[Header("Transition variables")]
	[Space]

	public float transitionSpeed3D = 2;

	float zRotation;
	float loopingTimer;
	float resetLoopingTimer;
	Quaternion initialRotation;
	bool isResetting;
	float eulerAngZ;
	[SerializeField]
	bool is2D;
	[SerializeField]
	bool immobilised;
	[SerializeField]
	public bool isSinging;
	public float wrongSingTimer;
	public bool songIsWrong;

    void Start()
	{
		//gameManager.AddPlayer (this);
		manager = GamepadManager.Instance;
		gamepad = manager.GetGamepad(playerIndex);
		rigidbody = GetComponent<Rigidbody>();
		song.SetActive(false);
		InitializeVariables();
        Camera.main.GetComponent<LightManager>().CraneOrOwlLight();
    }

	void InitializeVariables()
	{
		myFieldOfView = minimumFov;
		secondCam.m_Lens.FieldOfView = myFieldOfView;
	}

	void Update()
	{
		UpdateSpeed();

		AccelerateOrDecelerate();

		if (!is2D)
		{
			if (!immobilised)
			{
				UpdateFOV();

				UpdateChromaticAberration();
			}

			//            mainCam.m_LookAt = transform;
			//            mainCam.m_Follow = transform;
		}
		else
		{
			if (isSinging)
			{
				if (wrongSingTimer < 1)
				{
					wrongSingTimer += Time.deltaTime;
				}
				else
				{
					wrongSingTimer = 0;
					songIsWrong = true;
				}
			}
			if (!immobilised)
			{
				CheckTurnBack2D();
				if (Input.GetAxis("RT") == 1 || Input.GetAxis("LT") == 1)
				{
					StartSinging();
				}
				else if (isSinging)
				{
					StopSinging();
				}
				if (Input.GetButtonDown("Fire2"))
				{
					ResetSong();
				}
			}
		}

		//        CheckGround ();

		CheckRecenter();

		CheckFor2D();

		UpdateRecenterHeading();

	}

	void FixedUpdate()
	{
		if (!immobilised)
		{
			if (!is2D)
			{
				UpdateRotation();
			}
			else
			{
				UpdateRotation2D();
			}
		}

		Move();
	}

	void UpdateRotation2D()
	{
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if (/*Mathf.Abs (Input.GetAxis("Horizontal")) >= deadzone || Mathf.Abs (Input.GetAxis("Vertical")) >= deadzone*/ input.magnitude >= deadzone)                                    //4.5f jours de code pour en arriver là
		{
			//            Vector2 input = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")).normalized;

			input = input.normalized * ((input.magnitude - deadzone) / (1 - deadzone));

			Quaternion turnRotation = Quaternion.Euler(Mathf.Atan2(-input.y, input.x) * 180 / Mathf.PI, 90, 0);
			turnRotation = Camera.main.transform.rotation * turnRotation;
			transform.rotation = Quaternion.RotateTowards(transform.rotation, turnRotation, turnSpeed2D);
		}
	}

	void UpdateRotation()
	{
		eulerAngZ = transform.localEulerAngles.z;
		if (Mathf.Abs(Input.GetAxis("Horizontal")) >= deadzone)
		{
			if ((eulerAngZ < 180 && eulerAngZ > 45 && Mathf.Sign(Input.GetAxis("Horizontal")) == -1) || (eulerAngZ > 180 && eulerAngZ < 320 && Mathf.Sign(Input.GetAxis("Horizontal")) == 1))
			{
				zRotation = 0;
			}
			else
			{
				zRotation = -Input.GetAxis("Horizontal") * turnSpeed * 2;
			}

			Quaternion yRotator = Quaternion.AngleAxis((Input.GetAxis("Horizontal") - deadzone * Mathf.Sign(Input.GetAxis("Horizontal"))) * turnSpeed, Vector3.up);
			transform.rotation = yRotator * transform.rotation;
			Quaternion zRotator = Quaternion.AngleAxis(zRotation, transform.forward);
			transform.rotation = zRotator * transform.rotation;
		}

		if (Mathf.Abs(Input.GetAxis("Vertical")) >= deadzone)
		{
			if (loopingTimer != 0)
			{
				loopingTimer = 0f;
			}

			Quaternion xRotator = Quaternion.AngleAxis(Input.GetAxis("Vertical") * turnSpeed, transform.right);
			Quaternion rotation = Quaternion.LookRotation(xRotator * transform.forward, transform.up);
			transform.rotation = rotation;
		}
		if ((Mathf.Abs(transform.forward.y) > 0.1f || Mathf.Sign(transform.up.y) < 0 || Mathf.Abs(transform.right.y) >= 0.1f) && Mathf.Abs(Input.GetAxis("Horizontal")) < deadzone && Mathf.Abs(Input.GetAxis("Vertical")) < deadzone)
		{
			CheckResetZAngle();
		}
		if (isResetting)
		{
			ResetZAngle();
		}
	}

	void CheckResetZAngle()
	{
		if (loopingTimer >= 1)
		{
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
		if (resetLoopingTimer < 1 && Mathf.Abs(Input.GetAxis("Vertical")) < deadzone && Mathf.Abs(Input.GetAxis("Horizontal")) < deadzone)
		{
			loopingTimer = 0;
			resetLoopingTimer += 1.0f * Time.fixedDeltaTime;
			Quaternion rotation;
			//            if (Mathf.Abs (transform.forward.y) > 0.3f) {
			//                rotation = Quaternion.LookRotation (new Vector3 (transform.forward.x, 0, transform.forward.z), Vector3.up);
			//            } else {
			rotation = Quaternion.LookRotation(new Vector3(transform.forward.x, 0, transform.forward.z), Vector3.up);
			//            }
			transform.rotation = Quaternion.Slerp(initialRotation, rotation, resetLoopingTimer);
		}
		else
		{
			isResetting = false;
			resetLoopingTimer = 0;
		}
	}

	void UpdateRecenterHeading()
	{
		if (Mathf.Abs(Input.GetAxis("Horizontal")) > deadzone || Mathf.Abs(Input.GetAxis("Vertical")) > deadzone)
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

		//        if (Mathf.Abs (gamepad.GetStick_L ().X) > .7f) {
		//            speed += -transform.forward.y * Time.deltaTime * acceleration / accelerationModifiyer - turningSlowingAmount * Time.deltaTime;
		//            speed = Mathf.Clamp (speed, minimumSpeedTurning, maximumSpeed);
		//        } else {
		speed += (-transform.forward.y + speedModifiyer) * Time.deltaTime * acceleration / accelerationModifiyer;
		speed = Mathf.Clamp(speed, minimumSpeed, maximumSpeed);
		//        }

	}

	void AccelerateOrDecelerate()
	{
		if (/*gamepad.GetTrigger_L () > deadzone*/Input.GetAxis("RT") > deadzone)
		{
			speedModifiyer = -5f;
		}
		else if (/*gamepad.GetTrigger_R () > deadzone*/Input.GetAxis("LT") > deadzone)
		{
			speedModifiyer = 5f;
		}
		else
		{
			speedModifiyer = 0;
		}
	}

	void Move()
	{
		rigidbody.velocity = transform.forward * speed;
	}

	void UpdateFOV()
	{
		myFieldOfView = secondCam.m_Lens.FieldOfView;
		/*if (transform.forward.y > 0.8f)
		{
			currentMinimumFov = Mathf.Lerp(currentMinimumFov, minimumUpFov, Time.deltaTime * speed);
			//currentMinimumFov = minimumUpFov;
		}
		else if (currentMinimumFov < minimumFov)//if (myFieldOfView >= 40)
		{
			currentMinimumFov = Mathf.Lerp(currentMinimumFov, minimumFov, Time.deltaTime * speed);
			//currentMinimumFov = minimumFov;
		}
		if (accelerationModifiyer == 1)
		{
			//myFieldOfView = Mathf.Lerp(myFieldOfView, maximumFov, speed * Time.deltaTime);
			myFieldOfView += Time.deltaTime * fovSpeed * speed;
		}
		else
		{
			//myFieldOfView = Mathf.Lerp(myFieldOfView, currentMinimumFov, speed * Time.deltaTime);
			myFieldOfView -= Time.deltaTime * fovSpeed * speed;
		}*/
		float targetFOV;
		if (transform.forward.y > 0.8f)
		{
			targetFOV = minimumUpFov;
		}
		else if (accelerationModifiyer == 1)
		{
			targetFOV = maximumFov;
		}
		else
		{
			targetFOV = minimumFov;
		}
		//myFieldOfView = Mathf.Clamp(myFieldOfView, currentMinimumFov, maximumFov);
		myFieldOfView = Mathf.Lerp(myFieldOfView, targetFOV, speed * Time.deltaTime * fovSpeed);
		secondCam.m_Lens.FieldOfView = myFieldOfView;
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
		if (Mathf.Abs(Input.GetAxis("Horizontal")) >= deadzone || Mathf.Abs(Input.GetAxis("Vertical")) >= deadzone)
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
		if (Input.GetButtonDown("Fire1"))
		{
			if (is2D)
			{
				print("Pressing the button");
				gameManager.CmdCheckTransition3D("Player " + GetComponent<NetworkIdentity>().netId.ToString());
			}
			else if (transform.position.y > 45 && (Mathf.Abs(Input.GetAxis("Horizontal")) < deadzone && Mathf.Abs(Input.GetAxis("Vertical")) < deadzone))
			{
				gameManager.CmdCheckTransition2D("Player " + GetComponent<NetworkIdentity>().netId.ToString());
			}
		}
	}

	public void Initialize2D()
	{
		//if (gameManager.axis = )
		is2D = true;
		Camera.main.GetComponent<CameraController>().TransitionCamera2D(transform.forward, transform.position, secondCam);
		Camera.main.GetComponent<CinemachineBrain>().enabled = false;
		/*
        Vector3 rightNoY = new Vector3 (transform.right.x, 0, transform.right.z);
        Quaternion final2DRotation = Quaternion.LookRotation (rightNoY, Vector2.up);
        StartCoroutine(TransitionTo2D (final2DRotation));
        */
		Vector3 rightNoY = gameManager.axisRight;
		Quaternion final2DRotation = Quaternion.LookRotation(gameManager.axisRight, Vector2.up);
		StartCoroutine(TransitionTo2D(final2DRotation));
	}

	public void Follow2D()
	{
		/*
        Vector3 otherForward;
        transform.forward = -otherForward;*/
	}

	//IEnumerator TransitionFollow2D()
	//{

	//    //while(counter < 1)
	//    //{
	//        yield return null;
	//    //}
	//}
	IEnumerator TransitionTo2D(Quaternion final2DRotation)
	{
		immobilised = true;
		ChromaticAberrationModel.Settings chromaticAberration = postProcess.chromaticAberration.settings;
		float initialAberration = chromaticAberration.intensity;
		myFieldOfView = secondCam.m_Lens.FieldOfView;
		float _initialFOV = secondCam.m_Lens.FieldOfView;

		//        Vector3 rightNoY = new Vector3 (transform.right.x, 0, transform.right.z);
		//        Quaternion final2DRotation = Quaternion.LookRotation (rightNoY, Vector2.up);
		Quaternion initialRotation = transform.rotation;

		float counter = 0;
		while (counter < 1)
		{
			chromaticAberration.intensity = Mathf.Lerp(initialAberration, 0, counter);
			postProcess.chromaticAberration.settings = chromaticAberration;
			myFieldOfView = Mathf.Lerp(_initialFOV, 60, counter);
			Camera.main.fieldOfView = Mathf.Lerp(_initialFOV, 60, counter);
			//            myFieldOfView -= Time.deltaTime * fovSpeed * speed;
			//            myFieldOfView = Mathf.Clamp(myFieldOfView, currentMinimumFov, maximumFov);
			//            myFieldOfView = Mathf.Lerp(initialFOV, minimumFov, counter);
			secondCam.m_Lens.FieldOfView = myFieldOfView;

			transform.rotation = Quaternion.Slerp(initialRotation, final2DRotation, counter);

			counter += Time.deltaTime * 4;
			counter = Mathf.Clamp(counter, 0, 1);
			yield return null;
		}
		//gameManager.CreateFigure ();
		transform.rotation = final2DRotation;
		immobilised = false;
	}

	public void Initialize3D()
	{
		is2D = false;
		StartCoroutine(TransitionTo3D());
	}

	IEnumerator TransitionTo3D()
	{
		Vector3 initialPosition = Camera.main.transform.position;
		Vector3 initialDirection = Camera.main.transform.forward;
		float counter = 0;
		while (counter < 1)
		{
			Camera.main.transform.position = Vector3.Lerp(initialPosition, secondCam.transform.position, counter);
			Camera.main.transform.forward = Vector3.Lerp(initialDirection, transform.forward, counter);
			counter += Time.deltaTime * transitionSpeed3D;
			yield return null;
		}
		Camera.main.GetComponent<CinemachineBrain>().enabled = true;
	}

	void CheckTurnBack2D()
	{
		Vector3 position2D = Camera.main.transform.InverseTransformPoint(transform.position);
		if (Mathf.Abs(position2D.x) > 45 || Mathf.Abs(position2D.y) > 25)
		{
			StartCoroutine(TurnBack2D());
		}
	}

	IEnumerator TurnBack2D()
	{
		immobilised = true;
		Vector3 targetDirection = -transform.forward;
		float counter = 0;
		Vector3 position2D = Camera.main.transform.InverseTransformPoint(transform.position);
		while (Vector3.Angle(transform.forward, targetDirection) > 5 && (Mathf.Abs(position2D.x) > 45 || Mathf.Abs(position2D.y) > 25))
		{
			position2D = Camera.main.transform.InverseTransformPoint(transform.position);
			Quaternion newRot = Quaternion.AngleAxis(turnSpeed2D, Camera.main.transform.InverseTransformDirection(Camera.main.transform.right));
			transform.rotation = transform.rotation * newRot;
			counter += turnSpeed2D /** Time.fixedDeltaTime*/;
			yield return new WaitForSeconds(Time.fixedDeltaTime);
		}
		immobilised = false;
	}

	void StartSinging()
	{
		song.SetActive(true);
		if (!song.GetComponent<ParticleSystem>().isEmitting)
		{
			song.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		song.GetComponent<ParticleSystem>().Play();
		isSinging = true;
		song.SetActive(true);
	}

	void StopSinging()
	{
		isSinging = false;
		song.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
		wrongSingTimer = 0;
		if (songIsWrong)
		{
			ResetSong();
		}
	}

	void ResetSong()
	{
		song.SetActive(false);
		songIsWrong = false;
		gameManager.ResetSong();
	}

	private void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Obstacle")
		{
			rigidbody.AddForce((transform.position - other.transform.position) * 1, ForceMode.Impulse);
		}
	}

}