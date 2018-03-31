using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class scr_MGBattle : MonoBehaviour {

    public static List<scr_Character> OrderBattle = new List<scr_Character>();
    public static List<scr_Character> Enemys = new List<scr_Character>();
    public static scr_Character Player;
    public static bool InGame = false;

    public GameObject BattleOptions;
    public GameObject TargetPanel;
    public Text Actions;

    public GameObject GameOver;

    int IC = 0;

    float delay_turn = 0f;
    float time_enemy_atk = 0f;

	// Use this for initialization
	void Start () {
        OrderBattle.Clear();

    }
	
	// Update is called once per frame
	void Update () {
		if (delay_turn>0f)
        {
            delay_turn -= Time.deltaTime;
            if (delay_turn<=0)
            {
                NextTurn();
            }
        }
        if (time_enemy_atk > 0f)
        {
            time_enemy_atk -= Time.deltaTime;
            if (time_enemy_atk <= 0)
            {
                AttackTarget(Player);
            }
        }
    }

    public void LoadCharacters()
    {
        foreach (scr_Character btl in FindObjectsOfType<scr_Character>())
        {
            if (btl.IsInGame)
            {
                OrderBattle.Add(btl);
                if (btl.IsPlayer)
                {
                    Player = btl;
                } else
                {
                    Enemys.Add(btl);
                }
            }
        }
    }

    public void StartBattle()
    {
        IC = OrderBattle.IndexOf(Player);
        BattleOptions.SetActive(true);
        Actions.gameObject.SetActive(true);
        InGame = true;
    }

    public void SelectTarget()
    {
        if (Enemys.Count>1)
        {
            BattleOptions.SetActive(false);
            TargetPanel.SetActive(true);
            for (int i=0; i<Enemys.Count; i++)
            {
                Enemys[i].MyTrigger.SetActive(true);
            }

        } else
        {
            AttackTarget(Enemys[0]);
        }
    }

    public void OffEnemys()
    {
        for (int i = 0; i < Enemys.Count; i++)
        {
            Enemys[i].MyTrigger.SetActive(false);
        }
    }

    public void AttackTarget(scr_Character target)
    {
        OrderBattle[IC].Attack(Enemys[0].gameObject);
        Actions.text = OrderBattle[IC].gameObject.name+" Ataca a " + target.gameObject.name;
        BattleOptions.SetActive(false);
        TargetPanel.SetActive(false);
        delay_turn = 3f;
    }

    public void NextTurn()
    {
        if (Enemys.Count==0)
        {
            GameOver.SetActive(true);
            GameOver.transform.GetChild(0).gameObject.SetActive(true);
            return;
        }
        if (Player == null)
        {
            GameOver.SetActive(true);
            GameOver.transform.GetChild(1).gameObject.SetActive(true);
            return;
        }

        if (OrderBattle.Count==0)
        {
            return;
        }
        IC++;
        if (IC>= OrderBattle.Count)
        {
            IC = 0;
        }
        Actions.text = "Es Turno de " + OrderBattle[IC].gameObject.name;

        if (!OrderBattle[IC].IsPlayer)
        {
            time_enemy_atk = 3f;
        } else
        {
            BattleOptions.SetActive(true);
        }
    }

    public void Replay()
    {
        SceneManager.LoadScene("Juego");
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
