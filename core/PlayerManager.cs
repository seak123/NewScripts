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

    [OnInjected]
    public void AddRootAction()
    {
        GameRoot.moduleInit += Init;
        GameRoot.BattleStartAction += StartBattle;
        GameRoot.BattleEndAction += CleanUp;
    }

    public void Init()
    {
        Debug.Log("PlayerManager Init");

    }

    public void CleanUp(){
       
    }

    public void InjectData(BattleData data){

    }


    private void Start()
    {


    }

    public void StartBattle(){

    }

  
    private void Update(){

        //if(cardMng!=null)cardMng.PlayEnemyCard(playId, playGridX, playGridY);
    }


   


}
