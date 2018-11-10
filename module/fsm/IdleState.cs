using UnityEngine;
using System.Collections;
using System;

public class IdleState:FsmState{

    private readonly bool useMouse = BattleDef.useMouse;
    public Vector2 lastSingleTouchPosition;

    public IdleState(){
        stateType = GameState.IdleState;
    }

    override public void OnEnter(){
        Debug.Log("Enter IdleState");
    }

    public override void OnLeave()
    {

    }

    override public GameState OnUpdate(){
        if (Input.touchCount > 0){
            
        }
        if(useMouse){
            if (Input.GetMouseButtonDown(0))
            {
                lastSingleTouchPosition = Input.mousePosition;
                if((lastSingleTouchPosition.y/Screen.height)>BattleDef.cardPanalViewFactor){
                    return GameState.MoveCameraState;
                }
            }
            if (Input.GetMouseButton(0))
            {
                //MoveCamera(Input.mousePosition);
                if(GameRoot.GetInstance().StateManager.selectCard != null){
                    return GameState.SelectPosState;
                }
            }
        }
        return GameState.KeepRunning;
    }
};