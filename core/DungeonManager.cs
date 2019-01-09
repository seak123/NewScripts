using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PowerInject;


[Insert]
public class DungeonManager : MonoBehaviour {

    private Dictionary<int, IDungeonUnit> dungeonUnits;
    private const int dungeonSize=8;

    private DungeonUIMaker uiMaker;
    private Vector2Int currPos;
    private List<Vector2Int> nextPos;

    public DungeonCreaterData[] initData;


    public int GetSize(){
        return dungeonSize;
    }

    public void SetMaker(DungeonUIMaker maker){
        uiMaker = maker;
    }

    public DungeonUIMaker GetMaker(){
        return uiMaker;
    }

    public Vector2Int GetCurrPos(){
        return currPos;
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
        nextPos = new List<Vector2Int>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitDungeon(){
        currPos = new Vector2Int(0, 0);
        for (int i = 0; i < initData.Length;++i){
            DungeonCreaterData data = initData[i];
            Vector2Int pos = data.pos;
            //DungeonData dunData = GameRoot.GetInstance().BattleField.assetManager.GetDungeonData(data.fieldType, data.dungeonLevel);
            //dungeonDatas.Add(pos.x*100+pos.y, dunData);
            switch(data.unitType){
                case DungeonUnitType.Fight:
                    DungeonData dunData = GameRoot.GetInstance().BattleField.assetManager.GetDungeonData(data.fieldType, data.dungeonLevel);
                    FightDungeon dungeon = new FightDungeon();
                    dungeon.SetState(DungeonState.Sleeping);
                    dungeon.SetVisiable(false);
                    dungeon.SetPos(pos);
                    dungeon.Init(dunData);
                    dungeonUnits.Add(pos.x * 100 + pos.y, dungeon);
                    break;
            }
        }

        dungeonUnits[101].SetState(DungeonState.Ready);
        dungeonUnits[101].SetVisiable(true);

    }

    public IDungeonUnit GetDungeonData(int key){
        return dungeonUnits.ContainsKey(key) ? dungeonUnits[key] : null;
    }

    public void SetCurrPos(Vector2Int _pos){
        currPos = _pos;
        dungeonUnits[currPos.x * 100 + currPos.y].SetState(DungeonState.Running);
        if(currPos.x<=dungeonSize){
            int key = (currPos.x+1) * 100 + currPos.y;
            if(dungeonUnits.ContainsKey(key)){
                dungeonUnits[key].SetVisiable(true);
            }
            key = (currPos.x + 1) * 100 + currPos.y + 1;
            if (dungeonUnits.ContainsKey(key))
            {
                dungeonUnits[key].SetVisiable(true);
            }
            key = (currPos.x + 2) * 100 + currPos.y+1;
            if (dungeonUnits.ContainsKey(key))
            {
                dungeonUnits[key].SetVisiable(true);
            }
        }
        else if(currPos.x> 3 * dungeonSize - 3){

        }else{

        }
    }



}
