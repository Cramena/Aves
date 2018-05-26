using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetUpNetwork : NetworkBehaviour {

	public Behaviour[] componentsToDisable;

<<<<<<< HEAD
=======

>>>>>>> parent of 7d97277... Merge branch 'master' into Matthias
	// Use this for initialization
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
<<<<<<< HEAD
	
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
=======
>>>>>>> parent of 7d97277... Merge branch 'master' into Matthias
}
