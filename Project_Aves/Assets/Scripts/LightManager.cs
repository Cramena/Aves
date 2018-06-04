using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    /*
    If you need to change variables in the inspector :
    1) Drag and drop the "Player" prefab into the scene
    2) Go to "main camera", child of "Camerabase", child of "Player"
    3) Change variables via the inspector
    4) When finished, apply the whole "Player" game object to update the prefab
    5) Delete the "Player" game object in the scene
    */

    public bool hasCraneLights;

    public GameObject owlDayLight;
    public GameObject craneDayLight;
    public GameObject craneNightLight;

    public Light owlDayLightComp;
    public Light craneDayLightComp;
    public Light craneNightLightComp;

    [HideInInspector]
    public bool startingInDayLight;

    /*
    hasCraneLights is the variable that determines which directional lights are going to be rendered by the player camera
    hasCraneLights value (true or false) is determined during the Start event function of the "LensesMananger" script with the public variable IsCrane
    */

    void Start()
    {
        owlDayLight = GameObject.Find("Owl Day Light");
        craneDayLight = GameObject.Find("Crane Day Light");
        craneNightLight = GameObject.Find("Crane Night Light");

        owlDayLightComp = owlDayLight.GetComponent<Light>();
        craneDayLightComp = craneDayLight.GetComponent<Light>();
        craneNightLightComp = craneNightLight.GetComponent<Light>();
    }

    public void CraneOrOwlLight()
    {
        if (hasCraneLights == true)
        {
			print("Is a crane");
			craneNightLightComp.enabled = true;
            craneDayLightComp.enabled = true;
            owlDayLightComp.enabled = false;
        }

        else if (hasCraneLights == false)
        {
			print("Is an owl");
			craneNightLightComp.enabled = false;
            craneDayLightComp.enabled = false;
            owlDayLightComp.enabled = true;
            startingInDayLight = true;
        }
    }

    public void EnableNightLights()
    {
        craneDayLight.SetActive(false);
        craneNightLight.SetActive(true);
    }

    public void DisableNightLights()
    {
        craneDayLight.SetActive(true);
        craneNightLight.SetActive(false);
    }
}