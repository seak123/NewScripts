using UnityEngine;
using System.Collections;
using System;

public class IdleState:FsmState{
    virtual public void OnEnter(){

    }

    virtual public GameState OnUpdate(){
        if (Input.touchCount > 0){
            
        }
    }
};