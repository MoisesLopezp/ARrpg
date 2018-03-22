﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_Character : MonoBehaviour {

    public float f_maxhp = 100f;
    [HideInInspector]
    public float f_hp;
    public float f_atk = 20f;
    public int i_stamina = 5;
    public float f_armor = 0f;
    public float f_critic = 0.1f;
    public float f_vampiric = 0f;
    public bool IsPlayer = false;
    public bool IsInGame = false;

    public Slider HP;
    public GameObject MyTrigger;
    public GameObject Canvas;

    [HideInInspector]
    public bool IsDead = false;

	// Use this for initialization
	void Start () {
        Canvas = transform.GetChild(1).gameObject;

        if (IsPlayer)
        {
            f_maxhp = scr_Player.f_maxhp;
            f_atk = scr_Player.f_atk;
            i_stamina = scr_Player.i_stamina;
            f_armor = scr_Player.f_armor;
            f_critic = scr_Player.f_critic;
            f_vampiric = scr_Player.f_vampiric;
        }
        f_hp = f_maxhp;

        HP.maxValue = f_maxhp;
        HP.value = f_hp;
    }

    void Upate()
    {
        HP.value = f_hp;
    }

    public void AddDamage(float dmg)
    {
        dmg -= f_armor;
        if (dmg < 0) { dmg = 1f; }
        f_hp -= dmg;
        if (f_hp <= 0)
        {
            IsDead = true;
            scr_MGBattle.OrderBattle.Remove(this);
            if (IsPlayer)
            {
                scr_MGBattle.Player = null;
            } else
            {
                scr_MGBattle.Enemys.Remove(this);
            }
            f_hp = 0f;
            PlayDeath();
            if (scr_MGBattle.OrderBattle.Contains(this))
            {
                scr_MGBattle.OrderBattle.Remove(this);
            }
        } else
        {
            PlayHit();
        }
    }

    public void Attack(GameObject Target)
    {
        bool critic = false;
        if (Random.Range(0f,1f)<=f_critic)
        {
            critic = true;
            f_atk *= 1.5f;
        }
        if (critic && i_stamina<10) { i_stamina++; }
        f_hp += f_atk *= f_vampiric;
        if (f_hp > f_maxhp) { f_hp = f_maxhp; }
        Target.SendMessage("AddDamage", f_atk);
        PlayAttack();
    }

    void PlayAttack()
    {

    }

    void PlayHit()
    {

    }

    void PlayDeath()
    {

    }
}
