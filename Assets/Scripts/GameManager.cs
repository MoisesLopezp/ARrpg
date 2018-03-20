using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //esta cosita nos dira cuantos enemigos hay en escena
    int num_enemigos;
    //nos dice si el jugador esta en escena
    bool jugador;
    public GameObject UI;

    private void Start()
    {
        SetBJugador(false);
        SetNumEnemigo(0);
    }

    public void SetBJugador(bool b)
    {
        jugador = b;

        Debug.Log("Jugador: " + jugador + " Enemigos: " + num_enemigos);

        if (jugador && num_enemigos > 0)
            ActivarUI();
        else
            DesactivarUI();
    }

    public bool GetBJugador()
    {
        return jugador;
    }

    public void SetNumEnemigo(int _num)
    {
        num_enemigos = _num;
        if (num_enemigos < 0)
            num_enemigos = 0;

        if (jugador && num_enemigos > 0)
            ActivarUI();
        else
            DesactivarUI();
    }

    public int GetNumEnemigos()
    {
        return num_enemigos;
    }

    void ActivarUI()
    {
        UI.SetActive(true);
    }

    void DesactivarUI()
    {
        UI.SetActive(false);
    }
}