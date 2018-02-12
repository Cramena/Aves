using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ControllerManager : MonoBehaviour {

    public float speed = 5.0f;
    public float rotationSpeed = 100.0f;

	CinemachineFreeLook cam;

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

    void Start ()
    {
        myFieldOfView = 35.0f;
		cam = GetComponent<CinemachineFreeLook> ();
    }



    void Update ()
    {

        eulerAngX = transform.localEulerAngles.x;
        eulerAngY = transform.localEulerAngles.y;
        eulerAngZ = transform.localEulerAngles.z;

        if (transform.rotation.z != 0)
        {

            StartCoroutine("Replace");

            
        }

        transform.Rotate(Input.GetAxis("Vertical")/1.5f, Input.GetAxis("Horizontal"), -Input.GetAxis("Horizontal") );

		Move ();

		myFieldOfView += myFieldOfView * Time.deltaTime + speed;
		myFieldOfView = Mathf.Clamp(myFieldOfView, 30.0f, 90.0f);
		cam.m_Lens.FieldOfView = myFieldOfView;


        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position); 

        if (terrainHeight > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z); 
        }


    }

	void Move() {

		transform.position += transform.forward * Time.deltaTime * speed;
		speed -= transform.forward.y * Time.deltaTime * 4.0f;
		speed = Mathf.Clamp(speed, 2.0f, 15.0f);
	}

    IEnumerator Replace ()
    {
        yield return new WaitForSeconds(1);
        print("Done");

        Quaternion target = Quaternion.Euler (eulerAngX, eulerAngY, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);


        StartCoroutine("Stop");
    }

    IEnumerator Stop ()
    {
        yield return new WaitForSeconds(1);

        StopCoroutine("Replace");
    }

}
