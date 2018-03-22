using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Scr : MonoBehaviour
{
    public GameObject Credits;

    public void Jugar()
    {
        SceneManager.LoadScene("Juego");
    }

    public void Salir()
    {
        Application.Quit();
    }

    public void SwitchCredits()
    {
        Credits.SetActive(!Credits.activeSelf);
    }
}
