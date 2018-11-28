using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerInject;

public struct PlayerBattleData{
    public float saving;
    public float income;
    public float cost;
}

[Insert]
public class PlayerManager : MonoBehaviour {

    private PlayerData player;
    private PlayerData enemy;
    private PlayerBattleData playerData;
    private PlayerBattleData enemyData;

    private bool start = false;

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
        playerData = new PlayerBattleData();
        enemyData = new PlayerBattleData();
    }

    public void StartBattle(){
        start = true;
    }

    private void Update(){
        if(start){
            float playerDelta = Mathf.Max(0,(playerData.income - playerData.cost)*Time.deltaTime);
            playerData.saving = Mathf.Clamp(playerData.saving+playerDelta,0,BattleDef.MaxSaving);

            float enemyDelta = Mathf.Max(0,(enemyData.income - enemyData.cost)*Time.deltaTime);
            enemyData.saving = Mathf.Clamp(enemyData.saving+enemyDelta,0,BattleDef.MaxSaving);
        }
    }

    public bool RequestCost(int side,float cost){
        switch(side){
            case 1:
                float rest = playerData.saving - cost;
                if(rest>=0){
                    playerData.saving = rest;
                    return true;
                }else{
                    return false;
                }
            case 2:
                float rest2 = enemyData.saving - cost;
                if(rest2>=0){
                    enemyData.saving = rest2;
                    return true;
                }else{
                    return false;
                }
        }
        return false;
    }
}
