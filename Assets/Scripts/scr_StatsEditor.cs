using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class scr_StatsEditor : MonoBehaviour {

    public Slider ColorHair;
    public Slider Hp;
    public Slider Mana;
    public Slider Ataque;
    public Slider Critico;
    public Slider Vampire;
    public Slider Armadura;

    public Text t_Hp;
    public Text t_Mana;
    public Text t_Ataque;
    public Text t_Critico;
    public Text t_Armadura;
    public Text t_Vampire;

    Material Cabello;

    bool IsLoading = true;

    public GameObject UniatChan;

	// Use this for initialization
	void Start () {
        Cabello = UniatChan.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;

        IsLoading = true;

        Hp.value = scr_Player.i_maxhp;
        Mana.value = scr_Player.i_stamina;
        Ataque.value = scr_Player.i_atk;
        Armadura.value = scr_Player.i_armor;
        Critico.value = scr_Player.i_critic;
        Vampire.value = scr_Player.i_vampiric;
        ColorHair.value = scr_Player.f_colorhair;

        IsLoading = false;

        UpdateUIStats();

    }

    public void UpdateStats()
    {
        scr_Player.f_colorhair = ColorHair.value;
        scr_Player.i_maxhp = (int)Hp.value;
        scr_Player.i_stamina = (int)Mana.value;
        scr_Player.i_atk = (int)Ataque.value;
        scr_Player.i_armor = (int)Armadura.value;
        scr_Player.i_critic = (int)Critico.value;
        scr_Player.i_vampiric = (int)Vampire.value;
        Menu_Scr.SaveDataPlayer();
        UpdateUIStats();
    }

    public void ChangeHp()
    {
        if (IsLoading || EventSystem.current.currentSelectedGameObject!=Hp.gameObject)
            return;
        Mana.value = 110 - (Hp.value / 2);
        UpdateStats();
    }

    public void ChangeMana()
    {
        if (IsLoading || EventSystem.current.currentSelectedGameObject != Mana.gameObject)
            return;
        Hp.value = 220 - (Mana.value * 2);
        UpdateStats();
    }

    public void ChangeAtk()
    {
        if (IsLoading || EventSystem.current.currentSelectedGameObject != Ataque.gameObject)
            return;
        Armadura.value = 10 - (Ataque.value * 0.2f);
        UpdateStats();
    }

    public void ChangeArm()
    {
        if (IsLoading || EventSystem.current.currentSelectedGameObject != Armadura.gameObject)
            return;
        Ataque.value = 55 - (Armadura.value * 5);
        UpdateStats();
    }

    public void ChangeCritical()
    {
        if (IsLoading || EventSystem.current.currentSelectedGameObject != Critico.gameObject)
            return;
        Vampire.value = 50 - (Critico.value);
        UpdateStats();
    }

    public void ChangeVampire()
    {
        if (IsLoading || EventSystem.current.currentSelectedGameObject != Vampire.gameObject)
            return;
        Critico.value = 50 - (Vampire.value);
        UpdateStats();
    }

    public void UpdateUIStats()
    {

        Cabello.color = Color.HSVToRGB(ColorHair.value, 0.75f, 1f);
        t_Hp.text = ((int)Hp.value).ToString();
        t_Mana.text = ((int)Mana.value).ToString();
        t_Ataque.text = ((int)Ataque.value).ToString();
        t_Armadura.text = ((int)Armadura.value).ToString();
        t_Critico.text = ((int)Critico.value).ToString() + "%";
        t_Vampire.text = ((int)Vampire.value).ToString() + "%";

    }

    void OnDisable()
    {
        if (UniatChan!=null)
            UniatChan.transform.position = new Vector3(0.0f, -8.5f, 0f);
    }

    void OnEnable()
    {
        UniatChan.transform.position = new Vector3(5.5f, -8.5f, 0f);
    }
}
