using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Data;

public class BattleUIManager : MonoBehaviour {

    public Slider MagicSlider;
    public GameObject MagicStone;
    public Text MagicValue;

    private bool start = false;
    private int oldMagicValue=0;
   
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
        float saving = GameRoot.GetInstance().PlayerMng.GetPlayerSaving();
        MagicSlider.value = saving/BattleDef.MaxSaving;
        MagicValue.text = ((int)saving).ToString();
        if((int)saving != oldMagicValue){
            ShakeMagicStone();
            oldMagicValue = (int)saving;
        }
    }

    private void ShakeMagicStone(){
        MagicStone.transform.DOShakeScale(0.2f,new Vector3(0.4f,0.4f,0.4f),5,5);
        MagicStone.transform.DOShakeRotation(0.2f, new Vector3(0, 0, 10f));
    }
}
