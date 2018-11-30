using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class BattleUIManager : MonoBehaviour {

    public Slider MagicSlider;

    private bool start = false;
   
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
            cardSpeed = 1
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
        start = true;
    }

    private void Update()
    {
        if (!start) return;
        MagicSlider.value = GameRoot.GetInstance().PlayerMng.GetPlayerSaving()/BattleDef.MaxSaving;
    }
}
