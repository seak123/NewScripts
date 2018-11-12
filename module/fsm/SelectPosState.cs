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
        Debug.Log("Enter Select State");
        cardEntity = GameRoot.GetInstance().StateManager.selectCard;
    }

    public override void OnLeave()
    {
       
    }

    public override GameState OnUpdate()
    {
        if(useMouse){
            if(Input.GetMouseButton(0)){
                cardEntity.OnMove(Input.mousePosition);
            }
            if(Input.GetMouseButtonUp(0)){
                cardEntity.OnRelease(Input.mousePosition);
                GameRoot.GetInstance().StateManager.selectCard = null;
                return GameState.IdleState;
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
