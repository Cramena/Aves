using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{

	public static Dictionary<string, SetUpNetwork> players = new Dictionary<string, SetUpNetwork>();
	public static Dictionary<string, SetUpNetwork> playersReady = new Dictionary<string, SetUpNetwork>();

	public GameObject figure;
	public FigureController currentFigure;
	ControllerManager readyPlayer;
	public Vector3 axis;
	public Vector3 axisRight;
	public Vector3 axisLeft;
	public Vector3 axisBack;

	void Start()
	{
		/*
        if (players.Count > 0)
        {
            players.RemoveAt(0);
            players.Clear();
        }
        */
		Camera.main.GetComponent<CameraController>().gameManager = this;
		print(players.Count);
	}

	public void AddPlayer(SetUpNetwork _player)
	{
		players.Add("Player " + _player.ID, _player);
		print(players.Count);
	}

 	public void CheckTransition2D(SetUpNetwork _player)
	{
		if (players.Count == 1)
		{
			_player.Initialize2D();
		}
		else if (players.Count == 2)
		{
			if (playersReady.Count <= 0)
			{
				playersReady.Add("Player " + _player.ID, _player);
			}
			else if (playersReady.Count == 1)
			{
				playersReady.Add("Player " + _player.ID, _player);
				foreach (string key in players.Keys)
				{
					if (players[key] == _player)
					{
						_player.Initialize2D();
					}
					else
					{
						players[key].Follow2D();
					}
				}
			}
		}
	}

	public void CheckTransition3D(SetUpNetwork _player)
	{
		playersReady.Clear();
		foreach (string key in players.Keys)
		{
			players[key].Initialize3D();
		}
	}

	//public void AddPlayer(ControllerManager player)
	//{
	//    if (players.Count == 0)
	//    {
	//        player.playerIndex = 1;
	//    }
	//    else
	//    {
	//        player.playerIndex = 2;
	//    }
	//    players.Add (player);
	//    for (int i = 0; i < players.Count; i++)
	//    {
	//        players[i].gameManager = this;
	//    }
	//}

	/*public void CheckTransitionTo2D(ControllerManager _caller)
    {
        //print(players.Count);
        if (players.Count == 1)
        {
            print("Player forward : " + new Vector3(players[0].transform.forward.x, 0, players[0].transform.forward.z));
            axis = new Vector3(players[0].transform.forward.x, 0, players[0].transform.forward.z);
            axisRight = new Vector3(-axis.z, axis.y, axis.x);
            axisLeft = new Vector3(axis.z, axis.y, -axis.x);
            axisBack = -axis;
            players[0].Initialize2D();
        }
        else if (players.Count > 1)
        {
            print("Multiplayer");
            if (readyPlayer == null)
            {
                readyPlayer = _caller;
            }
            else if(readyPlayer != _caller)
            {
                Vector3 f1 = players[0].transform.forward;
                Vector3 f2 = players[1].transform.forward;
                axis = Vector3.Cross(f1, f2);
                for (int i = 0; i < players.Count; i++)
                {
                    players[i].Initialize2D();
                }
            }
        }
        else
        {
            print("No players!");
        }
        axisRight = new Vector3(-axis.z, axis.y, axis.x);
        axisLeft = new Vector3(axis.z, axis.y, -axis.x);
        axisBack = -axis;
        CreateFigure();
    }*/

	void Transition2D()
	{

	}

	//public void CheckTransitionTo3D()
	//{
	//    for (int i = 0; i < players.Count; i++)
	//    {
	//        players[i].Initialize3D();
	//    }
	//    axis = Vector3.zero;
	//    axisRight = Vector3.zero;
	//    axisLeft = Vector3.zero;
	//    axisBack = Vector3.zero;
	//}

	//public void CreateFigure()
	//{
	//    Vector3 figurePostion = players[0].transform.position;//Camera.main.transform.position + (Camera.main.transform.forward * Camera.main.GetComponent<CameraController> ().distance2D);
	//    currentFigure = Instantiate (figure, figurePostion, /*Quaternion.LookRotation(players[0].transform.forward)*/players[0].transform.rotation).GetComponent<FigureController>();
	//}

	public void ResetSong()
	{
		currentFigure.Reset();
	}

	public void FigureComplete(GameObject figure)
	{
		Destroy(figure);
	}
}