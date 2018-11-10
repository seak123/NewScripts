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

    public GameObject battleUI;

    public GameObject Camara;

    private static GameRoot instance;

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

	}

    public void StartBattle(BattleData data){
        Bridge.StartBattle(data);
        BattleStartAction();
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
    }

}
