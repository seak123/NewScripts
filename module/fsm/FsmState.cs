using UnityEngine;
using System.Collections;
using System;

public abstract class FsmState :  MonoBehaviour{
    public GameState stateType;
    abstract public void OnEnter();
    abstract public GameState OnUpdate();
    abstract public void OnLeave();
}