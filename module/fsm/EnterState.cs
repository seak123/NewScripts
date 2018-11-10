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
        GameRoot.BattleStartAction += BattleStart;
    }

    public override void OnLeave()
    {
        GameRoot.GetInstance().battleUI.transform.Find("StartPanal").gameObject.SetActive(false);
    }

    public override GameState OnUpdate()
    {
        return battleIsStart == true ? GameState.IdleState : GameState.KeepRunning;
    }

    private void BattleStart(){
        battleIsStart = true;
    }
}
