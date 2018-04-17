using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Scr : MonoBehaviour
{
    public GameObject Credits;
    public GameObject OKAudio;
    public GameObject ScreenLoading;
    public SkinnedMeshRenderer Hair;
    public static SaveGameFree.scr_DataSave MyData;
    public static string fileName = "PlayerData";
    public static bool OkSound = true;

    public AudioSource as_menu;
    public AudioSource as_select;

    public void Awake()
    {
        
        // Initialize our game data
        MyData = new SaveGameFree.scr_DataSave();

        // Initialize the Saver with the default configurations
        SaveGameFree.Saver.Initialize();
        
        MyData = SaveGameFree.Saver.Load<SaveGameFree.scr_DataSave>(fileName);
        scr_Player.Op_Leng = MyData.Leng;

        scr_Player.i_maxhp = MyData.i_maxhp;
        scr_Player.i_stamina = MyData.i_stamina;
        scr_Player.i_atk = MyData.i_atk;
        scr_Player.i_armor = MyData.i_armor;
        scr_Player.i_critic = MyData.i_critic;
        scr_Player.i_vampiric = MyData.i_vampiric;
        scr_Player.f_colorhair = MyData.f_colorhair;

        Hair.material.color = Color.HSVToRGB(scr_Player.f_colorhair, 0.75f, 1f);

        //Init Lang
        scr_Lang.setLanguage();
    }

    public void SwitchAudio()
    {
        OkSound = !OkSound;
        OKAudio.SetActive(OkSound);
        if (OkSound)
        {
            as_menu.gameObject.SetActive(true);
            as_select.gameObject.SetActive(true);
            as_menu.Play();
        } else
        {
            as_menu.gameObject.SetActive(false);
            as_select.gameObject.SetActive(false);
            as_menu.Stop();
        }
    }

    public void Jugar()
    {
        StartCoroutine(IE_Jugar());
    }


    IEnumerator IE_Jugar()
    {
        ScreenLoading.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Juego");
    }

    public void Salir()
    {

        Application.Quit();
    }

    public static void SaveDataPlayer()
    {
        MyData.i_maxhp = scr_Player.i_maxhp;
        MyData.i_stamina = scr_Player.i_stamina;
        MyData.i_atk = scr_Player.i_atk;
        MyData.i_armor = scr_Player.i_armor;
        MyData.i_critic = scr_Player.i_critic;
        MyData.i_vampiric = scr_Player.i_vampiric;
        MyData.f_colorhair = scr_Player.f_colorhair;
        MyData.Leng = scr_Player.Op_Leng;
        SaveGameFree.Saver.Save(MyData, fileName);
    }

    public void SwitchCredits()
    {
        Credits.SetActive(!Credits.activeSelf);
    }

    public void SetLeng(int Leng)
    {
        scr_Player.Op_Leng = Leng;
        scr_Lang.setLanguage();
        SaveDataPlayer();
    }
}
