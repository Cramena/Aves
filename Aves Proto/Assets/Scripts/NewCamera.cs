using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamera : MonoBehaviour {

    public float cameraSpeed = 220.0f;
    public GameObject cameraRotate;
    Vector3 followPosition;
    public float clampAngle = 80.0f;
    public float inputSensivity = 150.0f;
    public GameObject CameraObj;
    public GameObject PlayerObj;
    public float camDistanceXToPlayer;
    public float camDistanceYToPlayer;
    public float camDistanceZToPlayer;
    public float mouseX;
    public float mouseY;
    public float finalInputX;
    public float finalInputZ;
    public float smoothX;
    public float smoothY;
    public float rotY = 0.0f;
    public float rotX = 0.0f;

    // Use this for initialization
    void Start ()
    {


        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {


        float inputX = Input.GetAxis("AxisX");
        float inputZ = Input.GetAxis("AxisY");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = inputX + mouseX;
        finalInputZ = inputZ + mouseY;

        rotY += finalInputX * inputSensivity * Time.deltaTime;
        rotX += finalInputZ * inputSensivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
    }

    void LateUpdate()
    {
            CameraUpdater();
    }

    void CameraUpdater()
    {
        Transform target = cameraRotate.transform;

        float step = cameraSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
