using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerInject;

public struct PlayerBattleData{
    public int saving;
    public int income;
}

[Insert]
public class PlayerManager : MonoBehaviour {

    private PlayerData player;
    private PlayerData enemy;
    private PlayerBattleData playerData;
    private PlayerBattleData enemyData;

    private CardManager cardMng;
    private BattleUIManager uIManager;
    private EffectManager effectManager;

    private bool start = false;
    private float updateIncomeDelta = 0;
    private int baseIncome = 5;
    //private float incomeAdd = 0.5f;

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
        uIManager = GameRoot.GetInstance().battleUI.GetComponent<BattleUIManager>();
        effectManager = GameRoot.GetInstance().EffectMng;
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
            saving = 50,
            income = baseIncome,
        };
        enemyData = new PlayerBattleData
        {
            saving = 50,
            income = baseIncome,
        };
    }

    public void StartBattle(){
        start = true;
        uIManager.UpdatePlayerSaving(playerData.saving);
        uIManager.UpdatePlayerIncome(playerData.income);
    }

    public int GetPlayerSaving(){
        return playerData.saving;
    }

    private void Update(){
        if(start){
            if(updateIncomeDelta>=BattleDef.UpdateIncomeDelta){
                playerData.saving = playerData.saving + playerData.income;
                enemyData.saving = enemyData.saving + enemyData.income;
                updateIncomeDelta = -BattleDef.UpdateIncomeDelta;
                uIManager.UpdatePlayerSaving(playerData.saving);
            }
            //float playerDelta = Mathf.Max(0,(playerData.income - playerData.cost)*Time.deltaTime);
            //playerData.saving = Mathf.Clamp(playerData.saving+playerDelta,0,BattleDef.MaxSaving);

            //float enemyDelta = Mathf.Max(0,(enemyData.income - enemyData.cost)*Time.deltaTime);
            //enemyData.saving = Mathf.Clamp(enemyData.saving+enemyDelta,0,BattleDef.MaxSaving);

            updateIncomeDelta += Time.deltaTime;
        }
        //if(cardMng!=null)cardMng.PlayEnemyCard(playId, playGridX, playGridY);
    }

    public bool EnemyPlayCard(int id,int gridX,int gridY){
        //Debug.Log("play" + id+"X"+gridX+"Y"+gridY);
        return cardMng.PlayEnemyCard(id,gridX,gridY);
        //playId = id;
        //playGridX = gridX;
        //playGridY = gridY;
    }

    public bool RequestCost(int side,int cost){
        switch(side){
            case 1:
                int rest = playerData.saving - cost;
                if(rest>=0){
                    playerData.saving = rest;
                    uIManager.UpdatePlayerSaving(playerData.saving);
                    return true;
                }else{
                    return false;
                }
            case 2:
                int rest2 = enemyData.saving - cost;
                if(rest2>=0){
                    enemyData.saving = rest2;
                    return true;
                }else{
                    return false;
                }
        }
        return false;
    }

    public void ChangeIncome(int side,int value){
        switch (side)
        {
            case 1:
                playerData.income = playerData.income+value;
                uIManager.UpdatePlayerIncome(playerData.income);
                break;
            case 2:
                enemyData.income = enemyData.income+value;
                break;
        }
    }

    public void ChangeSaving(int side, int value)
    {
        switch (side)
        {
            case 1:
                playerData.saving = playerData.saving + value;
                uIManager.UpdatePlayerSaving(playerData.saving);
                break;
            case 2:
                enemyData.saving = enemyData.saving + value;
                break;
        }
    }

    public void AddSaving(Vector3 pos,int side,int value){
        switch(side)
        {
            case 1:
                ChangeSaving(side, value);
                effectManager.PrintGoldTips(pos, value);
                break;
            case 2:
                ChangeSaving(side, value);
                break;
        }
    }


}
