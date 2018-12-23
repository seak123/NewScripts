using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Data;

public class BattleUIManager : MonoBehaviour {

    //public Slider MagicSlider;
    //public GameObject MagicStone;
    //public GameObject MagicFill;
    public Text GoldValue;
    public Text IncomeValue;
    //public Material hightlightMaterial;

    public GameObject topPanel;
    public GameObject bottomPanel;
    public GameObject talkPanel;
    public GameObject goldSlider;

    private bool start = false;
    private int cacheSaving = 0;
    private int nowSaving = 0;
    private int cacheIncome = 0;
    private int nowIncome = 0;
    //private int oldMagicValue=0;
    //private float resetCache = 0;
   
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
            mainCastle = AssetManager.PackCreatureData(mng.GetCreatureData(101)),
            cardBoxNum = 4,
            cardSpeed = 10,
            cards = new List<int>
            {
                1081,
                1091,
                1081,
                1091,
                1091,
                1091,
                1081,
                1081,
                1081,
                1091,
                1081,
                1091,
                7,
                7,
                7,
            }
    };
        data.enemy = new PlayerData
        {
            attack = 1,
            hp = 3000,
            denfence = 100,
            magic_resist = 0.5f,
            mainCastle = AssetManager.PackCreatureData(mng.GetCreatureData(0)),
            cardBoxNum = 4,
            cardSpeed = 10,
            cards = new List<int>
            {
                1091,
                1091,
                1091,
                1091,
                1091,
                1081,
                1081
            }
        };
        data.beginDelay = 5f;
        InitPanelPosition();
        GameRoot.GetInstance().StartBattle(data);
        start = true;
    }

    private void Update()
    {
        //if (!start) return;
        //float saving = GameRoot.GetInstance().PlayerMng.GetPlayerSaving();
        //MagicValue.text = ((int)saving).ToString();
        //if((int)saving != oldMagicValue){
        //    resetCache = 0;
        //    ShakeMagicStone();
        //    MagicStone.GetComponent<Image>().material=hightlightMaterial;
        //    MagicFill.GetComponent<Image>().material = hightlightMaterial;
        //    oldMagicValue = (int)saving;
        //}
        //resetCache += Time.deltaTime;
        //if(resetCache>0.3f){
        //   ResetMagicStone();
        //    resetCache = 0;
        //}
        if(cacheSaving != nowSaving){
            cacheSaving = cacheSaving + (int)((nowSaving - cacheSaving) / 8f);
            if(Mathf.Abs(cacheSaving-nowSaving)<=8){
                cacheSaving = nowSaving;
            }
        }
        if (cacheIncome != nowIncome)
        {
            cacheIncome = cacheIncome + (int)((nowIncome - cacheIncome) / 8f);
            if (Mathf.Abs(cacheIncome - nowIncome) <= 8)
            {
                cacheIncome = nowIncome;
            }
        }
        GoldValue.text = cacheSaving.ToString();
        IncomeValue.text = cacheIncome.ToString();
    }

    ///private void ShakeMagicStone(){
    //    MagicStone.transform.DOScale(Vector3.one * 1.3f, 0.14f).SetLoops(2, LoopType.Yoyo).onComplete+=ResetMagicStone;
    //    MagicStone.transform.DOShakeScale(0.4f,new Vector3(0.4f,0.4f,0.4f),10,0);
    //    MagicStone.transform.DOShakeRotation(0.4f, new Vector3(0, 0, 10f));
    //}

    //private void ResetMagicStone(){
    //    MagicStone.transform.localScale = Vector3.one;
    //    MagicStone.transform.localRotation = Quaternion.Euler(0, 0, 0);
    //    MagicStone.GetComponent<Image>().material = null;
    //    MagicFill.GetComponent<Image>().material = null;
    //}

    private void InitPanelPosition(){
        topPanel.transform.DOLocalMove(new Vector3(0, 425, 0), 0.8f);
        //rightPanel.transform.DOLocalMove(new Vector3(860.4f, 108.77f, 0), 0.8f);
        bottomPanel.transform.DOLocalMove(new Vector3(0, -356, 0), 0.8f);
        //goldSlider.transform.DOLocalMove(new Vector3(27, -162, 0), 0.8f);
    }

    public void UpdatePlayerSaving(int saving){
        nowSaving = saving;
    }

    public void UpdatePlayerIncome(int income){
        nowIncome = income;
    }
    
}
