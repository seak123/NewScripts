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
public class GameData{
    //system data
    public int unitUid;
    //player base data

    //player property

    public int roomRow;
    public int roomCol;
    public CreatureFightData boss;
    public List<CreatureFightData> creatures;
    public List<CreatureFightData> constructures;
    public List<CreatureFightData> partTools;

}

[Serializable]
public class PlayerData{
    //hero exp
    public List<int> bossExp;
}


public class GameDataManager
{
    //player data
    public List<int> bossExp;

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

        bossExp = new List<int>();

        timeScaleFlag = 0;
        //init data

        unitUid = 0;
        roomRow = 3;
        roomCol = 3;
        creatures = new List<CreatureFightData>();
        constructures = new List<CreatureFightData>();
        partTools = new List<CreatureFightData>();
        rooms = new Dictionary<int, List<CreatureFightData>>();

        //int uid = GetNewConstructure(CreateNewCreature(601,3));
        ////ChangeRoomConstructure(34, uid, true);

        //uid = GetNewConstructure(CreateNewCreature(601, 2));
        ////ChangeRoomConstructure(32, uid, true);

        //uid = GetNewConstructure(CreateNewCreature(601, 2));

        //GetNewConstructure(CreateNewCreature(601, 2));
        ////ChangeRoomConstructure(43, uid, true);
        //GetNewConstructure(CreateNewCreature(602, 2));

        //GetNewConstructure(CreateNewCreature(602, 2));

        //GetNewConstructure(CreateNewCreature(602, 2));

        //GetNewConstructure(CreateNewCreature(602, 2));

        //GetNewConstructure(CreateNewCreature(602, 2));

        //uid = GetNewConstructure(CreateNewCreature(701, 10));
        ////ChangeRoomConstructure(33, uid, true);

        //uid = GetNewCreature(CreateNewCreature(108, 2));
        ////ChangeRoomSubData(33, 10, uid,true);
        //uid = GetNewCreature(CreateNewCreature(108, 12));
        ////ChangeRoomSubData(33,10,uid,true);
        //uid = GetNewCreature(CreateNewCreature(108, 9));
        ////ChangeRoomSubData(33, 10, uid, true);

        //uid = GetNewCreature(CreateNewCreature(108, 9));
        //uid = GetNewCreature(CreateNewCreature(108, 9));




        //GetNewConstructure(CreateNewCreature(701, 12));

        //GetNewConstructure(CreateNewCreature(701, 2));

        //GetNewConstructure(CreateNewCreature(701, 34));


        ////for (int i = 0; i < 20;++i){
        ////    GetNewConstructure(CreateNewCreature(601, 1));
        ////}


        ////for (int i = 0; i < 20; ++i)
        ////{
        ////    GetNewCreature(CreateNewCreature(108, 1));
        ////}
        //GetNewCreature(CreateNewCreature(302, 10));
        //GetNewCreature(CreateNewCreature(302, 13));
        //GetNewCreature(CreateNewCreature(302, 4));
        //GetNewCreature(CreateNewCreature(302, 89));


        CreatureFightData boss_data = CreateNewCreature(10002, 1);
        boss_data.type = -1;
        boss_data.uid = -1;
        boss_data.init_room = 23;
        boss = boss_data;

        LoadPlayerData();
    }

    //create new data
    public CreatureFightData CreateNewCreature(int id,int level){
        AssetManager assetManager = GameRoot.GetInstance().BattleField.assetManager;
        CreatureFightData temp = new CreatureFightData();
        temp.LoadData(assetManager.GetCreatureData(id));
        temp.level = level;
        temp.hp = temp.hp + (level - 1) * temp.hp_up;
        temp.attack = temp.attack + (level - 1) * temp.attack_up;
        temp.expMax = level * 20;
        temp.exp = level == 1 ? 0 : UnityEngine.Random.Range(0,temp.expMax);
        return temp;
    }
//get new data
    public int GetNewCreature(CreatureFightData creature){
        creature.uid = unitUid;
        creature.init_room = 0;
        ++unitUid;
        creatures.Add(creature);
        return creature.uid;
    }

    public int GetNewConstructure(CreatureFightData constructure){
        constructure.uid = unitUid;
        constructure.init_room = 0;
        ++unitUid;
        constructures.Add(constructure);
        return constructure.uid;
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
        UnitData res=null;
        if(boss!=null)
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

        battleData.enemys = new UnitData[25];

        for (int i = 0; i < 24; ++i)
        {
            CreatureFightData temp = new CreatureFightData();
            temp.LoadData(GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(109));
            if (UnityEngine.Random.Range(0f, 1f) < 0.1f) temp.enemy_level = 1;
            battleData.enemys[i] = AssetManager.PackCreatureData(temp, 2);
            battleData.enemys[i].init_room = -1;
        }
        CreatureFightData e_boss = new CreatureFightData();
        e_boss.LoadData(GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(110));
        e_boss.enemy_level = 2;
        battleData.enemys[24] = AssetManager.PackCreatureData(e_boss, 2);
        battleData.enemys[24].init_room = -1;

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

    private void CleanInRoomData(int roomId,bool rawData = false){
        CreatureFightData roomData = GetInRoomConstructure(roomId);
        List<CreatureFightData> subDatas = GetInRoomSubData(roomId);
        if(roomData!=null){
            int remainNum = roomData.contain_num;
            int conType = roomData.con_type;
            for(int i = subDatas.Count-1;i>=0;--i){
                if(subDatas[i].type!=conType){
                    subDatas[i].init_room = 0;
                    if (subDatas[i].type != 2 && rawData == false)
                        GameRoot.GetInstance().Bridge.RemoveEntity(subDatas[i].uid);
                    subDatas.RemoveAt(i);
                }
            }
            for(int i = subDatas.Count-1;i>=0;--i){
                if(i>remainNum){
                    subDatas[i].init_room = 0;
                    if (subDatas[i].type != 2 && rawData == false)
                        GameRoot.GetInstance().Bridge.RemoveEntity(subDatas[i].uid);
                    subDatas.RemoveAt(i);
                }
            }
        }else{
            for(int i = subDatas.Count-1;i>=0;--i){
                subDatas[i].init_room = 0;
                if (subDatas[i].type != 2 && rawData == false)
                    GameRoot.GetInstance().Bridge.RemoveEntity(subDatas[i].uid);
                subDatas.RemoveAt(i);
            }
        }   
    }

    public void SavePlayerData(){
        PlayerData data = new PlayerData
        {
            bossExp = new List<int>(),
        };
        foreach(var v in bossExp){
            data.bossExp.Add(v);
        }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playersave.save");
        formatter.Serialize(file, data);
        file.Close();
    }

    public void LoadPlayerData()
    {
        PlayerData data;
        if (File.Exists(Application.persistentDataPath + "/playersave.save"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playersave.save", FileMode.Open);
            data = (PlayerData)formatter.Deserialize(file);
            file.Close();
        }else{
            data = new PlayerData
            {
                bossExp = new List<int>(),
            };
            for (int i = 0; i < BattleDef.bossNum;++i){
                data.bossExp.Add(0);
            }
        }

        bossExp = data.bossExp;
    }
    public void SaveData(){

        //format data
        GameData data = new GameData
        {
            unitUid = unitUid,
            roomCol = roomCol,
            roomRow = roomRow,
            boss = boss,
            creatures = new List<CreatureFightData>(),
            constructures = new List<CreatureFightData>(),
            partTools = new List<CreatureFightData>(),
        };

        foreach (var creature in creatures){
            data.creatures.Add(creature);
        }

        foreach(var constructure in constructures){
            data.constructures.Add(constructure);
        }

        foreach(var tool in partTools){
            data.partTools.Add(tool);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        formatter.Serialize(file, data);
        file.Close();
       
    }

    public int LoadData(){
        GameData data;
        if(File.Exists(Application.persistentDataPath + "/gamesave.save")){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            data = (GameData)formatter.Deserialize(file);
            file.Close();

            //load data
            unitUid = data.unitUid;
            roomRow = data.roomRow;
            roomCol = data.roomCol;
            boss = new CreatureFightData();
            creatures = new List<CreatureFightData>();
            constructures = new List<CreatureFightData>();
            partTools = new List<CreatureFightData>();
            rooms = new Dictionary<int, List<CreatureFightData>>();

            boss = data.boss;

            int creatureNum = data.creatures.Count;
            for (int i = 0; i < creatureNum;++i){
                creatures.Add(data.creatures[i]);
                if(data.creatures[i].init_room!=0){
                    if(!rooms.ContainsKey(data.creatures[i].init_room)){
                        rooms.Add(data.creatures[i].init_room, new List<CreatureFightData>());
                    }
                    rooms[data.creatures[i].init_room].Add(data.creatures[i]);
                }
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
        CleanInRoomData(roomId,rawData);
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
                if (newData.type != 2 && rawData == false && newData.init_room != 0)
                    GameRoot.GetInstance().Bridge.AddEntity(AssetManager.PackCreatureData(newData));
                subDatas.Add(newData);
            }
        }
        CleanInRoomData(roomId,rawData);
    }

    private void AddMemOnArray(ref int[] array,int key){
        List<int> cache = new List<int>();
        foreach (var v in array)
        {
            cache.Add(v);
        }
        array = new int[cache.Count + 1];
        for (int i = 0; i < cache.Count; ++i)
        {
            array[i] = cache[i];
        }
        array[cache.Count] = key;
    }

    public void InjectBossSkill(int skillId){
        switch(skillId){
            case 100101:
                AddMemOnArray(ref boss.skills, 10011);
                break;
            case 100102:
                GetNewConstructure(CreateNewCreature(601, 1));
                GetNewConstructure(CreateNewCreature(601, 1));
                break;
        }
    }
}
