using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum color {
	nothing,
	blue,
	red
}

public class FigureElementController : MonoBehaviour {

	public color myColor = color.nothing;
	FigureController figure;

	// Use this for initialization
	void Start () {
		figure = transform.parent.GetComponent<FigureController> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (other.GetComponent<ControllerManager> ().isSinging) {
				if (other.GetComponent<ControllerManager> ().playerIndex == 1) {
					myColor = color.blue;
				} else {
					myColor = color.red;
				}
				figure.ElementCompleted ();
			}
		}
	}

}
