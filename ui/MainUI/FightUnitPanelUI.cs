using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightUnitPanelUI : MonoBehaviour {

    public void StartBattle()
    {
        GameRoot.GetInstance().mainUIMng.CloseUI();
        GameRoot.GetInstance().StartBattle();
    }
}
