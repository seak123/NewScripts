using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test:MonoBehaviour  {

    public void CreateTopCreature(){
        GameRoot.GetInstance().BattleField.AddCreature(1,1, 0, 0);
    }
    public void CreateMidCreature(){
        GameRoot.GetInstance().BattleField.AddCreature(2, 1, 0, 0);
    }
}
