using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPosState : FsmState {

    private CardEntity cardEntity;
    private readonly bool useMouse = BattleDef.useMouse;

    public SelectPosState(){
        stateType = GameState.SelectPosState;
    }

    public override void OnEnter()
    {
        cardEntity = GameRoot.GetInstance().StateManager.selectCard;
    }

    public override void OnLeave()
    {
       
    }

    public override GameState OnUpdate()
    {
        if(useMouse){
            if(Input.GetMouseButton(0)){
                cardEntity.gameObject.transform.position = Input.mousePosition;
            }
        }
        return GameState.KeepRunning;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
