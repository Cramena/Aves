using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	//public List<ControllerManager> players;
	public static Dictionary<string, SetUpNetwork> players = new Dictionary<string, SetUpNetwork>();
	public GameObject figure;
	public FigureController currentFigure;
	ControllerManager readyPlayer;
	public Vector3 axis;
	public Vector3 axisRight;
	public Vector3 axisLeft;
	public Vector3 axisBack;



	public void AddPlayer(SetUpNetwork _player)
	{
		players.Add("Player " + _player.ID, _player);
	}

	//public void AddPlayer(ControllerManager player)
	//{
	//	if (players.Count == 0)
	//	{
	//		player.playerIndex = 1;
	//	}
	//	else
	//	{
	//		player.playerIndex = 2;
	//	}
	//	players.Add (player);
	//}

	//public void CheckTransitionTo2D(ControllerManager _caller)
	//{
	//	if (players.Count == 1)
	//	{
	//		players [0].Initialize2D ();
	//		axis = players[0].transform.forward;
	//	}
	//	else if (players.Count > 1)
 //       {
	//		if (readyPlayer == null)
	//		{
	//			readyPlayer = _caller;
	//		}
	//		else if(readyPlayer != _caller)
	//		{
	//			Vector3 f1 = players[0].transform.forward;
	//			Vector3 f2 = players[1].transform.forward;
	//			axis = Vector3.Cross(f1, f2);
	//			for (int i = 0; i < players.Count; i++)
	//			{
	//				players[i].Initialize2D();
	//			}
	//		}
 //       }
	//	axisRight = new Vector3(-axis.z, axis.y, axis.x);
	//	axisLeft = new Vector3(axis.z, axis.y, -axis.x);
	//	axisBack = -axis;
	//}

	//public void CheckTransitionTo3D()
	//{
	//	for (int i = 0; i < players.Count; i++)
	//	{
	//		players[i].Initialize3D();
	//	}
	//}

	public void CreateFigure()
	{
		Vector3 figurePostion = Camera.main.transform.position + (Camera.main.transform.forward * Camera.main.GetComponent<CameraController> ().distance2D);
		currentFigure = Instantiate (figure, figurePostion, Quaternion.identity).GetComponent<FigureController>();
	}

	public void ResetSong()
	{
		currentFigure.Reset ();
	}

	public void FigureComplete(GameObject figure)
	{
		Destroy (figure);
	}
}
