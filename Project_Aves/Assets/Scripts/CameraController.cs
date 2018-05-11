using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

	public float distance2D = 50;
	public float transitionSpeed = 4;

	bool is2D;
	Vector3 initialPosition;
	Vector3 initialDirection;
	public Vector3 direction2D;
	Vector3 playerStartPosition;
	Vector3 playerStartDirection;
	float transitionTimer;

	void Start()
    {

    }

    void FixedUpdate()
    {
		if (is2D) {
			Transitionning ();
		}
	}

	void Transitionning() {
		if (transitionTimer < 1) {
			transform.forward = Vector3.Slerp (initialDirection, direction2D, transitionTimer);
			transform.position = Vector3.Lerp (initialPosition, playerStartPosition + (playerStartDirection.normalized * distance2D), transitionTimer);
			transitionTimer += Time.fixedDeltaTime * transitionSpeed;
		}
	}

	public void TransitionCamera2D(Vector3 _playerDirection, Vector3 _playerPosition, CinemachineFreeLook _cinemachine) {
		transform.position = _cinemachine.transform.position;
		transform.rotation = Quaternion.LookRotation(_playerPosition - _cinemachine.transform.position);
		is2D = true;
		transitionTimer = 0;
		direction2D = new Vector3 (-_playerDirection.x, 0, -_playerDirection.z);
		playerStartPosition = _playerPosition;
		playerStartDirection = _playerDirection;
		initialPosition = transform.position;
		initialDirection = transform.forward;
	}

}