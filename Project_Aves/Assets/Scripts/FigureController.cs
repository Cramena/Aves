using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureController : MonoBehaviour {

	bool completed;
	public FigureElementController[] blueElements;
	public FigureElementController[] redElements;

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
			completed = true;
		}
	}

	public void Reset()
	{
		for (int x = 0; x < blueElements.Length; x++)
		{
			blueElements [x].myColor = color.nothing;	
		}
		for (int y = 0; y < redElements.Length; y++)
		{
			redElements [y].myColor = color.nothing;	
		}
	}

}
