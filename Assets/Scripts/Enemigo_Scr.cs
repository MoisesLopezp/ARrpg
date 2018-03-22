using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class Enemigo_Scr : MonoBehaviour, ITrackableEventHandler
{
    TrackableBehaviour trackable;
    GameManager gameManager;
    scr_Character mycharacter;

    private void Start()
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
            gameManager.SetNumEnemigo(gameManager.GetNumEnemigos() + 1);
            mycharacter.Canvas.SetActive(true);
            if (!scr_MGBattle.InGame)
                mycharacter.IsInGame = true;
        }
        else
        {
            gameManager.SetNumEnemigo(gameManager.GetNumEnemigos() - 1);
            mycharacter.Canvas.SetActive(false);
            mycharacter.IsInGame = false;
        }
    }
}