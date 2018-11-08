using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test:MonoBehaviour  {

    public void CreateTopCreature(){
        GameRoot.GetInstance().BattleField.AddCreature(4,1, 0, 150);
    }
    public void CreateMidCreature(){
        GameRoot.GetInstance().BattleField.AddCreature(1, 2, 600, 350);
    }
    public void StartBattle()
    {
        BattleData data = new BattleData();
        data.player = new PlayerData
        {
            attack = 1,
            hp = 3000,
            denfence = 100,
            magic_resist = 0.5f
        };
        data.enemy = new PlayerData
        {
            attack = 1,
            hp = 3000,
            denfence = 100,
            magic_resist = 0.5f
        };
        GameRoot.GetInstance().StartBattle(data);
    }
}
