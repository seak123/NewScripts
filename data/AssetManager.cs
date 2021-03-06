﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class AssetManager : ScriptableObject
{
    public HeroData[] heroDatas;

    public CreatureData[] creatures;

    public EffectData[] effects;

    public SkillData[] skills;

    public GameObject[] Fields;

    public GameObject[] UnitPrefabs;

    public Sprite[] icons;

    public Sprite[] cards;

    public GameObject RedSlider;
    public GameObject GreenSlider;
    public GameObject Message;
    public GameObject GoldTips;

    public HeroData GetHeroData(int id){
        foreach (var data in heroDatas)
        {
            if (data.id == id) return data;
        }
        return null;
    }

    public CreatureData GetCreatureData(int id){
        return id > 1000 ? HeroData.PackHeroData2(GetHeroData(id)) : FindDataById(id);
    }

    private CreatureData FindDataById(int key){
        foreach(var data in creatures){
            if (data.id == key) return data;
        }
        return null;
    }

    public EffectData GetEffectData(int id){
        return FindEffectDataById(id);
    }
    private EffectData FindEffectDataById(int key){
        foreach(var data in effects){
            if (data.effectId == key) return data;
        }
        return null;
    }

    public SkillData GetSkillData(int id){
        return FindSkillDataById(id);
    }
    private SkillData FindSkillDataById(int key){
        foreach(var data in skills){
            if (data.skill_id == key) return data;
        }
        return null;
    }

    public GameObject GetField(int id){
        return Fields[id];
    }

    public GameObject GetUnitPrefab(int id){
        return UnitPrefabs[id];
    }

    public Sprite GetIcon(int id){
        return icons[id];
    }

    public Sprite GetCards(int id){
        return cards[id];
    }


    public static UnitData PackCreatureData(CreatureFightData data,int _side = 1){
        UnitData pack = new UnitData
        {
            id = data.id,
            level = data.level,
            uid = data.uid,
            type = data.type,
            con_type = data.con_type,
            enemy_level = data.enemy_level,
            side = _side,
            genus = data.genus,
            name = data.CreatureName,
            hp = data.hp,
            attack = data.attack,
            base_attack_interval = data.base_attack_interval,
            attack_rate = data.attack_rate,
            defence = data.defence,
            magic_resist = data.magic_resist,
            crit = data.crit,
            crit_value = data.crit_value,
            hit_rate = data.hit_rate,
            dodge = data.dodge,
            speed = data.speed,
            physic_suck = data.physic_suck,
            magic_suck = data.magic_suck,
            coold_reduce = data.coold_reduce,
            radius = data.radius,
            block_num = data.block_num,
            attack_range = data.attack_range,
            channal = data.channal,
            ready_time = data.ready_time,
            live_time = data.live_time,
            skills = data.skills,
            init_room = data.init_room,
        };


        return pack;
    }
}
