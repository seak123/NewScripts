using UnityEngine;
using System.Collections;

public class BattleCasterState : BattleFsm
{
    private readonly float CasterTime = 20f;
    private float enterTime = 0f;
    public override void OnEnter()
    {
        Debug.Log("kaishi bu shu !!!");
        enterTime = 0;
    }

    public override void OnLeave()
    {
        enterTime = 0;
    }

    public override BattleState OnUpdate()
    {
        enterTime += Time.deltaTime;
        return enterTime>CasterTime?BattleState.Fight:BattleState.KeepRunning;
    }
}
