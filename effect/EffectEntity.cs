using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class EffectEntity : MonoBehaviour {

    public void SetPos(int x,int y,float z){
        MapField mapField = GameRoot.GetInstance().MapField;
        float v_x, v_y;
        mapField.GetViewPos(x, y, out v_x, out v_y);
        gameObject.transform.position = new Vector3(v_x,z, v_y);
    }

    public void GetPos(out int x,out int y,out float z){
        MapField mapField = GameRoot.GetInstance().MapField;
        int g_x, g_y;
        mapField.GetGridPos(gameObject.transform.position.x, gameObject.transform.position.z, out g_x, out g_y);
        x = g_x;
        y = g_y;
        z = gameObject.transform.position.y;
    }

    public void CleanUp(){
        Destroy(gameObject);
    }
}
