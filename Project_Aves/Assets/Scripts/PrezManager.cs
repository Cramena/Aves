using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PrezManager : MonoBehaviour
{

    public Animator play;
    public GameObject Timeline1;
    public PlayableDirector playableDirector;

    public void Update()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            PlayableDirector playableDirector = Timeline1.GetComponent<PlayableDirector>();
            if (playableDirector != null)
            {
                playableDirector.Play();
                startAnimDiapo1();
            }
        }
    }

    public void startAnimDiapo1()
    {
        play.SetBool("playAnim", true);
    }


    //public void Play ()
    //{
    //playableDirector.Play(); 
    //}
}