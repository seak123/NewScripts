using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightDungeon : IDungeonUnit
{

    Sprite dungeonIcon;
    DungeonState state;
    bool isVisiable = false;
    Vector2Int pos;
    DungeonUnit viewUnit;
    FightDungeonData data;

    public FightDungeon()
    {
    }

    public void Init(FightDungeonData _data)
    {
        dungeonIcon = _data.icon;
        data = _data;
    }

    public void OpenDungeon()
    {
        //GameRoot.GetInstance().mainUIMng.EnterBattle();
        switch (state)
        {
            case DungeonState.Sleeping:
                Debug.Log("sleeping now");
                GameRoot.GetInstance().mainUIMng.OpenUI(9);
                break;
            case DungeonState.Ready:
                GameRoot.GetInstance().DungeonMng.GetMaker().SetSelectPos(pos);
                GameRoot.GetInstance().mainUIMng.OpenUI(10);
                break;
            case DungeonState.Running:
                GameRoot.GetInstance().mainUIMng.OpenUI(11);
                break;
        }
    }

    public FightDungeonData GetFightData(){
        return data;
    }

    public Sprite GetDungeonSprite()
    {
        return dungeonIcon;
    }

    public void LeaveDungeon()
    {

    }

    public DungeonState GetState()
    {
        return state;
    }

    public void SetState(DungeonState _state)
    {
        state = _state;
    }

    public bool GetVisiable()
    {
        return isVisiable;
    }

    public void SetVisiable(bool flag)
    {
        isVisiable = flag;
        if (viewUnit != null)
        {
            viewUnit.SetVisiable(flag);
        }
    }

    public Vector2Int GetPos()
    {
        return pos;
    }

    public void SetPos(Vector2Int _pos)
    {
        pos = _pos;
    }

    public void SetViewUnit(DungeonUnit view)
    {
        viewUnit = view;
    }

}
