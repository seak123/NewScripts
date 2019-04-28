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

    public GameObject LoadingUI;

    public GameObject MainUI;

    public GameObject TipUI;

    public GameObject InfoUI;

    public GameObject MessageUI;

    public GameObject battleUI;

    public GameObject battleGroundUI;

    public GameObject battleTextUI;

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
    public CamaraManager CameraMng { get; set; }


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
        GameRoot.GetInstance().mainUIMng.OpenUI(9);
    }

    public void LoadGame(){
        if(gameDataManager.LoadData()== -1){
            Debug.Log("No Save File");
            return;
        }
        moduleInit();
        GameRoot.GetInstance().mainUIMng.OpenUI(9);
    }

    public void StartBattle(){
        //Time.timeScale = 3;

        BattleStartAction();
        mainUIMng.HideUI(true);
        battleGroundUI = mainUIMng.OpenUI(4);
        battleTextUI = mainUIMng.OpenUI(5);
        battleUI = mainUIMng.OpenUI(6);
        fieldObj = Instantiate(BattleField.assetManager.GetField(0));
        fieldObj.transform.position = Vector3.zero;


        battleData = gameDataManager.GetBattleData();

        Bridge.StartBattle(battleData);

    }

    public void StartStrategy(){

        List<int> uiList = new List<int>
        {
            14
        };
        Action action = new Action(()=>{
            BattleStartAction();
            fieldObj = Instantiate(BattleField.assetManager.GetField(0));
            fieldObj.transform.position = Vector3.zero;

            battleData = gameDataManager.GetBattleData();
            Bridge.StartStrategy(battleData);

        });

        mainUIMng.OpenScene(uiList, action,"地牢",14);
       
    }

    public void QuitStrategy(){
        mainUIMng.CloseScene();

        Destroy(fieldObj);
        BattleEndAction();
        mainUIMng.HideUI(false);
        gameDataManager.SaveData();
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
