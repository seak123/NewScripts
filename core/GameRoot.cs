using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerInject;

[Power]
[Insert]
public class GameRoot : MonoBehaviour {

    public GameObject[] dontDestroy;

    private static GameRoot instance;

    [Inject]
    public GameStateManager StateManager { get; set; }

    [Inject]
    public Schedular Schedular { get; set; }

    [Inject]
    public LuaBridge Bridge { get; set; }

    [Inject]
    public BattleField BattleField { get; set; }

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

    public static GameRoot GetInstance(){
        return instance ?? null;
    }

}
