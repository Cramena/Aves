using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetUpNetwork : NetworkBehaviour {

	public Behaviour[] componentsToDisable;
	GameManager gameManager;
	public string ID;

	void Start ()
	{
		if (!isLocalPlayer)
		{
			for (int i = 0; i < componentsToDisable.Length; i++)
			{
				componentsToDisable [i].enabled = false;
			}
		}
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		ID = GetComponent<NetworkIdentity>().netId.ToString();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		//GameManager.players.Add("Player " + ID, this);
		gameManager.AddPlayer(this);
	}

	private void OnDisable()
	{
		GameManager.players.Remove("Player " + ID);
	}
}
