using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PresentationState {
	showed,
	hidden
}

public class PresentationManager : MonoBehaviour {

	#region
	[HideInInspector]
	public x360_Gamepad gamepad;
	private GamepadManager manager;
	#endregion
	public int playerIndex = 1;
	public Image[] slides;
	int slideIndex = 0;
	PresentationState state = PresentationState.hidden;

	// Use this for initialization
	void Start () {
		manager = GamepadManager.Instance;
		gamepad=manager.GetGamepad(playerIndex);
		TransitionSlides ();
	}

	void TransitionSlides() {
		for (int i = 0; i < slides.Length; i++) {
			if (i == slideIndex) {
				slides [i].enabled = true;
//				Color slideColor = slides [i].material.color;
//				slideColor.a = 1;
//				print ("activating slide : " + i);
			} else {
				slides [i].enabled = false;
//				Color slideColor = slides [i].material.color;
//				slideColor.a = 0;
//				print ("activating slide : " + i);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (gamepad.GetButtonDown ("DPad_Right")) {
			NextSlide ();
		}
		if (gamepad.GetButtonDown ("DPad_Left")) {
			PreviousSlide ();
		}
		if (gamepad.GetButtonDown ("DPad_Up")) {
			ShowSlide ();
		}
		if (gamepad.GetButtonDown ("DPad_Down")) {
			HideSlide ();
		}
	}

	void NextSlide() {
		if (slideIndex < slides.Length - 1) {
			print ("Next slide");
			slideIndex++;
			if (state == PresentationState.showed) {
				TransitionSlides ();
			}
		}
	}

	void PreviousSlide() {
		if (slideIndex > 0) {
			print ("Previous slide");
			slideIndex--;
			if (state == PresentationState.showed) {
				TransitionSlides ();
			}
		}
	}

	void ShowSlide() {
		print ("Show slide");
		TransitionSlides ();
		state = PresentationState.showed;
	}

	void HideSlide() {
		print ("Hide slide");
		for (int i = 0; i < slides.Length; i++) {
			slides [i].enabled = false;
//			Color slideColor = slides [i].material.color;
//			slideColor.a = 0;
		}
		state = PresentationState.hidden;
	}
}
