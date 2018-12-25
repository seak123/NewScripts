using System.Collections;
using System.Collections.Generic;
using PowerInject;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Map;

public class MessageEffect{
    public GameObject effect;
    public int uid;
    public float duration;
    public Vector3 pos;
}

[Insert]
public class EffectManager : MonoBehaviour {

    private AssetManager assetMng;
    private List<MessageEffect> messageCantainer;
    private bool hasInited = false;

    [OnInjected]
    public void AddRootAction()
    {
        GameRoot.init += Init;
    }

    public void Init()
    {
        Debug.Log("EffectManager Init");
        hasInited = true;
       
        assetMng = GameRoot.GetInstance().BattleField.assetManager;
        messageCantainer = new List<MessageEffect>();
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

    public void PrintMessage(int uid,string text){
        MapField mapField = GameRoot.GetInstance().MapField;
        Entity entity;
      
        entity = mapField.FindEntity(uid);
        if (entity == null) {
            Debug.Log("entity is null");
            return; }
        Canvas canvas = GameRoot.GetInstance().battleTextUI.GetComponent<Canvas>();
        CamaraManager camara = GameRoot.GetInstance().Camara.GetComponent<CamaraManager>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(entity.GetSocketPos("S_Center"));
        //Vector2 uiPos = Vector2.zero;

        //uiPos.x = screenPos.x - (Screen.width / 2);
        //uiPos.y = screenPos.y - (Screen.height / 2);
        //hpBar.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
        GameObject message = Instantiate(GameRoot.GetInstance().BattleField.assetManager.Message);
        message.GetComponent<RectTransform>().parent = GameRoot.GetInstance().battleTextUI.GetComponent<RectTransform>();
        //hpBar.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Sqrt(radius) / 2 * 80, 22);
        message.SetActive(true);
        message.GetComponent<Text>().text = StrUtil.GetText(text);
        if(entity.side == 1){
            message.GetComponent<Text>().color = new Color(0.34f, 0.87f, 0.34f);
        }else{
            message.GetComponent<Text>().color = new Color(0.88f, 0.49f, 0.83f);
        }
        message.transform.position = new Vector3(screenPos.x, screenPos.y, 0);

        //init message struct
        MessageEffect effect = new MessageEffect
        {
            effect = message,
            uid = uid,
            duration = 2,
            pos = entity.GetSocketPos("S_Center")
        };
        messageCantainer.Add(effect);

        float scale = camara.minSize / camara.size;
        message.transform.localScale = Vector3.one * scale;
        //message.transform.DOMoveY(message.transform.position.y + 20, 0.2f);
        //Destroy(message, 0.2f);

    }

    public void PrintGoldTips(Vector3 pos,int value){
        GameObject tips = Instantiate(GameRoot.GetInstance().BattleField.assetManager.GoldTips);
        tips.GetComponent<RectTransform>().parent = GameRoot.GetInstance().battleTextUI.GetComponent<RectTransform>();
        tips.SetActive(true);
        tips.GetComponent<Text>().text = "+"+value.ToString();
        Canvas canvas = GameRoot.GetInstance().battleTextUI.GetComponent<Canvas>();
        CamaraManager camara = GameRoot.GetInstance().Camara.GetComponent<CamaraManager>();
        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
        tips.transform.position = new Vector3(screenPos.x, screenPos.y, 0);

        MessageEffect effect = new MessageEffect
        {
            effect = tips,
            uid = -1,
            duration = 2,
            pos = pos
        };
        messageCantainer.Add(effect);
        float scale = camara.minSize / camara.size;
        tips.transform.localScale = Vector3.one * scale;
    }

    private void Update()
    {
        if(hasInited){
            for (int index = messageCantainer.Count - 1; index >= 0;--index){
                MessageEffect effect = messageCantainer[index];
                MapField mapField = GameRoot.GetInstance().MapField;
                //Debug.Log("duration " + effect.duration);
                if(effect.duration<=0){
                    Debug.Log("destroy");
                    messageCantainer.RemoveAt(index);
                    Destroy(effect.effect);
                    continue;
                }


                Canvas canvas = GameRoot.GetInstance().battleTextUI.GetComponent<Canvas>();
                CamaraManager camara = GameRoot.GetInstance().Camara.GetComponent<CamaraManager>();
                Vector2 screenPos = Camera.main.WorldToScreenPoint(effect.pos);

                float scale = camara.minSize / camara.size;
                float hight = Screen.height / 10 * scale;

                effect.effect.transform.position = new Vector3(screenPos.x, screenPos.y+Mathf.Clamp(2-effect.duration,0,0.8f)*hight, 0);
                effect.effect.transform.localScale = Vector3.one * scale;

                messageCantainer[index].duration = effect.duration - Time.deltaTime;
            }
        }
    }


}
