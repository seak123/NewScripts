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

    public GameObject topPanel;
    public GameObject rightPanel;
    public GameObject bottomPanel;
    public GameObject talkPanel;

    private bool start = false;
    private int oldMagicValue=0;
    private float resetCache = 0;
   
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
            cardSpeed = 1,
            cards = new List<int>
            {
                7,
                2,
                1,
                2,
                1,
                2,
                7,
                1
            }
    };
        data.enemy = new PlayerData
        {
            attack = 1,
            hp = 3000,
            denfence = 100,
            magic_resist = 0.5f,
            mainCastle = AssetManager.PackCreatureData(mng.GetCreatureData(0)),
            cardBoxNum = 3,
            cardSpeed = 1,
            cards = new List<int>
            {
                7,
                2,
                1,
                2,
                1,
                2,
                7,
                1
            }
        };
        data.beginDelay = 5f;
        InitPanelPosition();
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
            resetCache = 0;
            ShakeMagicStone();
            oldMagicValue = (int)saving;
        }
        resetCache += Time.deltaTime;
        if(resetCache>0.3f){
            ResetMagicStone();
            resetCache = 0;
        }
    }

    private void ShakeMagicStone(){
        MagicStone.transform.DOScale(Vector3.one * 1.2f, 0.1f).SetLoops(4, LoopType.Yoyo);
    //    MagicStone.transform.DOShakeScale(0.4f,new Vector3(0.4f,0.4f,0.4f),10,0);
    //    MagicStone.transform.DOShakeRotation(0.4f, new Vector3(0, 0, 10f));
    }

    private void ResetMagicStone(){
        MagicStone.transform.localScale = Vector3.one;
        MagicStone.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    private void InitPanelPosition(){
        topPanel.transform.DOLocalMove(new Vector3(0, 425, 0), 0.8f);
        rightPanel.transform.DOLocalMove(new Vector3(860.4f, 108.77f, 0), 0.8f);
        bottomPanel.transform.DOLocalMove(new Vector3(0, -356, 0), 0.8f);
    }
}
