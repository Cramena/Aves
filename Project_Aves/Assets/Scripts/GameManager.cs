﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public List<ControllerManager> players;
	public GameObject figure;
	public FigureController currentFigure;


	public void AddPlayer(ControllerManager player)
	{
		if (players.Count == 0)
		{
			player.playerIndex = 1;
		}
		else
		{
			player.playerIndex = 2;
		}
		players.Add (player);
	}

	public void CreateFigure()
	{
		Vector3 figurePostion = Camera.main.transform.position + (Camera.main.transform.forward * Camera.main.GetComponent<CameraController> ().distance2D);
		currentFigure = Instantiate (figure, figurePostion, Quaternion.identity).GetComponent<FigureController>();
	}

	public void ResetSong()
	{
		currentFigure.Reset ();
	}

	public void FigureComplete(GameObject figure) {
		Destroy (figure);
	}
}