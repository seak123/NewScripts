using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPosState : FsmState {

    private CardEntity cardEntity;
    private readonly bool useMouse = BattleDef.useMouse;
    private Vector2 offset;

    public SelectPosState(){
        stateType = GameState.SelectPosState;
    }

    public override void OnEnter()
    {
        Debug.Log("Enter Select State");
        //cardEntity = GameRoot.GetInstance().StateManager.selectCard;
    }

    public override void OnLeave()
    {
       
    }

    public override GameState OnUpdate()
    {
        //if (Input.touchCount > 0)
        //{
        //    if (Input.GetTouch(0).phase == TouchPhase.Moved)
        //    {
        //        cardEntity.OnMove(Input.GetTouch(0).position+offset);
        //    }
        //    if (Input.GetTouch(0).phase == TouchPhase.Ended)
        //    {
        //        cardEntity.OnRelease(Input.GetTouch(0).position+offset);
        //        GameRoot.GetInstance().StateManager.selectCard = null;
        //        return GameState.IdleState;
        //    }
        //}
        //if(useMouse){
        //    if(Input.GetMouseButton(0)){
        //        cardEntity.OnMove(Input.mousePosition+new Vector3(offset.x,offset.y,0));
        //    }
        //    if(Input.GetMouseButtonUp(0)){
        //        cardEntity.OnRelease(Input.mousePosition);
        //        GameRoot.GetInstance().StateManager.selectCard = null;
        //        return GameState.IdleState;
        //    }
        //}
        return GameState.KeepRunning;
    }

    // Use this for initialization
    void Start () {
        offset = new Vector2(0, Screen.height / 4);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
