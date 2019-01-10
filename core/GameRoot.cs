using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerInject;
using Map;
using System;

[Power]
[Insert]
public class GameRoot : MonoBehaviour {

    public GameObject[] dontDestroy;

    public static Action init;

    public static Action BattleStartAction;

    public static Action BattleStartDelayAction;

    public GameObject MainUI;

    public GameObject battleUI;

    public GameObject battleGroundUI;

    public GameObject battleTextUI;

    public GameObject Camara;

    public BattleData battleData;

    private static GameRoot instance;
    private GameDataManager dataManager;

    private bool battleStart = false;
    private float constEnterDelay = 0;
    private float battleEnterDelay = 0;

    [Inject]
    public GameStateManager StateManager { get; set; }

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
    public DungeonManager DungeonMng { get; set; }

    public MainUIManager mainUIMng;

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
        mainUIMng = MainUI.GetComponent<MainUIManager>();
        mainUIMng.OpenUI(0);
	}

    public float GetBattleEnterDelay(){
        return constEnterDelay;
    }

    public void StartBattle(){
        //battleData = data;
        //battleEnterDelay = 3 + data.beginDelay;
        //constEnterDelay = battleEnterDelay;
        //Bridge.StartBattle(data);
        //PlayerMng.InjectData(data);
        //battleStart = true;
        //if(BattleStartAction!=null) BattleStartAction();
        mainUIMng.HideUI(true);
        battleGroundUI = mainUIMng.OpenUI(4);
        battleTextUI = mainUIMng.OpenUI(5);
        battleUI = mainUIMng.OpenUI(6);
        PlayerData playerData = dataManager.GetPlayerData();
        PlayerData enemyData = dataManager.GetEnemyData();
    }

    public static GameRoot GetInstance(){
        return instance ?? null;
    }

    private void Update()
    {
        if(init != null){
            init();
            init = null;
        }
        if(battleStart){
            battleEnterDelay -= Time.deltaTime;
            if(battleEnterDelay<0){
                if(BattleStartDelayAction!=null) BattleStartDelayAction();
                battleStart = false;
            }
        }
    }
}
