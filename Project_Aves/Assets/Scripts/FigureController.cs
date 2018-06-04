using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureController : MonoBehaviour {

	bool completed;
	public GameObject figureImage;
	public FigureElementController[] blueElements;
	public FigureElementController[] redElements;
	public GameManager manager;

	void Start() {
		InitializeTargetColors ();
		//figureImage = GameObject.Find("Figure");
		//figureImage.SetActive(true);
	}

	public void ElementCompleted()
	{
		bool blueComplete = true;
		bool redComplete = true;
		for (int x = 0; x < blueElements.Length; x++)
		{
			if (blueElements [x].myColor != color.blue)
			{
				blueComplete = false;
			}
		}
		for (int y = 0; y < redElements.Length; y++)
		{
			if (redElements [y].myColor != color.red)
			{
				redComplete = false;
			}
		}
		if (redComplete && blueComplete)
		{
			//figureImage.SetActive(false);
			manager.FigureComplete (gameObject);
		}
	}

	public void Reset()
	{
		ResetSpecific (blueElements);
		ResetSpecific (redElements);
	}

	public void ResetSpecific (FigureElementController[] elements) {
		for (int x = 0; x < elements.Length; x++)
		{
			elements [x].myColor = color.nothing;	
		}
	}

	public void InitializeTargetColors()  {
		for (int x = 0; x < blueElements.Length; x++)
		{
			blueElements [x].targetColor = color.blue;	
		}
		for (int x = 0; x < redElements.Length; x++)
		{
			redElements [x].targetColor = color.red;	
		}
	}

}
