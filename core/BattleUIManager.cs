using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Data;

public class BattleUIManager : MonoBehaviour,ISceneUI {

    //public Slider MagicSlider;
    //public GameObject MagicStone;
    //public GameObject MagicFill;
  
    //public Material hightlightMaterial;

    public GameObject topPanel;
    public GameObject bottomPanel;
    public GameObject talkPanel;
    public GameObject goldSlider;

   
    //private int oldMagicValue=0;
    //private float resetCache = 0;
    public void OnEnter(){

    }
   
    public void InitBattleUI(){
        InitPanelPosition();
    }

    private void Update()
    {

    }

   
    private void InitPanelPosition(){
        topPanel.transform.DOLocalMove(new Vector3(0, 425, 0), 0.8f);
        //rightPanel.transform.DOLocalMove(new Vector3(860.4f, 108.77f, 0), 0.8f);
        bottomPanel.transform.DOLocalMove(new Vector3(0, -356, 0), 0.8f);
        //goldSlider.transform.DOLocalMove(new Vector3(27, -162, 0), 0.8f);
    }

    public void ChangeTimeScale(){
        GameRoot.GetInstance().Schedular.ChangeTimeScale();
    }

  
    
}
