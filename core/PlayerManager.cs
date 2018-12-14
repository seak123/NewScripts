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

    private CardManager cardMng;

    private bool start = false;
    private float updateIncomeDelta = 0;
    private float baseIncome = 0;
    private float incomeAdd = 1;

    public int[] enemyCards;

    private int playId;
    private int playGridX;
    private int playGridY;

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

    public PlayerData GetPlayerData(){
        return player;
    }
    public PlayerData GetEnemyData(){
        return enemy;
    }
    public void SetCardManager(CardManager mng){
        cardMng = mng;
    }
    public CardManager GetCardManager(){
        return cardMng;
    }

    private void Start()
    {
        GameRoot.BattleStartDelayAction += StartBattle;
        playerData = new PlayerBattleData
        {
            saving = 0,
            income = 0,
            cost = 0
        };
        enemyData = new PlayerBattleData
        {
            saving = 0,
            income = 0,
            cost = 0
        };
    }

    public void StartBattle(){
        start = true;
    }

    public float GetPlayerSaving(){
        return playerData.saving;
    }

    private void Update(){
        if(start){
            if(updateIncomeDelta>=0 && baseIncome < BattleDef.MaxBaseIncome){
                baseIncome += incomeAdd;
                playerData.income += incomeAdd;
                enemyData.income += incomeAdd;
                updateIncomeDelta = -BattleDef.UpdateIncomeDelta;
            }
            float playerDelta = Mathf.Max(0,(playerData.income - playerData.cost)*Time.deltaTime);
            playerData.saving = Mathf.Clamp(playerData.saving+playerDelta,0,BattleDef.MaxSaving);

            float enemyDelta = Mathf.Max(0,(enemyData.income - enemyData.cost)*Time.deltaTime);
            enemyData.saving = Mathf.Clamp(enemyData.saving+enemyDelta,0,BattleDef.MaxSaving);

            updateIncomeDelta += Time.deltaTime;
        }
        if(cardMng!=null)cardMng.PlayEnemyCard(playId, playGridX, playGridY);
    }

    public void EnemyPlayCard(int id,int gridX,int gridY){
        //Debug.Log("play" + id+"X"+gridX+"Y"+gridY);
        //cardMng.PlayEnemyCard(id,gridX,gridY);
        playId = id;
        playGridX = gridX;
        playGridY = gridY;
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
