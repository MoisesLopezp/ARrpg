using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_Character : MonoBehaviour {

    public float f_maxhp = 100f;
    public float f_hp;
    public float f_atk = 20f;
    [HideInInspector]
    public float f_baseatk = 0;
    public int i_stamina = 5;
    public float f_armor = 0f;
    public float f_critic = 0.1f;
    public float f_vampiric = 0f;
    public bool IsPlayer = false;
    public bool IsInGame = false;

    public Slider HP;
    public Slider MANA;
    public Text DMG;
    public Text ARMOR;
    public Text CRITICAL;
    public Text VAMPIRIC;

    public GameObject MyTrigger;
    public GameObject Canvas;
    public scr_MGBattle Gmb;

    public string EnemyName = "None";

    [HideInInspector]
    public bool IsDead = false;

    [HideInInspector]
    public bool DoubleAttack = false;
    float f_datime = 0f;
    GameObject da_target = null;
    [HideInInspector]
    public bool InDefense = false;

	// Use this for initialization
	void Start () {
        Canvas = transform.GetChild(1).gameObject;

        if (IsPlayer)
        {
            f_maxhp = scr_Player.i_maxhp;
            f_atk = scr_Player.i_atk;
            i_stamina = scr_Player.i_stamina;
            f_armor = scr_Player.i_armor;
            f_critic = ((float)scr_Player.i_critic)*0.01f;
            f_vampiric = ((float)scr_Player.i_vampiric) * 0.01f;
            transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().material.color = Color.HSVToRGB(scr_Player.f_colorhair, 0.75f, 1f);
        } else
        {
            name = scr_Lang.GetText(EnemyName);
        }
        f_hp = f_maxhp;

        HP.maxValue = f_maxhp;
        HP.value = f_hp;
        f_baseatk = f_atk;
    }

    void Update()
    {
        HP.value = f_hp;
        MANA.value = i_stamina;
        if (i_stamina > 10)
            i_stamina = 10;

        DMG.text = ((int)f_atk).ToString();
        ARMOR.text = ((int)f_armor).ToString();
        CRITICAL.text = ((int)(f_critic*100f)).ToString()+"%";
        VAMPIRIC.text = ((int)(f_vampiric* 100f)).ToString() + "%";

        if (f_datime>0f)
        {
            f_datime -= Time.deltaTime;
            if (f_datime<=0)
            {
                f_datime = 0f;
                Attack(da_target);
                da_target = null;
            }
        }
    }

    void CreateFXS(GameObject fxs, float time, Transform pos)
    {
        GameObject _fxs = Instantiate(fxs, pos.position, Quaternion.identity);
        Destroy(_fxs, time);
    }

    public void AddDamage(float dmg)
    {
        dmg -= f_armor;
        if (InDefense)
            dmg -= (dmg * 0.5f) + (dmg * (Random.Range(0.0f, 0.4f)));
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
            CreateFXS(Gmb.fxs_KO, 6f, transform);
            Gmb.ImDeath(name);
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
            i_stamina++;
            CreateFXS(Gmb.fxs_CriticalHit, 6f, Target.transform);
        } else
        {
            CreateFXS(Gmb.fxs_Hit, 6f, Target.transform);
        }
        if (critic && i_stamina<10) { i_stamina++; }
        f_hp += f_atk *= f_vampiric;
        if (f_hp > f_maxhp) { f_hp = f_maxhp; }
        Target.SendMessage("AddDamage", f_atk);
        PlayAttack();
        if (DoubleAttack)
        {
            i_stamina -= 6;
            DoubleAttack = false;
            da_target = Target;
            f_datime = 2f;
        }
    }

    void PlayAttack()
    {
        Animator MyAnim = transform.GetChild(0).GetComponent<Animator>();
        if (MyAnim)
        {
            MyAnim.SetTrigger("Attack");
        } else
        {
            Animation MyAnimation = transform.GetChild(0).GetComponent<Animation>();
            if (transform.GetChild(0).name=="bear")
            {
                MyAnimation.Play("Claws Attack 1");
                MyAnimation.PlayQueued("Idle");
            } else
            {
                MyAnimation.Play("sj001_skill1");
                MyAnimation.PlayQueued("sj001_wait");
            }
        }
    }

    void PlayHit()
    {
        Animator MyAnim = transform.GetChild(0).GetComponent<Animator>();
        if (MyAnim)
        {
            MyAnim.SetTrigger("GetHit");
        }
        else
        {
            Animation MyAnimation = transform.GetChild(0).GetComponent<Animation>();
            if (transform.GetChild(0).name == "bear")
            {
                MyAnimation.Play("Hit");
                MyAnimation.PlayQueued("Idle");
            }
            else
            {
                MyAnimation.Play("sj001_hurt");
                MyAnimation.PlayQueued("sj001_wait");
            }
        }
    }

    void PlayDeath()
    {
        Animator MyAnim = transform.GetChild(0).GetComponent<Animator>();
        if (MyAnim)
        {
            MyAnim.SetBool("Die",true);
        }
        else
        {
            Animation MyAnimation = transform.GetChild(0).GetComponent<Animation>();
            if (transform.GetChild(0).name == "bear")
            {
                MyAnimation.Play("Death");
            }
            else
            {
                MyAnimation.Play("sj001_die");
            }
        }
    }
}
