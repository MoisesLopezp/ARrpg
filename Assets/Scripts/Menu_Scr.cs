using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Scr : MonoBehaviour
{
    public GameObject Credits;
    public SkinnedMeshRenderer Hair;
    public static SaveGameFree.scr_DataSave MyData;
    public static string fileName = "PlayerData";
    
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

    public void Jugar()
    {
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
