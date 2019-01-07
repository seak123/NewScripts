using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonUnit : MonoBehaviour {

    public Button button;

    //private DungeonData data;
    private IDungeonUnit unit;


    private void Start()
    {

    }

    public void Init(IDungeonUnit _unit)
    {
        if (_unit == null) return;
        unit = _unit;
        button.image.sprite = _unit.GetDungeonSprite();
    }

    public void ClickUnit(){
        if (unit == null) return;
        unit.OpenDungeon();
    }
}
