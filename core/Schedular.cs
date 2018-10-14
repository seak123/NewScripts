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
        if(onUpdate != null){
            onUpdate(Time.deltaTime);
        }
	}
    [OnInjected]
    public void AddRootAction(){
        GameRoot.init += Init;
    }

    public void Init()
    {
        //Bridge.Init();
        Debug.Log("Schedular Init");
    }

}
