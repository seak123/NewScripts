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

    public static Action moduleInit;

    public static Action init;

    public static Action clean;

    public static Action BattleStartAction;

    public static Action BattleStartDelayAction;

    public GameObject MainUI;

    public GameObject battleUI;

    public GameObject battleGroundUI;

    public GameObject battleTextUI;

    public GameObject Camara;

    public BattleData battleData;

    private static GameRoot instance;
    private GameObject fieldObj;

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

    public float GetBattleEnterDelay(){
        return constEnterDelay;
    }

    public void StartNewGame(){
        gameDataManager.InitData();
        moduleInit();

    }

    public void StartBattle(){
        mainUIMng.HideUI(true);
        battleGroundUI = mainUIMng.OpenUI(4);
        battleTextUI = mainUIMng.OpenUI(5);
        battleUI = mainUIMng.OpenUI(6);
        fieldObj = Instantiate(BattleField.assetManager.GetField(gameDataManager.GetFieldId()));
        fieldObj.transform.position = Vector3.zero;
        
        PlayerData playerData = gameDataManager.GetPlayerData();
        PlayerData enemyData = gameDataManager.GetEnemyData();
        BattleData data = new BattleData
        {
            player = playerData,
            enemy = enemyData,
            beginDelay = 5f,
            fieldId = gameDataManager.GetFieldId(),
        };
        battleData = data;
        Camara.GetComponent<CamaraManager>().Init();
        init();
    }

    public void BeginBattle(){
        battleEnterDelay = 3 + battleData.beginDelay;
        constEnterDelay = battleEnterDelay;
        battleUI.GetComponent<BattleUIManager>().InitBattleUI();
        Bridge.StartBattle(battleData);
        PlayerMng.InjectData(battleData);
        battleUI.GetComponentInChildren<CardManager>().InjectData();
        battleStart = true;
        if (BattleStartAction != null) BattleStartAction();
    }

    public void CompleteBattle(int res){
        if(res==0){

        }
        else if(res==1){
            DungeonMng.DungeonCompleted();
        }
    }

    public void ClearBattle(){
        Destroy(fieldObj);
        clean();
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
        if(battleStart){
            battleEnterDelay -= Time.deltaTime;
            if(battleEnterDelay<0){
                if(BattleStartDelayAction!=null) BattleStartDelayAction();
                battleStart = false;
            }
        }
    }
}
