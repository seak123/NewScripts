using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightDungeon : IDungeonUnit {

    Sprite dungeonIcon;
    DungeonState state;

    public FightDungeon()
    {
    }

    public void Init(DungeonData data){
        dungeonIcon = data.icon;
        state = DungeonState.Hide;
    }

    public void OpenDungeon()
    {
        //GameRoot.GetInstance().mainUIMng.EnterBattle();
        switch(state){
            case DungeonState.Ready:
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
}
