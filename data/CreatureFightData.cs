using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CreatureFightData
{
    public int id;

    public int uid;

    public int type = 0;

    //constructureType: 0,position 2,trap
    public int con_type = 0;
    //0,normal_enemy 1,rare_enemy 2,boss_enemy
    public int enemy_level = 0;

    public int contain_num = 3;

    public int genus = 1;

    public string CreatureName;

    public int star = 1;

    public int level = 1;

    public int exp = 0;

    public int expMax = 0;

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

    public float ready_time = 1;

    public int cost = 1;

    public int init_room = 0;

    public float live_time = -1;

    public int[] skills;

    public int prefab;

    public int icon;

    public void LoadData(CreatureData _data){
        id = _data.id;
        uid = _data.uid;
        type = _data.type;
        con_type = _data.con_type;
        contain_num = _data.contain_num;
        genus = _data.genus;
        CreatureName = _data.CreatureName;
        star = _data.ini_star;
        hp = _data.hp;
        hp_up = _data.hp_up;
        attack = _data.attack;
        attack_up = _data.attack_up;
        base_attack_interval = _data.base_attack_interval;
        attack_rate = _data.attack_rate;
        defence = _data.defence;
        magic_resist = _data.magic_resist;
        crit = _data.crit;
        crit_value = _data.crit_value;
        hit_rate = _data.hit_rate;
        dodge = _data.dodge;
        speed = _data.speed;
        base_speed = _data.base_speed;
        physic_suck = _data.physic_suck;
        magic_suck = _data.magic_suck;
        coold_reduce = _data.coold_reduce;
        radius = _data.radius;
        block_num = _data.block_num;
        attack_range = _data.attack_range;
        channal = _data.channal;
        ready_time = _data.ready_time;
        cost = _data.cost;
        init_room = _data.init_room;
        live_time = _data.live_time;
        prefab = _data.prefab;
        icon = _data.icon;
        skills = new int[_data.skills.Length];
        for (int i = 0; i < skills.Length; ++i)
        {
            skills[i] = _data.skills[i];
        }
    }


}



