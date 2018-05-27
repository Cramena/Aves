using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum color {
	nothing,
	blue,
	red
}

public class FigureElementController : MonoBehaviour {

	public color targetColor;
	public color myColor = color.nothing;
	FigureController figure;

	// Use this for initialization
	void Start ()
	{
		figure = transform.parent.GetComponent<FigureController> ();
	}

	void OnTriggerEnter(Collider other)
	{
		ControllerManager player = other.GetComponent<ControllerManager> ();
		if (other.tag == "Player")
		{
			if (player.isSinging)
			{
				if (player.playerIndex == 1)
				{
					myColor = color.blue;
				}
				else
				{
					myColor = color.red;
				}
				if (myColor == targetColor)
				{
					player.wrongSingTimer = 0;
				}
				figure.ElementCompleted ();
			}
		}
	}

}
