using UnityEngine;
using System.Collections;

public class BattleWaitingState : BattleFsm
{
    private bool isStart = false;

    public BattleWaitingState()
    {
    }

    public override void OnEnter()
    {
        isStart = false;
        GameRoot.BattleStartDelayAction += BattleStart;
    }

    public override void OnLeave()
    {
        isStart = false;
        GameRoot.BattleStartDelayAction -= BattleStart;
    }

    public override BattleState OnUpdate()
    {
        return isStart == true ? BattleState.Caster : BattleState.KeepRunning;
    }

    private void BattleStart(){
        isStart = true;
    }
}
