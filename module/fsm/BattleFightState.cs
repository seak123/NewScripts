using UnityEngine;
using System.Collections;

public class BattleFightState : BattleFsm
{
    public override void OnEnter()
    {
        Debug.Log("kai shi zhan dou !");
        GameRoot.GetInstance().BattleField.CasterEnemy();
    }

    public override void OnLeave()
    {
        Debug.Log("clean battle");
    }

    public override BattleState OnUpdate()
    {
        int num = GameRoot.GetInstance().Bridge.GetEnemyNum();
        Debug.Log("enemyNum:" + num);
        return num > 0 ? BattleState.KeepRunning : BattleState.Caster;
    }
}
