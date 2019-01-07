using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DungeonState{
    Hide = 1,
    Active = 2,
    Ready = 3,
    Disable = 4
}

public class DungeonUnit : MonoBehaviour {

    public Button button;

    //private DungeonData data;
    private IDungeonUnit unit;
    private DungeonState state;


    private void Start()
    {

    }

    public void Init(IDungeonUnit _unit)
    {
        if (_unit == null) return;
        unit = _unit;
        button.image.sprite = _unit.GetDungeonSprite();
        ChangeState(DungeonState.Hide);
    }

    public void ChangeState(DungeonState newState){
        state = newState;
        switch(state){
            case DungeonState.Active:
                break;
            case DungeonState.Hide:
                gameobject.SetActive(false);
                break;
            case DungeonState.Ready:
                break;
            case DungeonState.Disable:
                break;
        }
    }

    public void ClickUnit(){
        if (unit == null) return;
        unit.OpenDungeon(state);
    }
}
