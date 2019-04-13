using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerInject;
using System;

[Insert]
public class Schedular : MonoBehaviour {

    public Action<float> onUpdate;

	// Use this for initialization
	void Start () {
        Debug.Log("start");
    }

	void Update () {
        float delta = Time.deltaTime;
        if(onUpdate != null){
            onUpdate(delta);
        }
	}
    [OnInjected]
    public void AddRootAction(){
        GameRoot.moduleInit += Init;
        GameRoot.BattleStartAction += ResetTimeScale;
        GameRoot.BattleEndAction += ResetTimeScale;
    }

    public void Init()
    {
        //Bridge.Init();
        Debug.Log("Schedular Init");
    }

    public void ResetTimeScale()
    {
        Time.timeScale = (float)(GameRoot.GetInstance().gameDataManager.timeScaleFlag % 4) * 0.5f + 1f;
    }


    public void ChangeTimeScale(){
        GameRoot.GetInstance().gameDataManager.timeScaleFlag = GameRoot.GetInstance().gameDataManager.timeScaleFlag + 1;
        Time.timeScale = (float)(GameRoot.GetInstance().gameDataManager.timeScaleFlag % 4) * 0.5f + 1f;
    }

}
