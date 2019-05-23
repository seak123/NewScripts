using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CreatureData : ScriptableObject
{
    public int id;

    public int uid;

    public int type = 0;

    public int con_type = 0;

    public int contain_num = 3;

    public int genus = 1;

    public string CreatureName;

    public int ini_star = 1;

    public float hp = 0;

    public float hp_up = 0;

    public float attack = 0;

    public float attack_up = 0;

    public float base_attack_interval = 1;

    public float attack_rate = 0;

    public float defence = 0;

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

    public int block_num = 1;

    public float attack_range = 4;

    public float channal = 1;

    public float ready_time = 0.2f;

    public int cost = 1;

    public int init_room = 0;

    public float live_time = -1;

    public int[] skills;

    public int[] passives;

    public int prefab;

    public int icon;

}
