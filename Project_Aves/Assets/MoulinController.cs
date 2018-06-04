using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cinemachine;

public class MoulinController : NetworkBehaviour {

	public CinemachineVirtualCamera moulinCam;
	public AudioSource source;
	public AudioClip moulinSnd;

	[SyncVar]
	int nbPlayers;

	[SyncVar]
	bool hasTurned;

	public Animator animator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (other.GetComponent<ControllerManager>().speed > 27)
			{
				nbPlayers++;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player" && !hasTurned)
		{
			//StartCoroutine(RemovePlayer());
			CheckDone();
			nbPlayers--;
		}
	}

	IEnumerator RemovePlayer()
	{
		yield return new WaitForSeconds(3);
		nbPlayers--;
	}

	void CheckDone()
	{
		if (nbPlayers >= 1)
		{
			print("Le moulin tourne.");
			hasTurned = true;
			source.Play();
			animator.SetBool("turning", true);
			StartCoroutine(moulinShot());
		}
	}

	IEnumerator moulinShot()
	{
		moulinCam.Priority = 20;
		yield return new WaitForSeconds(2);
		moulinCam.Priority = 1;
	}

}
