using System.Collections;
using System.Collections.Generic;
using PowerInject;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Map;

public enum BattleUICanvas{
    Message = 0,
    BattleText = 1,
}

public class MessageEffect{
    public GameObject effect;
    public int uid;
    public float duration;
    public Vector3 pos;
    public BattleUICanvas canvas;
}

[Insert]
public class EffectManager : MonoBehaviour {

    private AssetManager assetMng;
    private List<MessageEffect> messageCantainer;
    private List<GameObject> effects;
    private bool inBattle = false;


    [OnInjected]
    public void AddRootAction()
    {
        GameRoot.moduleInit += Init;
        GameRoot.BattleStartAction += BattleStart;
        GameRoot.BattleEndAction += CleanUp;
    }

    public void Init()
    {
        Debug.Log("EffectManager Init");
       

    }

    public void BattleStart(){
        inBattle = true;
        assetMng = GameRoot.GetInstance().BattleField.assetManager;
        messageCantainer = new List<MessageEffect>();
        effects = new List<GameObject>();
    }

    public void CleanUp(){
        Debug.Log("Clean EffectManager");
        foreach(var message in messageCantainer){
            Destroy(message.effect);
        }
        foreach(var effect in effects){
            if(effect.gameObject!=null){
                Destroy(effect.gameObject);
            }
        }
        messageCantainer.Clear();
        effects.Clear();
        inBattle = false;
    }

    public EffectEntity CreateEffect(int id,bool attach,int unitUid,int posX,int posY){
        MapField mapField = GameRoot.GetInstance().MapField;
        EffectData data = assetMng.GetEffectData(id);
        Vector3 pos;
        Entity entity;
        if(unitUid != -1){
            entity = mapField.FindEntity(unitUid);
            if (entity == null) Debug.Log("CreateEffect fail:entity is null");
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
        effects.Add(effectEntity.gameObject);
        return effectEntity;
    }

    //flag:1,crit,2,miss,3,heal,4,warning
    public void PrintMessage(int uid,string text,int flag){
        MapField mapField = GameRoot.GetInstance().MapField;
        Entity entity;
      
        entity = mapField.FindEntity(uid);
        if (entity == null) {
            Debug.Log("entity is null");
            return; }
        Canvas canvas = GameRoot.GetInstance().battleTextUI.GetComponent<Canvas>();
        CamaraManager camara = GameRoot.GetInstance().CameraMng;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(entity.GetSocketPos("S_Center"));
        //Vector2 uiPos = Vector2.zero;

        //uiPos.x = screenPos.x - (Screen.width / 2);
        //uiPos.y = screenPos.y - (Screen.height / 2);
        //hpBar.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
        GameObject message = Instantiate(GameRoot.GetInstance().BattleField.assetManager.Message);
        message.GetComponent<RectTransform>().parent = GameRoot.GetInstance().battleTextUI.GetComponent<RectTransform>();
        //hpBar.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Sqrt(radius) / 2 * 80, 22);
        message.SetActive(true);

        if(entity.side == 2){
            switch(flag){
                case 0:
                    message.GetComponentInChildren<Text>().text = text;
                    message.GetComponentInChildren<Text>().color = new Color(0.96f, 0.05f, 0f);
                    message.transform.localScale = Vector3.one*0.75f;
                    break;
                case 1:
                    message.GetComponentInChildren<Text>().text = text;
                    message.GetComponentInChildren<Text>().color = new Color(0.9f, 0.1f, 0.7f);
                    message.GetComponent<TipMessage>().SetLogo(int.Parse(text));
                    message.transform.DOScale(Vector3.one * 2.5f, 0.15f).onComplete+=()=>{
                        message.transform.DOScale(Vector3.one, 0.15f);
                    };
                    break;
                case 4:
                    message.GetComponentInChildren<Text>().text = StrUtil.GetText(text);
                    message.GetComponentInChildren<Text>().color = new Color(0.87f, 0.82f, 0.34f);
                    message.transform.localScale = new Vector3(1.4f,1,1);
                    break;
            }

        }else{
            switch(flag){
                case 0:
                case 1:
                    message.GetComponentInChildren<Text>().text = text;
                    message.GetComponentInChildren<Text>().color = new Color(0.7f, 0.6f, 1f);
                    message.transform.localScale = Vector3.one * 0.75f;
                    break;
                case 4:
                    message.GetComponentInChildren<Text>().text = StrUtil.GetText(text);
                    message.GetComponentInChildren<Text>().color = new Color(0.87f, 0.82f, 0.34f);
                    message.transform.localScale = new Vector3(1.4f, 1, 1);
                    break;
            }
        }
        message.transform.position = new Vector3(screenPos.x, screenPos.y, 0);
        Color t_color = message.GetComponentInChildren<Text>().color;
        message.GetComponentInChildren<Text>().DOColor(new Color(t_color.r, t_color.g, t_color.b, 1), 0.3f).onComplete += () =>
        {
            message.GetComponentInChildren<Text>().DOColor(new Color(t_color.r, t_color.g, t_color.b, 0), 0.4f);
        };
        Image logo = message.GetComponent<TipMessage>().CritLogo.GetComponentInChildren<Image>();
        Color l_color = logo.color;
        if(logo!=null && logo.gameObject.activeSelf == true)
        logo.DOColor(new Color(l_color.r, l_color.g, l_color.b, 1), 0.3f).onComplete += () =>
        {
            logo.DOColor(new Color(l_color.r, l_color.g, l_color.b, 0), 0.4f);
        };

        //init message struct
        MessageEffect effect = new MessageEffect
        {
            effect = message,
            uid = uid,
            duration = 0.8f,
            pos = entity.GetSocketPos("S_Center"),
            canvas = BattleUICanvas.BattleText,
        };
        messageCantainer.Add(effect);

        //float scale = camara.minSize / camara.size;
        //message.transform.localScale = Vector3.one;
        //message.transform.DOMoveY(message.transform.position.y + 20, 0.2f);
        //Destroy(message, 0.2f);

    }

    public void PrintGoldTips(Vector3 pos,int value){
        GameObject tips = Instantiate(GameRoot.GetInstance().BattleField.assetManager.GoldTips);
        tips.GetComponent<RectTransform>().parent = GameRoot.GetInstance().battleTextUI.GetComponent<RectTransform>();
        tips.SetActive(true);
        tips.GetComponent<Text>().text = "+"+value.ToString();
        Canvas canvas = GameRoot.GetInstance().battleTextUI.GetComponent<Canvas>();
        CamaraManager camara = GameRoot.GetInstance().CameraMng;
        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
        tips.transform.position = new Vector3(screenPos.x, screenPos.y, 0);

        MessageEffect effect = new MessageEffect
        {
            effect = tips,
            uid = -1,
            duration = 0.6f,
            pos = pos
        };
        messageCantainer.Add(effect);
        //float scale = camara.minSize / camara.size;
        tips.transform.localScale = Vector3.one;
    }

    private void Update()
    {
        if(inBattle){
            for (int index = messageCantainer.Count - 1; index >= 0;--index){
                MessageEffect effect = messageCantainer[index];
                MapField mapField = GameRoot.GetInstance().MapField;
                //Debug.Log("duration " + effect.duration);
                if(effect.duration<=0){
                    messageCantainer.RemoveAt(index);
                    Destroy(effect.effect);
                    continue;
                }


                Canvas canvas = GameRoot.GetInstance().battleTextUI.GetComponent<Canvas>();
                CamaraManager camara = GameRoot.GetInstance().CameraMng;
                Vector2 screenPos = Camera.main.WorldToScreenPoint(effect.pos);

                float scale = camara.GetViewSize();
                float hight = Screen.height / 10 * scale;

                effect.effect.transform.position = new Vector3(screenPos.x, screenPos.y + Mathf.Clamp((0.7f - effect.duration) * 0.3f, 0, 0.5f) * hight, 0);
                if(effect.canvas == BattleUICanvas.BattleText){
                    Text text = effect.effect.GetComponentInChildren<Text>();
                    if(text!=null){
                        text.gameObject.transform.localScale = new Vector3(0.7f,1f,1f)*scale*1.3f;
                    }
                }


                messageCantainer[index].duration = effect.duration - Time.deltaTime;
            }
        }
    }


}
