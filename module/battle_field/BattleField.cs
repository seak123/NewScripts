using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerInject;

[Insert]
public class BattleField : MonoBehaviour {

    public AssetManager assetManager;

    //public void AddCreature(int id,int side ,int x,int y){
    //    CreatureData data = assetManager.GetCreatureData(id);
    //    var unitdata = AssetManager.PackCreatureData(data);
    //    unitdata.side = side;
    //    unitdata.init_x = x;
    //    unitdata.init_y = y;
    //    GameRoot.GetInstance().Bridge.AddUnit(unitdata);
    //}

    [OnInjected]
    public void AddRootAction(){
        GameRoot.moduleInit += Init;
    }

    public void Init(){
        Debug.Log("BattleField Init");
    }
   
}
