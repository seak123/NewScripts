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
    }

    public void Init()
    {
        //Bridge.Init();
        Debug.Log("Schedular Init");
    }

}
