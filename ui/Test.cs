using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class Test:MonoBehaviour  {

    public void CreateTopCreature(){
        AssetManager mng = GameRoot.GetInstance().BattleField.assetManager;
        UnitData data = AssetManager.PackCreatureData( mng.GetCreatureData(1));
        GameRoot.GetInstance().Bridge.CasterSkill(1, 1, 0, 150,data,10);
        //GameRoot.GetInstance().BattleField.AddCreature(4,1, 0, 150);
    }
    public void CreateMidCreature(){
       //GameRoot.GetInstance().BattleField.AddCreature(1, 2, 600, 350);
    }
    public void StartBattle()
    {
        AssetManager mng = GameRoot.GetInstance().BattleField.assetManager;
        BattleData data = new BattleData();
        data.player = new PlayerData
        {
            attack = 1,
            hp = 3000,
            denfence = 100,
            magic_resist = 0.5f,
            mainCastle = AssetManager.PackCreatureData(mng.GetCreatureData(0)),
            cardBoxNum = 3,
            cardSpeed = 10
    };
        data.enemy = new PlayerData
        {
            attack = 1,
            hp = 3000,
            denfence = 100,
            magic_resist = 0.5f,
            mainCastle = AssetManager.PackCreatureData(mng.GetCreatureData(0)),
            cardBoxNum = 3,
            cardSpeed = 10
        };
        GameRoot.GetInstance().StartBattle(data);
    }
}
