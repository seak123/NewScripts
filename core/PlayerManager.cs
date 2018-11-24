using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerInject;

[Insert]
public class PlayerManager : MonoBehaviour {

    private PlayerData player;
    private PlayerData enemy;

    [OnInjected]
    public void AddRootAction()
    {
        GameRoot.init += Init;
    }

    public void Init()
    {
        Debug.Log("PlayerManager Init");
    }

    public void InjectData(BattleData data){
        player = data.player;
        enemy = data.enemy;
    }

    private void Start()
    {
        GameRoot.BattleStartDelayAction += StartBattle;
    }

    public void StartBattle(){

    }
}
