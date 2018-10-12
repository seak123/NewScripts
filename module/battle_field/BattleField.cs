using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerInject;

[Insert]
public class BattleField : MonoBehaviour {

    public AssetManager assetManager;

    public void AddUnit(int id,float x,float y){
        CreatureData data = assetManager.GetCreatureData(id);
        Instantiate(data.prefab, new Vector3(x, y, 0), Quaternion.identity);
    }
   
}
