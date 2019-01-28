using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class AvatarEvent : MonoBehaviour {

    public void CreateEffect(int key){
        int id = (int)key / 10;
        bool attach = (key - id * 10) == 1 ? true : false;
        Entity entity = GetComponentInParent<Entity>();
        int unitUid = entity.uid;
        int posX = entity.posX;
        int posY = entity.posY;
        GameRoot.GetInstance().EffectMng.CreateEffect(id, attach, unitUid, posX, posY);
    }

    public void HideUnit(){
        Material material = gameObject.GetComponent<SkinnedMeshRenderer>().material;
        {
            material.SetFloat("_Alpha", 0.6f);
        }
    }
}
