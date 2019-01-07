using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PowerInject;


[Insert]
public class DungeonManager : MonoBehaviour {

    private Dictionary<int, IDungeonUnit> dungeonUnits;
    private int dungeonSize;

    public DungeonCreaterData[] initData;


    public int GetSize(){
        return dungeonSize;
    }


    [OnInjected]
    public void AddRootAction()
    {
        GameRoot.init += Init;
    }

    private void Init()
    {
        Debug.Log("DungeonManager Init");
        InitDungeon();

    }


    // Use this for initialization
    void Start () {
        dungeonUnits = new Dictionary<int, IDungeonUnit>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitDungeon(){
        for (int i = 0; i < initData.Length;++i){
            DungeonCreaterData data = initData[i];
            Vector2Int pos = data.pos;
            //DungeonData dunData = GameRoot.GetInstance().BattleField.assetManager.GetDungeonData(data.fieldType, data.dungeonLevel);
            //dungeonDatas.Add(pos.x*100+pos.y, dunData);
            switch(data.unitType){
                case DungeonUnitType.Fight:
                    DungeonData dunData = GameRoot.GetInstance().BattleField.assetManager.GetDungeonData(data.fieldType, data.dungeonLevel);
                    FightDungeon dungeon = new FightDungeon();
                    dungeon.Init(dunData);
                    dungeonUnits.Add(pos.x * 100 + pos.y, dungeon);
                    break;
            }
        }
    }

    public IDungeonUnit GetDungeonData(int key){
        return dungeonUnits.ContainsKey(key) ? dungeonUnits[key] : null;
    }



}
