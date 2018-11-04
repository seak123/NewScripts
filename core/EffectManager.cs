using System.Collections;
using System.Collections.Generic;
using PowerInject;
using UnityEngine;
using Map;

[Insert]
public class EffectManager : MonoBehaviour {

    private AssetManager assetMng;

    [OnInjected]
    public void AddRootAction()
    {
        GameRoot.init += Init;
    }

    public void Init()
    {
        Debug.Log("EffectManager Init");
       
        assetMng = GameRoot.GetInstance().BattleField.assetManager;
    }

    public EffectEntity CreateEffect(int id,bool attach,int unitUid,int posX,int posY){
        MapField mapField = GameRoot.GetInstance().MapField;
        EffectData data = assetMng.GetEffectData(id);
        Vector3 pos;
        Entity entity;
        if(unitUid != -1){
            entity = mapField.FindEntity(unitUid);
            pos = entity.GetSocketPos(data.effectSocket);
        }else{
            float v_x, v_y;
            mapField.GetViewPos(posX, posY, out v_x, out v_y);
            pos = new Vector3(v_x, v_y, 0.05f);
        }
        GameObject  obj = Instantiate(data.effectPrefab,pos, Quaternion.identity);
        if (attach == true ){
            entity = mapField.FindEntity(unitUid);
            obj.transform.SetParent(entity.gameObject.transform);
        }
        EffectEntity effectEntity = obj.AddComponent<EffectEntity>();
        return effectEntity;
    }
}
