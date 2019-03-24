using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public enum PlayerProperty{
    PlayerHp = 1,
    PlayerMaxHp = 2,
    MagicAttack = 3,
    Defence = 4,
    MagicResist = 5,
    HeroHp = 10,
    HeroAttack = 11,
    HeroAttackRate = 12,
    HeroDefence = 13,
    HeroMagicResist = 14,
    HeroCrit = 15,
    HeroCritValue = 16,
    HeroHitRate = 17,
    HeroDodge = 18,
    HeroSpeed = 19,
    HeroPhysicSuck = 20,
    HeroMagicSuck = 21,
    HeroCooldReduce = 22,
}


public class GameDataManager
{
    //property
    public List<float> properties;
    public List<UnitData> creatures;
    public List<UnitData> constructures;
    //public UnitData[] 
    public int heroId;

    //hero data
    public int heroLvl = 12;
    public int Skill1Lvl =2;
    public int Skill2Lvl =0;
    public int Skill3Lvl =0;
    public int Skill4Lvl =0;
    public int Skill5Lvl = 0;
    //enhance data
    public int Hero1Lvl =0;
    public int Hero2Lvl = 0;
    public int Hero3Lvl = 0;


    public void InitData(){
        //init data
        properties = new List<float>();
        creatures = new List<UnitData>();
        constructures = new List<UnitData>();

        AssetManager assetManager = GameRoot.GetInstance().BattleField.assetManager;
        creatures.Add(assetManager.GetUnitData(1081));
        creatures.Add(assetManager.GetUnitData(1081));
        creatures.Add(assetManager.GetUnitData(1081));
     

        for (int i = 0; i < creatures.Count;++i){
            creatures[i].init_room = 22;
        }

        creatures.Add(assetManager.GetUnitData(6011));
        creatures[3].init_room = 21;

        creatures.Add(assetManager.GetUnitData(6011));
        creatures[4].init_room = 23;

        creatures.Add(assetManager.GetUnitData(6011));
        creatures[5].init_room = 32;

        //init playerData
        //properties[(int)PlayerProperty.PlayerHp] = 1000;
        //properties[(int)PlayerProperty.PlayerMaxHp] = 1000;
        //properties[(int)PlayerProperty.MagicAttack] = 1;
        //properties[(int)PlayerProperty.Defence] = 10;
        //properties[(int)PlayerProperty.MagicResist] = 0.5f;
        //for (int i = 9; i < properties.Count;++i){
        //    properties[i] = 0;
        //}

        ////temp
        //heroId = 10001;

    }

    public List<UnitData> GetBattleCreatures(){
        List<UnitData> res = new List<UnitData>();
        foreach(UnitData data in creatures){
            if(data.init_room != 0){
                res.Add(data);
            }
        }
        return res;
    }

    public List<UnitData> GetBattleConstructures(){
        List<UnitData> res = new List<UnitData>();
        foreach(UnitData data in constructures){
            if(data.init_room !=0){
                res.Add(data);
            }
        }
        return res;
    }


    public CreatureData GetHeroData(){
        AssetManager assetManager = GameRoot.GetInstance().BattleField.assetManager;
        HeroData heroData = assetManager.GetHeroData(heroId);
        CreatureData data = new CreatureData
        {
            id = heroId,
            type = -1,
            opposite_type = heroData.opposite_type,
            CreatureName = heroData.creatureName,
            hp = heroData.base_hp + heroLvl * heroData.add_hp + properties[(int)PlayerProperty.HeroHp],
            attack = heroData.base_attack + heroLvl*heroData.add_attack + properties[(int)PlayerProperty.HeroAttack],
            attack_rate = heroData.base_attack_rate + heroLvl*heroData.add_attack_rate + properties[(int)PlayerProperty.HeroAttackRate],
            defence = heroData.base_defence + heroLvl*heroData.add_defence + properties[(int)PlayerProperty.HeroDefence],
            magic_resist = properties[(int)PlayerProperty.HeroMagicResist],
            crit = properties[(int)PlayerProperty.HeroCrit],
            crit_value = properties[(int)PlayerProperty.HeroCritValue],
            hit_rate = properties[(int)PlayerProperty.HeroHitRate],
            dodge = properties[(int)PlayerProperty.HeroDodge],
            speed = heroData.speed + properties[(int)PlayerProperty.HeroSpeed],
            base_speed = heroData.base_speed,
            physic_suck = properties[(int)PlayerProperty.HeroPhysicSuck],
            magic_suck = properties[(int)PlayerProperty.HeroMagicSuck],
            coold_reduce = properties[(int)PlayerProperty.HeroCooldReduce],
            radius = heroData.radius,
            attack_range = heroData.attack_range,
            channal = heroData.channal,
            ready_time = heroData.ready_time,
            cost = 10 + 5*heroLvl,
            prefab = heroData.heroPrefab,
            Skill1Lvl = Skill1Lvl,
            Skill2Lvl = Skill2Lvl,
            Skill3Lvl = Skill3Lvl
        };
        List<int> skillList = new List<int>();
        if(Skill1Lvl>0){
            skillList.Add(heroId * 100 + Skill1Lvl);
        }
        if (Skill2Lvl > 0)
        {
            skillList.Add(heroId * 100 + 4 + Skill2Lvl);
        }
        if (Skill3Lvl > 0)
        {
            skillList.Add(heroId * 100 + 8 + Skill3Lvl);
        }
        data.skills = new int[skillList.Count];
        for (int i = 0; i < skillList.Count;++i){
            data.skills[i] = skillList[i];
        }


        return data;
    }

    public CardData GetHeroCardData(){
        AssetManager assetManager = GameRoot.GetInstance().BattleField.assetManager;
        HeroData heroData = assetManager.GetHeroData(heroId);
        CardData data = new CardData
        {
            cardId = heroId,
            cardName = heroData.card_name,
            cardType = CardType.Hero,
            race = heroData.race,
            icon = heroData.icon,
            entityPrefab = heroData.heroPrefab,
            skillId = 1,
            cost = 10 + 5 * heroLvl,
            liveTime = -1,
            num = 1

        };
        return data;
    }


       
}
