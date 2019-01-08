using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightDungeon : IDungeonUnit {

    Sprite dungeonIcon;
    DungeonState state;
    bool isVisiable=false;
    Vector2Int pos;

    public FightDungeon()
    {
    }

    public void Init(DungeonData data){
        dungeonIcon = data.icon;
    }

    public void OpenDungeon()
    {
        //GameRoot.GetInstance().mainUIMng.EnterBattle();
        switch(state){
            case DungeonState.Ready:
                GameRoot.GetInstance().DungeonMng.GetMaker().SetSelectPos(pos);
                GameRoot.GetInstance().mainUIMng.OpenUI(10);
                break;
        }
    }

    public Sprite GetDungeonSprite(){
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
    }

    public Vector2Int GetPos()
    {
        return pos;
    }

    public void SetPos(Vector2Int _pos)
    {
        pos = _pos;
    }
}
