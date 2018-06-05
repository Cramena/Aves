using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
	public GameManager gameManager;
	public GameManager secondCam;

	public AudioSource stormSource;
	public AudioSource calmSource;

	public float distance2D = 50;
	public float transitionSpeed = 4;

	bool is2D;
	Vector3 initialPosition;
	Vector3 initialDirection;
	Vector3 endPosition;
	public Vector3 direction2D;
	Vector3 playerStartPosition;
	Vector3 playerStartDirection;
	float transitionTimer;

	void Start()
	{
		gameManager = GameObject.Find("GameManager1").GetComponent<GameManager>();
		stormSource.Play();
	}

	void FixedUpdate()
	{
		if (is2D)
		{
			Transitionning();
		}
	}

	void Transitionning()
	{
		if (transitionTimer < 1)
		{
			transform.forward = Vector3.Lerp(initialDirection, direction2D, transitionTimer);
			transform.position = Vector3.Lerp(initialPosition, endPosition, transitionTimer);
			//secondCam.transform.position = transform.position;
			//secondCam.transform.rotation = transform.rotation;
			transitionTimer += Time.fixedDeltaTime * transitionSpeed;
		}
		else
		{
			Camera.main.fieldOfView = 60;
			transform.forward = direction2D;
			transform.position = endPosition;
			//            print(Vector3.Distance(GameObject.Find("Player").transform.position, endPosition));
		}
	}

	public void TransitionCamera2D(Vector3 _playerDirection, Vector3 _playerPosition, CinemachineVirtualCamera _cinemachine)
	{
		transform.position = _cinemachine.transform.position;
		transform.rotation = Quaternion.LookRotation(_playerPosition - _cinemachine.transform.position);
		is2D = true;
		transitionTimer = 0;
		direction2D = /*endPosition - _playerPosition;*//*-gameManager.axis;*/new Vector3(-_playerDirection.x, 0, -_playerDirection.z);
		playerStartPosition = _playerPosition;
		playerStartDirection = new Vector3(_playerDirection.x, 0, _playerDirection.z);
		initialPosition = transform.position;
		initialDirection = transform.forward;
		//endPosition = playerStartPosition + (gameManager.axis * distance2D);

		endPosition = playerStartPosition + (playerStartDirection.normalized * distance2D);
		//        print(Vector3.Distance(GameObject.Find("Player").transform.position, endPosition));
		print("Axis : " + gameManager.axis);
	}

}