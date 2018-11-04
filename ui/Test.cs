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
}
