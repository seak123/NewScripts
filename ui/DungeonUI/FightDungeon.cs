using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightDungeon : IDungeonUnit {

    Sprite dungeonIcon;
   
    public void Init(DungeonData data){
        dungeonIcon = data.icon;
    }

    public void OpenDungeon()
    {
        GameRoot.GetInstance().mainUIMng.EnterBattle();
    }

    public Sprite GetDungeonSprite(){
        return dungeonIcon;
    }
}
