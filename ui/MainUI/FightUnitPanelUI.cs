using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightUnitPanelUI : MonoBehaviour,ISceneUI {

    public void StartBattle()
    {
        GameRoot.GetInstance().mainUIMng.CloseUI();
        GameRoot.GetInstance().StartBattle();
    }

    public void OnEnter(){

    }
}
