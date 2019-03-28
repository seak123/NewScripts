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
    public UnitData boss;
    public List<UnitData> creatures;
    public List<UnitData> constructures;

    public int roomRow;
    public int roomCol;
    //public UnitData[] 
    public int heroId;


    public void InitData(){
        //init data
        properties = new List<float>();
        creatures = new List<UnitData>();
        constructures = new List<UnitData>();
        roomRow = 3;
        roomCol = 3;

        AssetManager assetManager = GameRoot.GetInstance().BattleField.assetManager;
        creatures.Add(assetManager.GetUnitData(1081));
        creatures.Add(assetManager.GetUnitData(1081));
        creatures.Add(assetManager.GetUnitData(1081));
     

        for (int i = 0; i < creatures.Count;++i){
            creatures[i].init_room = 33;
        }

        creatures.Add(assetManager.GetUnitData(6011));
        creatures[3].init_room = 32;

        creatures.Add(assetManager.GetUnitData(6011));
        creatures[4].init_room = 34;

        creatures.Add(assetManager.GetUnitData(6011));
        creatures[5].init_room = 43;

        UnitData boss_data = assetManager.GetUnitData(10002);
        boss_data.type = -1;
        boss_data.init_room = 23;
        boss = boss_data;

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
           
        };

        return data;
    }
 
}
