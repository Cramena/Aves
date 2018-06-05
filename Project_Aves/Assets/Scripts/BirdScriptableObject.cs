using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New bird profile", menuName = "Bird profile")]
public class BirdScriptableObject : ScriptableObject {

	public float acceleration;
	public float deceleration;

	public float minSpeed;
	public float maxSpeed;

	public float fovSpeed;
	public float minUpFOV;
	public float minFOV;
	public float maxFOV;

	public float turnSpeed;
	public float resettingSpeed;

	public GameObject model;
}
