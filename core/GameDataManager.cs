using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Data;
using System;

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
[Serializable]
public class PlayerData{
    //system data
    public int unitUid;
    //player base data

    //player property

    public int roomRow;
    public int roomCol;
    public CreatureData4Save boss;
    public List<CreatureData4Save> creatures;
    public List<CreatureData4Save> constructures;

}


public class GameDataManager
{
    //data
    public int unitUid;
    public int roomRow;
    public int roomCol;
    public CreatureData boss;
    public List<CreatureData> creatures;
    public List<CreatureData> constructures;

    public void InitData(){
        //init data

        unitUid = 0;
        roomRow = 3;
        roomCol = 3;
        creatures = new List<CreatureData>();
        constructures = new List<CreatureData>();

        AssetManager assetManager = GameRoot.GetInstance().BattleField.assetManager;
        GetNewCreature(assetManager.GetCreatureData(1081),33);
        GetNewCreature(assetManager.GetCreatureData(1081),33);
        GetNewCreature(assetManager.GetCreatureData(1081),33);
     

        GetNewConstructure(assetManager.GetCreatureData(6011),32);

        GetNewConstructure(assetManager.GetCreatureData(6011),34);

        GetNewConstructure(assetManager.GetCreatureData(6011),43);

        CreatureData boss_data = assetManager.GetCreatureData(10002);
        boss_data.type = -1;
        boss_data.uid = -1;
        boss_data.init_room = 23;
        boss = boss_data;

        SaveData();

    }

    public void GetNewCreature(CreatureData creature,int initRoom = 0){
        CreatureData data = CreatureData.Clone(creature);
        data.uid = unitUid;
        data.init_room = initRoom;
        ++unitUid;
        creatures.Add(data);
    }

    public void GetNewConstructure(CreatureData constructure,int initRoom = 0){
        CreatureData data = CreatureData.Clone(constructure);
        data.uid = unitUid;
        data.init_room = initRoom;
        ++unitUid;
        constructures.Add(data);
    }

    public List<UnitData> GetBattleCreatures(){
        List<UnitData> res = new List<UnitData>();
        foreach(CreatureData data in creatures){
            if(data.init_room != 0){
                res.Add(AssetManager.PackCreatureData(data));
            }
        }
        return res;
    }

    public List<UnitData> GetBattleConstructures(){
        List<UnitData> res = new List<UnitData>();
        foreach(CreatureData data in constructures){
            if(data.init_room !=0){
                res.Add(AssetManager.PackCreatureData(data));
            }
        }
        return res;
    }


    public UnitData GetHeroData(){
        UnitData res;

        res = AssetManager.PackCreatureData(boss);
        return res;
    }

    public BattleData GetBattleData(){
        BattleData battleData = new BattleData();

        List<UnitData> allStructures = GetBattleConstructures();
        List<UnitData> allCreatures = GetBattleCreatures();

        int num = allStructures.Count + allCreatures.Count;
        battleData.units = new UnitData[num];

        battleData.roomCol = roomCol;
        battleData.roomRow = roomRow;

        for (int i = 0; i < allStructures.Count; ++i)
        {
            battleData.units[i] = allStructures[i];
        }

        for (int i = 0; i < allCreatures.Count; ++i)
        {
            battleData.units[allStructures.Count + i] = allCreatures[i];
        }

        battleData.enemys = new UnitData[20];

        for (int i = 0; i < 20; ++i)
        {
            battleData.enemys[i] = AssetManager.PackCreatureData(GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(1091), 2);
            battleData.enemys[i].init_room = -1;
        }

        battleData.boss = GetHeroData();

        return battleData;
    }

    public void SaveData(){

        //format data
        PlayerData data = new PlayerData();
        data.unitUid = unitUid;
        data.roomCol = roomCol;
        data.roomRow = roomRow;
        data.boss = boss.Save2Data();
        data.creatures = new List<CreatureData4Save>();
        data.constructures = new List<CreatureData4Save>();

        foreach(var creature in creatures){
            data.creatures.Add(creature.Save2Data());
        }

        foreach(var constructure in constructures){
            data.constructures.Add(constructure.Save2Data());
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        formatter.Serialize(file, data);
        file.Close();
       
    }

    public int LoadData(){
        PlayerData data;
        if(File.Exists(Application.persistentDataPath + "/gamesave.save")){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            data = (PlayerData)formatter.Deserialize(file);
            file.Close();

            //load data
            unitUid = data.unitUid;
            roomRow = data.roomRow;
            roomCol = data.roomCol;
            boss = new CreatureData();
            creatures = new List<CreatureData>();
            constructures = new List<CreatureData>();

            boss.LoadData(data.boss);

            int creatureNum = data.creatures.Count;
            for (int i = 0; i < creatureNum;++i){
                CreatureData temp = new CreatureData();
                temp.LoadData(data.creatures[i]);
                creatures.Add(temp);
            }

            int constructureNum = data.constructures.Count;
            for (int i = 0; i < constructureNum; ++i)
            {
                CreatureData temp = new CreatureData();
                temp.LoadData(data.constructures[i]);
                constructures.Add(temp);
            }

            return 1;
        }
        return -1;
    }
 
}
