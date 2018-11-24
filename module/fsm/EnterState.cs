using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterState : FsmState
{

    public EnterState(){
        stateType = GameState.EnterState;
    }

    private bool battleIsStart = false;

    public override void OnEnter()
    {
        GameRoot.BattleStartAction += BattleEnter;
        GameRoot.BattleStartDelayAction += BattleStart;
    }

    public override void OnLeave()
    {

    }

    public override GameState OnUpdate()
    {
        return battleIsStart == true ? GameState.IdleState : GameState.KeepRunning;
    }

    private void BattleEnter(){
        GameRoot.GetInstance().battleUI.transform.Find("StartPanal").gameObject.SetActive(false);
    }

    private void BattleStart(){
        battleIsStart = true;
    }
}
