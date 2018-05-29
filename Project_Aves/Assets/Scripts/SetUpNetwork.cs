using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetUpNetwork : NetworkBehaviour
{

	public Behaviour[] componentsToDisable;
	public GameManager gameManager;
	public ControllerManager controller;

	public string ID;

	// Use this for initialization
	void Start()
	{
		if (!isLocalPlayer)
		{
			for (int i = 0; i < componentsToDisable.Length; i++)
			{
				componentsToDisable[i].enabled = false;
			}
		}
	}

	public void Ready()
	{

	}

	public void Initialize2D()
	{
		controller.Initialize2D();
	}

	public void Follow2D()
	{
		controller.Follow2D();
	}

	public void Initialize3D()
	{
		controller.Initialize3D();
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		ID = GetComponent<NetworkIdentity>().netId.ToString();
		gameManager = GameObject.Find("GameManager1").GetComponent<GameManager>();
		//GameManager.players.Add("Player " + ID, this);
		gameManager.AddPlayer(this);
	}

	private void OnDisable()
	{
		GameManager.players.Remove("Player " + ID);
	}
}