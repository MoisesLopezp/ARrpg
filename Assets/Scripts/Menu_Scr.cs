using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Scr : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Juego");
    }
}
