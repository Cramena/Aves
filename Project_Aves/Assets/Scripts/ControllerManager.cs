using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ControllerManager : MonoBehaviour {

    public float speed = 5.0f;
	private float previousSpeed = 5.0f;
    public float rotationSpeed = 100.0f;

	public CinemachineFreeLook mainCam;
	public CinemachineFreeLook secondCam;

    public float smooth = 4.0f;

    [HideInInspector]
    public float roll, yaw, pitch; //Inputs for roll, yaw, and pitch, taken from Unity's input system.

    [SerializeField]
    float eulerAngX;
    [SerializeField]
    float eulerAngY;
    [SerializeField]
    float eulerAngZ;

    float myFieldOfView;
	float fovDeceleration = 2;

	bool goingDown;

    void Start ()
    {
//		mainCam = GetComponentInChildren<CinemachineFreeLook> ();
		myFieldOfView = mainCam.m_Lens.FieldOfView;
	}

    void Update ()
    {

		UpdateRotation ();

		UpdateSpeed ();

		Move ();

		UpdateFOV ();

		CheckGround ();
       
    }

	void UpdateRotation()
	{
		eulerAngX = transform.localEulerAngles.x;
		eulerAngY = transform.localEulerAngles.y;
		eulerAngZ = transform.localEulerAngles.z;
		print (eulerAngX);

		if (transform.rotation.z != 0 && Input.GetAxis ("Vertical") < 0.5f && Input.GetAxis ("Horizontal") < 0.5f)
		{
			StartCoroutine("Replace");
		}

		transform.Rotate((Input.GetAxis("Vertical")/1.5f) * Time.deltaTime * rotationSpeed, (Input.GetAxis("Horizontal")) * Time.deltaTime * rotationSpeed, (-Input.GetAxis("Horizontal")) * Time.deltaTime * rotationSpeed );
	}

	void UpdateSpeed() {
		previousSpeed = speed;
		speed += -transform.forward.y * Time.deltaTime * 4.0f;
		speed = Mathf.Clamp(speed, 5.0f, 20.0f);
	}

	void Move()
	{
		transform.position += transform.forward * Time.deltaTime * speed;
	}

	void UpdateFOV()
	{
		if (eulerAngX > 45 && eulerAngY < 135) {
			myFieldOfView = Mathf.Lerp (mainCam.m_Lens.FieldOfView, 90, fovDeceleration * Time.deltaTime);
			secondCam.Priority = 15;
			goingDown = true;
		} else {
			myFieldOfView = Mathf.Lerp (mainCam.m_Lens.FieldOfView, 30, fovDeceleration * Time.deltaTime);
			secondCam.Priority = 5;
			goingDown = false;
		}
		myFieldOfView = Mathf.Clamp(myFieldOfView, 30.0f, 90.0f);
		mainCam.m_Lens.FieldOfView = myFieldOfView;
//		if (speed > previousSpeed) {
//			myFieldOfView = cam.m_Lens.FieldOfView + speed * Time.deltaTime;
//		} else {
//			myFieldOfView = Mathf.Lerp (cam.m_Lens.FieldOfView, 30, fovDeceleration * Time.deltaTime);
//		}
//		myFieldOfView = Mathf.Clamp(myFieldOfView, 30.0f, 90.0f);
//		cam.m_Lens.FieldOfView = myFieldOfView;
	}

	void CheckGround()
	{
		float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position); 

		if (terrainHeight > transform.position.y)
		{
			transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z); 
		}
	}


    IEnumerator Replace ()
    {
        yield return new WaitForSeconds(1);
        print("Done");

        Quaternion target = Quaternion.Euler (eulerAngX, eulerAngY, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

//        StartCoroutine("Stop");
    }

    IEnumerator Stop ()
    {
        yield return new WaitForSeconds(1);

        StopCoroutine("Replace");
    }

}
