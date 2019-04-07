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
    public CreatureFightData boss;
    public List<CreatureFightData> creatures;
    public List<CreatureFightData> constructures;

}


public class GameDataManager
{
    //data
    public int unitUid;
    public int roomRow;
    public int roomCol;
    public CreatureFightData boss;
    public List<CreatureFightData> creatures;
    public List<CreatureFightData> constructures;


    public void InitData(){
        //init data

        unitUid = 0;
        roomRow = 3;
        roomCol = 3;
        creatures = new List<CreatureFightData>();
        constructures = new List<CreatureFightData>();

        AssetManager assetManager = GameRoot.GetInstance().BattleField.assetManager;
        CreatureFightData temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(1081));
        GetNewCreature(temp,33);
        temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(1081));
        GetNewCreature(temp,33);
        temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(1081));
        GetNewCreature(temp,33);


        temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(6011));
        GetNewConstructure(temp,32);

        temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(6011));
        GetNewConstructure(temp,34);

        temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(6011));
        GetNewConstructure(temp,43);

        for (int i = 0; i < 20;++i){
            temp = new CreatureFightData();
            temp.LoadData(assetManager.GetCreatureData(6011));
            GetNewConstructure(temp);
        }

        for (int i = 0; i < 20; ++i)
        {
            temp = new CreatureFightData();
            temp.LoadData(assetManager.GetCreatureData(1081));
            GetNewCreature(temp);
        }

        temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(10002));
        CreatureFightData boss_data = temp;
        boss_data.type = -1;
        boss_data.uid = -1;
        boss_data.init_room = 23;
        boss = boss_data;

        SaveData();

    }

    public void GetNewCreature(CreatureFightData creature,int initRoom = 0){
        creature.uid = unitUid;
        creature.init_room = initRoom;
        ++unitUid;
        creatures.Add(creature);
    }

    public void GetNewConstructure(CreatureFightData constructure,int initRoom = 0){
        constructure.uid = unitUid;
        constructure.init_room = initRoom;
        ++unitUid;
        constructures.Add(constructure);
    }

    public List<UnitData> GetBattleCreatures(){
        List<UnitData> res = new List<UnitData>();
        foreach(CreatureFightData data in creatures){
            if(data.init_room != 0){
                res.Add(AssetManager.PackCreatureData(data));
            }
        }
        return res;
    }

    public List<UnitData> GetBattleConstructures(){
        List<UnitData> res = new List<UnitData>();
        foreach(CreatureFightData data in constructures){
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
            CreatureFightData temp = new CreatureFightData();
            temp.LoadData(GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(1091));
            battleData.enemys[i] = AssetManager.PackCreatureData(temp, 2);
            battleData.enemys[i].init_room = -1;
        }

        battleData.boss = GetHeroData();

        return battleData;
    }

    //view function
    public CreatureFightData GetInRoomConstructure(int roomId){
        for (int i = 0; i < constructures.Count; ++i)
        {
            if(constructures[i].init_room == roomId){
                return constructures[i];
            }
        }
        return null;
    }

    public List<CreatureFightData> GetInRoomCreature(int roomId){
        List<CreatureFightData> list = new List<CreatureFightData>();
        for (int i = 0; i < creatures.Count;++i){
            if(creatures[i].init_room == roomId){
                list.Add(creatures[i]);
            }
        }
        return list;
    }

    public CreatureFightData GetCreatureFightDataByUid(int uid){
        for (int i = 0; i < constructures.Count; ++i)
        {
            if (constructures[i].uid == uid)
            {
                return constructures[i];
            }
        }
        return null;
    }

    public void SaveData(){

        //format data
        PlayerData data = new PlayerData
        {
            unitUid = unitUid,
            roomCol = roomCol,
            roomRow = roomRow,
            boss = boss,
            creatures = new List<CreatureFightData>(),
            constructures = new List<CreatureFightData>()
        };

        foreach (var creature in creatures){
            data.creatures.Add(creature);
        }

        foreach(var constructure in constructures){
            data.constructures.Add(constructure);
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
            boss = new CreatureFightData();
            creatures = new List<CreatureFightData>();
            constructures = new List<CreatureFightData>();

            boss = data.boss;

            int creatureNum = data.creatures.Count;
            for (int i = 0; i < creatureNum;++i){
                creatures.Add(data.creatures[i]);
            }

            int constructureNum = data.constructures.Count;
            for (int i = 0; i < constructureNum; ++i)
            {
                constructures.Add(data.constructures[i]);
            }

            return 1;
        }
        return -1;
    }

    public List<CreatureFightData> GetPackageList(PackageType type){
        List<CreatureFightData> res = new List<CreatureFightData>();
        switch(type){
            case PackageType.IdleCreature:
                foreach(var unit in creatures){
                    if(unit.init_room == 0){
                        res.Add(unit);
                    }
                }
                break;
            case PackageType.AllCreature:
                foreach(var unit in creatures){
                    res.Add(unit);
                }
                break;
            case PackageType.IdleConstructure:
                foreach(var con in constructures){
                    if (con.init_room == 0)
                        res.Add(con);
                }
                break;
        }
        return res;
    }

    public void ChangeRoomConstructure(int roomId,int uid){
        CreatureFightData data = GetInRoomConstructure(roomId);
        if (data != null)
        {
            data.init_room = 0;
            GameRoot.GetInstance().Bridge.RemoveEntity(data.uid);
        }
        data = GetCreatureFightDataByUid(uid);
        if (data != null) {
            data.init_room = roomId;
            GameRoot.GetInstance().Bridge.AddEntity(AssetManager.PackCreatureData(data));
        }
    }
 
}
