using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    public Transform TargetPos;
    public float PosLerpSpeed;

    public Transform TargetToLook;
    public float RotLerpSpeed;


    Vector3 localRotation;
    float cameraDistance;
    public float speedRot =5f;


    /*public float cameraSpeed = 120.0f;
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
    private float rotY = 0.0f;
    private float rotX = 0.0f;
    */


    void Start()
    {
        /*Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        */

    }

    void Update()
    {

        /*float inputX = Input.GetAxis("AxisX");
        float inputZ = Input.GetAxis("AxisY");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = inputX + mouseX;
        finalInputZ = inputZ + mouseY; 

        rotY += finalInputX * inputSensivity * Time.deltaTime;
        rotX += finalInputZ * inputSensivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        //if (Input.GetAxis("AxisX") != 0 || Input.GetAxis("AxisY") != 0)
        //{
            Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
            transform.rotation = localRotation; 
        //}
        */

            transform.position = Vector3.Lerp(transform.position, TargetPos.position, PosLerpSpeed);

            Vector3 PosToLook = TargetToLook.position - transform.position;
            Quaternion Rot = Quaternion.LookRotation(PosToLook);
            transform.rotation = Quaternion.Slerp(transform.rotation, Rot, RotLerpSpeed);

        //transform.Rotate(Input.GetAxis("AxisY"), Input.GetAxis("AxisX"), -Input.GetAxis("AxisX") + Input.GetAxis("AxisY") / 2);
    }

    void LateUpdate()
    {
        //if (Input.GetAxis("AxisX") != 0 || Input.GetAxis("AxisY") != 0)
        //{
            //CameraUpdater(); 
        //}
   
    }

    /*void CameraUpdater ()
    {
        Transform target = TargetToLook.transform;

        float step = cameraSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step); 
    }*/
}