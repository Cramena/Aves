using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

	private uint roomSize = 2;

	private string roomName = "Presentation_Room";

	public NetworkManager networkManager;

	private void Start()
	{
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null)
		{
			networkManager.StartMatchMaker();
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CreateRoom();
		}
	}

	public void CreateRoom()
	{
		networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
		print(roomName + roomSize);
	}
}
