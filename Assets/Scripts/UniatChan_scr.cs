using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class UniatChan_scr : MonoBehaviour, ITrackableEventHandler
{
    TrackableBehaviour trackable;
    GameManager gameManager;
    scr_Character mycharacter;

    void Start()
    {
        mycharacter = GetComponent<scr_Character>();
        gameManager = FindObjectOfType<GameManager>();
        trackable = this.GetComponent<TrackableBehaviour>();
        if (trackable)
            trackable.RegisterTrackableEventHandler(this);
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status PreviousSta, TrackableBehaviour.Status NewSta)
    {
        if (NewSta == TrackableBehaviour.Status.DETECTED || NewSta == TrackableBehaviour.Status.TRACKED)
        {
            gameManager.SetBJugador(true);
            mycharacter.Canvas.SetActive(true);
            if (!scr_MGBattle.InGame)
                mycharacter.IsInGame = true;
        }
        else
        {
            gameManager.SetBJugador(false);
            mycharacter.Canvas.SetActive(false);
            mycharacter.IsInGame = false;
        }
    }
}