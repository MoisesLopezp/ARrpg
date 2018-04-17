using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class scr_MGBattle : MonoBehaviour {

    public static List<scr_Character> OrderBattle = new List<scr_Character>();
    public static List<scr_Character> Enemys = new List<scr_Character>();
    public static scr_Character Player;
    public static bool InGame = false;

    public GameObject ScreenLoading;
    public GameObject BattleOptions;
    public GameObject TargetPanel;
    public GameObject SpecialPanel;
    public Text Actions;

    public GameObject fxs_Hit;
    public GameObject fxs_CriticalHit;
    public GameObject fxs_KO;
    public GameObject fxs_Special;
    public GameObject fxs_Defense;
    public GameObject fxs_Winer;

    public GameObject GameOver;

    public Button btn_skill_01;
    public Button btn_skill_02;
    public Button btn_skill_03;

    public Text txt_skill_01;
    public Text txt_skill_02;
    public Text txt_skill_03;

    public AudioSource as_music;
    public AudioSource as_atack;
    public AudioSource as_death;
    public AudioSource as_Win;
    public AudioSource as_Skill;

    public Button[] btn_Enemys = new Button[3];

    int IC = 0;

    float delay_turn = 0f;
    float time_enemy_atk = 0f;

	// Use this for initialization
	void Start () {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        InGame = false;
        OrderBattle.Clear();
        Enemys.Clear();
        if (Menu_Scr.OkSound)
        {
            as_music.Play();
        }
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

    public void CheckOrders()
    {
        if (IC>=OrderBattle.Count)
        {
            IC--;
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
            for (int i = 0; i < btn_Enemys.Length; i++)
            {
                btn_Enemys[i].gameObject.SetActive(false);
            }
            for (int i=0; i<Enemys.Count; i++)
            {
                Enemys[i].MyTrigger.SetActive(true);
                btn_Enemys[i].gameObject.SetActive(true);
                btn_Enemys[i].transform.GetComponentInChildren<Text>().text = Enemys[i].name;
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
        OrderBattle[IC].Attack(target);
        Actions.text = OrderBattle[IC].gameObject.name+" "+scr_Lang.GetText("txt_game_info_14") +" " + target.gameObject.name;
        BattleOptions.SetActive(false);
        TargetPanel.SetActive(false);
        SpecialPanel.SetActive(false);
        delay_turn = 3f;
        if (OrderBattle[IC].DoubleAttack)
            delay_turn = 5f;
    }

    public void AttackTargetName(Text s_target)
    {
        scr_Character target = null;
        for (int i=0; i<Enemys.Count; i++)
        {
            if (Enemys[i].name == s_target.text)
            {
                target = Enemys[i];
                break;
            }
        }
        Actions.text = OrderBattle[IC].gameObject.name + " " + scr_Lang.GetText("txt_game_info_14") + " " + target.gameObject.name;
        BattleOptions.SetActive(false);
        TargetPanel.SetActive(false);
        SpecialPanel.SetActive(false);
        delay_turn = 3f;
        if (OrderBattle[IC].DoubleAttack)
            delay_turn = 5f;
        OrderBattle[IC].Attack(target);
    }

    public void Defense()
    {
        GameObject _fxs = Instantiate(fxs_Defense, OrderBattle[IC].transform.position, Quaternion.identity);
        Destroy(_fxs, 6f);
        OrderBattle[IC].InDefense = true;
        OrderBattle[IC].f_armor++;
        Actions.text = OrderBattle[IC].gameObject.name + " "+scr_Lang.GetText("txt_game_info_15");
        BattleOptions.SetActive(false);
        TargetPanel.SetActive(false);
        SpecialPanel.SetActive(false);
        delay_turn = 3f;
    }

    public void CheckSkills()
    {
        btn_skill_01.interactable = true;
        btn_skill_02.interactable = true;
        btn_skill_03.interactable = true;

        txt_skill_01.text = scr_Lang.GetText("txt_game_info_17");
        txt_skill_02.text = scr_Lang.GetText("txt_game_info_18");
        txt_skill_03.text = scr_Lang.GetText("txt_game_info_19");

        if (OrderBattle[IC].i_stamina < 3)
        {
            btn_skill_01.interactable = false;
            txt_skill_01.text = scr_Lang.GetText("txt_game_info_16");
        }
        if (OrderBattle[IC].i_stamina<6)
        {
            btn_skill_02.interactable = false;
            txt_skill_02.text = scr_Lang.GetText("txt_game_info_16");
        }
        if (OrderBattle[IC].i_stamina <= 0)
        {
            btn_skill_03.interactable = false;
            txt_skill_03.text = scr_Lang.GetText("txt_game_info_16");
        }
    }

    public void CancelDoubleAttack()
    {
        OrderBattle[IC].DoubleAttack = false;
    }

    public void UseSpecial(int Special)
    {
        GameObject _fxs = Instantiate(fxs_Special, OrderBattle[IC].transform.position, Quaternion.identity);
        Destroy(_fxs, 6f);
        if (Menu_Scr.OkSound)
            as_Skill.Play();
        switch (Special)
        {
            case 0:
                {
                    while (OrderBattle[IC].i_stamina > 0)
                    {
                        OrderBattle[IC].f_hp += OrderBattle[IC].f_maxhp * 0.1f;
                        OrderBattle[IC].i_stamina--;
                    }
                    if (OrderBattle[IC].f_hp > OrderBattle[IC].f_maxhp)
                        OrderBattle[IC].f_hp = OrderBattle[IC].f_maxhp;
                    BattleOptions.SetActive(false);
                    SpecialPanel.SetActive(false);
                    delay_turn = 3f;
                    Actions.text = OrderBattle[IC].gameObject.name + " "+ scr_Lang.GetText("txt_game_info_20");
                }
                break;
            case 1:
                {
                    OrderBattle[IC].DoubleAttack = true;
                    SelectTarget();
                    Actions.text = OrderBattle[IC].gameObject.name + " "+ scr_Lang.GetText("txt_game_info_21");
                }
                break;
            case 2:
                {
                    while (OrderBattle[IC].i_stamina > 0)
                    {

                        OrderBattle[IC].i_stamina--;
                        OrderBattle[IC].f_atk += OrderBattle[IC].f_baseatk * 0.2f;
                    }
                    BattleOptions.SetActive(false);
                    SpecialPanel.SetActive(false);
                    delay_turn = 3f;
                    Actions.text = OrderBattle[IC].gameObject.name + " " +scr_Lang.GetText("txt_game_info_22");
                }
                break;
        }
        SpecialPanel.SetActive(false);
    }

    public void NextTurn()
    {
        if (Enemys.Count==0) //Winer
        {
            if (Menu_Scr.OkSound)
                as_Win.Play();
            GameObject _fxs = Instantiate(fxs_Winer, Player.transform.position, Quaternion.identity);
            Destroy(_fxs, 10f);
            GameOver.SetActive(true);
            GameOver.transform.GetChild(0).gameObject.SetActive(true);
            Actions.text = scr_Lang.GetText("txt_game_info_23");
            return;
        }
        if (Player == null) //Defeat
        {
            GameOver.SetActive(true);
            GameOver.transform.GetChild(1).gameObject.SetActive(true);
            Actions.text = scr_Lang.GetText("txt_game_info_24");
            return;
        }

        if (OrderBattle.Count==0)
        {
            return;
        } else
        {
            for (int i = 0; i < OrderBattle.Count; i++)
                OrderBattle[i].i_stamina++;
        }
        IC++;
        if (IC>= OrderBattle.Count)
        {
            IC = 0;
        }
        Actions.text = scr_Lang.GetText("txt_game_info_25")+" " + OrderBattle[IC].gameObject.name;

        if (!OrderBattle[IC].IsPlayer)
        {
            time_enemy_atk = 3f;
        } else
        {
            BattleOptions.SetActive(true);
        }
    }

    public void ImDeath(string _name)
    {
        Actions.text = _name + " "+scr_Lang.GetText("txt_game_info_26");
    }

    public void Replay()
    {
        SceneManager.LoadScene("Juego");
    }

    public void ExitToMenu()
    {
        StartCoroutine(IE_Exit());
    }

    IEnumerator IE_Exit()
    {
        ScreenLoading.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Menu");
    }
}
