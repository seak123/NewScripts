using UnityEngine;
using System.Collections;

public abstract class BattleFsm : MonoBehaviour
{
    public BattleState stateType;
    abstract public void OnEnter();
    abstract public BattleState OnUpdate();
    abstract public void OnLeave();
}
