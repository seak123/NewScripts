using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroData : ScriptableObject
{
    public int id;

    public int type = -1;

    public int genus = 1;

    public int opposite_type = 1;

    public string creatureName;

    public float base_hp = 0;

    public float add_hp = 0;

    public float base_attack = 0;

    public float add_attack = 0;

    public float base_attack_rate = 0;

    public float add_attack_rate = 0;

    public float base_defence = 0;

    public float add_defence = 0;

    public float magic_resist = 0;

    public float crit = 0;

    public float crit_value = 1;

    public float hit_rate = 0;

    public float dodge = 0;

    public float speed = 0;

    public float base_speed = 4;

    public float physic_suck = 0;

    public float magic_suck = 0;

    public float coold_reduce = 0;

    public int radius = 4;

    public float attack_range = 4;

    public float channal = 1;

    public float ready_time = 1;

    public int cost = 1;

    public float live_time = -1;

    public int[] skills;

    public GameObject prefab;

    //card info

    public string card_name = "none";

    public int race = 0;

    public Sprite icon;

    public GameObject heroPrefab;

}
