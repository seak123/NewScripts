using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DungeonState{
    Sleeping = 1,
    Ready = 2,
    Running = 3,
    Completed = 4,
    Disabled = 5
}

public class DungeonUnit : MonoBehaviour {

    public Button button;

    //private DungeonData data;
    private IDungeonUnit unitData;
    private DungeonState state;

    private bool isVisiable;


    private void Start()
    {
        isVisiable = false;
    }

    public void Init(IDungeonUnit _unit)
    {
        if (_unit == null) return;
        unitData = _unit;
        button.image.sprite = _unit.GetDungeonSprite();
        ChangeState(unitData.GetState());
        SetVisiable(unitData.GetVisiable());
        _unit.SetViewUnit(this);
    }

    public void ChangeState(DungeonState newState){
        state = newState;
        switch(state){
            case DungeonState.Sleeping:
                break;
            case DungeonState.Ready:
                break;
            case DungeonState.Running:
                break;
            case DungeonState.Completed:
                break;
            case DungeonState.Disabled:
                break;
        }
    }

    public void ClickUnit(){
        if (unitData == null) return;
        unitData.OpenDungeon();
    }

    public void SetVisiable(bool _isVisiable){
        if(_isVisiable==true){
            isVisiable = true;
            gameObject.SetActive(true);
        }else if(_isVisiable==false){
            isVisiable = false;
            gameObject.SetActive(false);
        }
    }
}
