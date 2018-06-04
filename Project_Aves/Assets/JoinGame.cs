using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

	public NetworkManager networkManager;

	// Use this for initialization
	void Start () {
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null)
		{
			networkManager.StartMatchMaker();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			JoinRoom();
		}
	}

	void JoinRoom()
	{
		print("Joining room");
		networkManager.matchMaker.ListMatches(0, 1, "", false, 0, 0, OnMatchList);
	}

	void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
	{
		if (!success || matchList == null)
		{
			print("null");
			return;
		}
		//matchList[0]
		networkManager.matchMaker.JoinMatch(matchList[0].networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
		print("Sucessfully joined");
	}

}
