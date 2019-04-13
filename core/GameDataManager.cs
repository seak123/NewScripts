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
    //system data
    public int timeScaleFlag;
    //data
    public int unitUid;
    public int roomRow;
    public int roomCol;
    public CreatureFightData boss;
    public List<CreatureFightData> creatures;
    public List<CreatureFightData> constructures;
    public List<CreatureFightData> partTools;

    //temp data
    private Dictionary<int,List<CreatureFightData>> rooms;


    public void InitData(){
        timeScaleFlag = 0;
        //init data

        unitUid = 0;
        roomRow = 3;
        roomCol = 3;
        creatures = new List<CreatureFightData>();
        constructures = new List<CreatureFightData>();
        rooms = new Dictionary<int, List<CreatureFightData>>();

        AssetManager assetManager = GameRoot.GetInstance().BattleField.assetManager;
        CreatureFightData temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(601));
        GetNewConstructure(temp);
        ChangeRoomConstructure(34, temp.uid, true);

        temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(601));
        GetNewConstructure(temp);
        ChangeRoomConstructure(32, temp.uid, true);

        temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(601));
        GetNewConstructure(temp);
        ChangeRoomConstructure(43, temp.uid, true);

        temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(701));
        GetNewConstructure(temp);
        ChangeRoomConstructure(33, temp.uid, true);

        temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(108));
        GetNewCreature(temp);
        ChangeRoomSubData(33, 10, temp.uid,true);
        temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(108));
        GetNewCreature(temp);
        ChangeRoomSubData(33,10,temp.uid,true);
        temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(108));
        GetNewCreature(temp);
        ChangeRoomSubData(33,10,temp.uid,true);




        temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(701));
        GetNewConstructure(temp);

        for (int i = 0; i < 20;++i){
            temp = new CreatureFightData();
            temp.LoadData(assetManager.GetCreatureData(601));
            GetNewConstructure(temp);
        }

        for (int i = 0; i < 20; ++i)
        {
            temp = new CreatureFightData();
            temp.LoadData(assetManager.GetCreatureData(108));
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

//get new data
    public void GetNewCreature(CreatureFightData creature){
        creature.uid = unitUid;
        creature.init_room = 0;
        ++unitUid;
        creatures.Add(creature);
    }

    public void GetNewConstructure(CreatureFightData constructure){
        constructure.uid = unitUid;
        constructure.init_room = 0;
        ++unitUid;
        constructures.Add(constructure);
    }

    public void GetNewPartTool(CreatureFightData partTool){
        partTool.uid = unitUid;
        partTool.init_room = 0;
        ++unitUid;
        partTools.Add(partTool);
    }
//battle data
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
            temp.LoadData(GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(109));
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

    public List<CreatureFightData> GetInRoomSubData(int roomId){
        if(!rooms.ContainsKey(roomId)){
            rooms.Add(roomId,new List<CreatureFightData>());
        }
        return rooms[roomId];
    }

    public CreatureFightData GetCreatureFightDataByUid(int uid){
        for (int i = 0; i < constructures.Count; ++i)
        {
            if (constructures[i].uid == uid)
            {
                return constructures[i];
            }
        }
        for (int i = 0; i < creatures.Count; ++i)
        {
            if (creatures[i].uid == uid)
            {
                return creatures[i];
            }
        }
         for (int i = 0; i < partTools.Count; ++i)
        {
            if (partTools[i].uid == uid)
            {
                return partTools[i];
            }
        }
        return null;
    }

    private void CleanInRoomData(int roomId){
        CreatureFightData roomData = GetInRoomConstructure(roomId);
        List<CreatureFightData> subDatas = GetInRoomSubData(roomId);
        if(roomData!=null){
            int remainNum = roomData.contain_num;
            int conType = roomData.con_type;
            for(int i = subDatas.Count-1;i>=0;--i){
                if(subDatas[i].type!=conType){
                    subDatas[i].init_room = 0;
                    subDatas.RemoveAt(i);
                }
            }
            for(int i = subDatas.Count-1;i>=0;--i){
                if(i>remainNum){
                    subDatas[i].init_room = 0;
                    subDatas.RemoveAt(i);
                }
            }
        }else{
            for(int i = subDatas.Count-1;i>=0;--i){
                subDatas[i].init_room = 0;
                subDatas.RemoveAt(i);
            }
        }   
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
            case PackageType.AllConstructure:
                foreach(var con in constructures){
                    res.Add(con);
                }
                break;
            case PackageType.IdlePartTool:
                foreach(var part in partTools){
                    if(part.init_room == 0){
                        res.Add(part);
                    }
                }
                break;
            case PackageType.AllPartTool:
                foreach(var part in partTools){
                    res.Add(part);
                }
                break;
        }
        return res;
    }

    public void ChangeRoomConstructure(int roomId,int uid,bool rawData = false){
        CreatureFightData oldData = GetInRoomConstructure(roomId);
        if (oldData != null)
        {
            oldData.init_room = 0;
            if (rawData == false)
                GameRoot.GetInstance().Bridge.RemoveEntity(oldData.uid);
        }
        CreatureFightData newData = GetCreatureFightDataByUid(uid);
        if (newData != null) {
            newData.init_room = roomId;
            if (rawData == false)
                GameRoot.GetInstance().Bridge.AddEntity(AssetManager.PackCreatureData(newData));
        }
        CleanInRoomData(roomId);
    }

    public void ChangeRoomSubData(int roomId,int index,int uid,bool rawData = false){
        List<CreatureFightData> subDatas = GetInRoomSubData(roomId);
        CreatureFightData newData = GetCreatureFightDataByUid(uid);
        if (index < subDatas.Count){
            CreatureFightData oldData = subDatas[index];
            if(oldData!=null){
                oldData.init_room = 0;
                if(oldData.type != 2&&rawData==false)
                GameRoot.GetInstance().Bridge.RemoveEntity(oldData.uid);
                subDatas.RemoveAt(index);
            }

            if (newData != null)
            {
                newData.init_room = roomId;
                if(newData.type != 2 && rawData == false)
                GameRoot.GetInstance().Bridge.AddEntity(AssetManager.PackCreatureData(newData));
                subDatas.Insert(index,newData);
            }
        }else{

            if (newData != null)
            {
                newData.init_room = roomId;

                subDatas.Add(newData);
            }
        }
        CleanInRoomData(roomId);
        if (newData.type != 2 && rawData == false&&newData.init_room!=0)
            GameRoot.GetInstance().Bridge.AddEntity(AssetManager.PackCreatureData(newData));
    }
}
