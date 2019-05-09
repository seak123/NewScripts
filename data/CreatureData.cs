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

    public float ready_time = 1;

    public int cost = 1;

    public int init_room = 0;

    public float live_time = -1;

    public int[] skills;

    public int prefab;

    public int icon;


    //public static CreatureData Clone(CreatureData data)
    //{
    //    CreatureData newData = new CreatureData
    //    {
    //        id = data.id,
    //        uid = data.uid,
    //        type = data.type,
    //        genus = data.genus,
    //        CreatureName = data.CreatureName,
    //        hp = data.hp,
    //        hp_up = data.hp_up,
    //        attack = data.attack,
    //        attack_up = data.attack_up,
    //        base_attack_interval = data.base_attack_interval,
    //        attack_rate = data.attack_rate,
    //        defence = data.defence,
    //        magic_resist = data.magic_resist,
    //        crit = data.crit,
    //        crit_value = data.crit_value,
    //        hit_rate = data.hit_rate,
    //        dodge = data.dodge,
    //        speed = data.speed,
    //        base_speed = data.base_speed,
    //        physic_suck = data.physic_suck,
    //        magic_suck = data.magic_suck,
    //        coold_reduce = data.coold_reduce,
    //        radius = data.radius,
    //        attack_range = data.attack_range,
    //        channal = data.channal,
    //        ready_time = data.ready_time,
    //        cost = data.cost,
    //        init_room = data.init_room,
    //        live_time = data.live_time,
    //        prefab = data.prefab,
           
    //    };
    //    newData.skills = new int[data.skills.Length];
    //    for (int i = 0; i < newData.skills.Length;++i){
    //        newData.skills[i] = data.skills[i];
    //    }
    //    return newData;
    //}

    //public CreatureData4Save Save2Data(){
    //    CreatureData4Save data = new CreatureData4Save();
    //    data.id = id;
    //    data.uid = uid;
    //    data.type = type;
    //    data.genus = genus;
    //    data.CreatureName = CreatureName;
    //    data.hp = hp;
    //    data.hp_up = hp_up;
    //    data.attack = attack;
    //    data.attack_up = attack_up;
    //    data.base_attack_interval = base_attack_interval;
    //    data.attack_rate = attack_rate;
    //    data.defence = defence;
    //    data.magic_resist = magic_resist;
    //    data.crit = crit;
    //    data.crit_value = crit_value;
    //    data.hit_rate = hit_rate;
    //    data.dodge = dodge;
    //    data.speed = speed;
    //    data.base_speed = base_speed;
    //    data.physic_suck = physic_suck;
    //    data.magic_suck = magic_suck;
    //    data.coold_reduce = coold_reduce;
    //    data.radius = radius;
    //    data.attack_range = attack_range;
    //    data.channal = channal;
    //    data.ready_time = ready_time;
    //    data.cost = cost;
    //    data.init_room = init_room;
    //    data.live_time = live_time;
    //    data.prefab = prefab;
    //    data.skills = new int[skills.Length];
    //    for (int i = 0; i < skills.Length; ++i)
    //    {
    //        data.skills[i] = skills[i];
    //    }
    //    return data;
    //}

    //public void LoadData(CreatureData4Save _data){
    //    id = _data.id;
    //    uid = _data.uid;
    //    type = _data.type;
    //    genus = _data.genus;
    //    CreatureName = _data.CreatureName;
    //    hp = _data.hp;
    //    hp_up = _data.hp_up;
    //    attack = _data.attack;
    //    attack_up = _data.attack_up;
    //    base_attack_interval = _data.base_attack_interval;
    //    attack_rate = _data.attack_rate;
    //    defence = _data.defence;
    //    magic_resist = _data.magic_resist;
    //    crit = _data.crit;
    //    crit_value = _data.crit_value;
    //    hit_rate = _data.hit_rate;
    //    dodge = _data.dodge;
    //    speed = _data.speed;
    //    base_speed = _data.base_speed;
    //    physic_suck = _data.physic_suck;
    //    magic_suck = _data.magic_suck;
    //    coold_reduce = _data.coold_reduce;
    //    radius = _data.radius;
    //    attack_range = _data.attack_range;
    //    channal = _data.channal;
    //    ready_time = _data.ready_time;
    //    cost = _data.cost;
    //    init_room = _data.init_room;
    //    live_time = _data.live_time;
    //    prefab = _data.prefab;
    //    skills = new int[_data.skills.Length];
    //    for (int i = 0; i < skills.Length; ++i)
    //    {
    //        skills[i] = _data.skills[i];
    //    }
    //}
       
}
