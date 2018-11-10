using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperState : FsmState {
    public HelperState(){
        stateType = GameState.HelperState;
    }
    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnLeave()
    {
        throw new System.NotImplementedException();
    }

    public override GameState OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
