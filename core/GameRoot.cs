using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerInject;
using Map;
using System;
using Data;

[Power]
[Insert]
public class GameRoot : MonoBehaviour {

    public GameObject[] dontDestroy;

    public static Action moduleInit;

    public static Action BattleStartAction;

    public static Action BattleEndAction;

    public GameObject MainUI;

    public GameObject battleUI;

    public GameObject battleGroundUI;

    public GameObject battleTextUI;

    public GameObject Camara;

    public BattleData battleData;

    private static GameRoot instance;
    private GameObject fieldObj;


    //[Inject]
    //public GameStateManager StateManager { get; set; }

    [Inject]
    public Schedular Schedular { get; set; }

    [Inject]
    public LuaBridge Bridge { get; set; }

    [Inject]
    public BattleField BattleField { get; set; }

    [Inject]
    public MapField MapField { get; set; }

    [Inject]
    public EffectManager EffectMng { get; set; }

    [Inject]
    public PlayerManager PlayerMng { get; set; }

    [Inject]
    public MapManager mapManager { get; set; }


    public MainUIManager mainUIMng;

    public GameDataManager gameDataManager;


	// Use this for initialization
	void Start () {
        Debug.Log("GameRoot Start");
        Application.targetFrameRate = 30;
        DontDestroyOnLoad(gameObject);
        if(dontDestroy != null){
            foreach(var obj in dontDestroy){
                DontDestroyOnLoad(obj);
            }
        }
        instance = this;
        StrUtil.Init();
        gameDataManager = new GameDataManager();
        mainUIMng = MainUI.GetComponent<MainUIManager>();
        mainUIMng.OpenUI(0);
	}

    public void StartNewGame(){
        gameDataManager.InitData();
        moduleInit();
    }

    public void StartBattle(){
        BattleStartAction();
        mainUIMng.HideUI(true);
        battleGroundUI = mainUIMng.OpenUI(4);
        battleTextUI = mainUIMng.OpenUI(5);
        battleUI = mainUIMng.OpenUI(6);
        fieldObj = Instantiate(BattleField.assetManager.GetField(0));
        fieldObj.transform.position = Vector3.zero;

       
        battleData = new BattleData();

        List<UnitData> allStructures = gameDataManager.GetBattleConstructures();
        List<UnitData> allCreatures = gameDataManager.GetBattleCreatures();

        int num = allStructures.Count + allCreatures.Count;
        battleData.units = new UnitData[num];

        for (int i = 0; i < allStructures.Count;++i){
            battleData.units[i] = allStructures[i];
        }

        for (int i = 0; i < allCreatures.Count;++i){
            battleData.units[allStructures.Count + i] = allCreatures[i];
        }

        battleData.enemys = new UnitData[20];
       
        for (int i = 0; i < 20;++i){
            battleData.enemys[i] = AssetManager.PackCreatureData(BattleField.assetManager.GetCreatureData(1091), 2);
            battleData.enemys[i].init_x = 600;
            battleData.enemys[i].init_y = 310;
            battleData.enemys[i].init_room = -1;
        }

        //battleData = new BattleData();
        //battleData.unitNum = 2;
        //battleData.units = new UnitData[2];
        //battleData.units[0] = AssetManager.PackCreatureData(BattleField.assetManager.GetCreatureData(1091));
        //battleData.units[0].init_x = 100;
        //battleData.units[0].init_y = 50;
        //battleData.units[1] = AssetManager.PackCreatureData(BattleField.assetManager.GetCreatureData(1091),2);
        //battleData.units[1].init_x = 400;
        //battleData.units[1].init_y = 100;


        Bridge.StartBattle(battleData);
    }

    //public void BeginBattle(){

    //    constEnterDelay = battleEnterDelay;
    //    //Bridge.StartBattle(battleData);
    //    PlayerMng.InjectData(battleData);
    //    battleUI.GetComponentInChildren<CardManager>().InjectData();
    //    battleStart = true;
    //    if (BattleStartAction != null) BattleStartAction();
    //}

    public void InjectBattleData(){
        Bridge.StartBattle(battleData);
    }


    public void CompleteBattle(int res){
        if(res==0){

        }
        else if(res==1){
            //DungeonMng.DungeonCompleted();
        }
    }

    public void ClearBattle(){
        Destroy(fieldObj);
        BattleEndAction();
        mainUIMng.CloseUI();
        mainUIMng.CloseUI();
        mainUIMng.CloseUI();
        mainUIMng.HideUI(false);
    }

    public static GameRoot GetInstance(){
        return instance ?? null;
    }

    private void Update()
    {

    }
}
