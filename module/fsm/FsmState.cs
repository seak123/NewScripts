using UnityEngine;
using System.Collections;
using System;

public class FsmState :  MonoBehaviour{
    virtual public void OnEnter();
    virtual public GameState OnUpdate();
    virtual public void OnLeave();
}