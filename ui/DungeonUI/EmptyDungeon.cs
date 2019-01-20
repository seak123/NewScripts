using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyDungeon : IDungeonUnit
{
    DungeonState state;
    Vector2Int pos;
    bool isVisiable=false;
    DungeonUnit viewUnit;
    EmptyDungeonData data;

    public void CompleteDungeon()
    {
       
    }

    public void Init(EmptyDungeonData _data){
        data = _data;
    }

    public Sprite GetDungeonSprite()
    {
        return null;
    }

    public Vector2Int GetPos()
    {
        return pos;
    }

    public DungeonState GetState()
    {
        return state;
    }

    public bool GetVisiable()
    {
        return isVisiable;
    }

    public void LeaveDungeon()
    {
        throw new System.NotImplementedException();
    }

    public void OpenDungeon()
    {
        //GameRoot.GetInstance().mainUIMng.EnterBattle();
        switch (state)
        {
            case DungeonState.Sleeping:
                Debug.Log("sleeping now");
                //GameRoot.GetInstance().mainUIMng.OpenUI(9);
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

    public void SetPos(Vector2Int _pos)
    {
        pos = _pos;
    }

    public void SetState(DungeonState _state)
    {
        state = _state;
    }

    public void SetViewUnit(DungeonUnit view)
    {
        viewUnit = view;
    }

    public void SetVisiable(bool flag)
    {
        isVisiable = flag;
    }
}
