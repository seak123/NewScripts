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
            obj.transform.SetParent(entity.gameObject.transform.Find(data.effectSocket).gameObject.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
            obj.transform.localScale = Vector3.one * entity.radius / 4;
        }
        EffectEntity effectEntity = obj.AddComponent<EffectEntity>();

        //clean effect
        if(data.isAutoClean){
            ParticleSystem[] array = obj.GetComponentsInChildren<ParticleSystem>();
            float time = 0;
            foreach(var par in array){
                float temp = par.main.duration;
                if(temp>time){
                    time = temp;
                }
            }
            Destroy(obj, time);
        }
        return effectEntity;
    }
}
