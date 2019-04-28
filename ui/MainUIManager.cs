using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public enum UIType{
    MainUI = 1,
    SceneUI = 2,
    SubUI = 3,
}

public class MainUIManager : MonoBehaviour {

    public GameObject[] UIPrefab;
    public UIType[] UITypes;
    public Sprite[] loadingSprite;
    public GameObject creatureCardPrefab;
    public GameObject LoadingPrefab;
    public GameObject tipPrefab;
    // public GameObject loadingImage;
    // public GameObject siteBand;
    // public Text siteName;

    private bool useMouse = BattleDef.useMouse;

    private int currSort;
    private List<GameObject> loadingCache;
    private List<GameObject> tipCache;
    private GameObject cardPrefab;
    private List<GameObject> uiQueue;
    private int sceneCache = 0;
    private List<Action> sceneFuncList;
    private bool showTip = false;
    private Vector3 showTop;
    private Vector3 showBottom;
    private float sizeFactor;
	// Use this for initialization
	void Start () {
        currSort = 10;
        showTip = false;
        sizeFactor = (float)Screen.width / 750f;
        uiQueue = new List<GameObject>();
        sceneFuncList = new List<Action>();
        loadingCache = new List<GameObject>();
        tipCache = new List<GameObject>();
	}

    public void HideUI(bool flag){
        if(flag){
            foreach(var obj in uiQueue){
                obj.SetActive(false);
            }
        }else{
            foreach (var obj in uiQueue)
            {
                obj.SetActive(true);
            }
        }
    }
	
    public GameObject OpenUI(int id){
        GameObject ui = Instantiate(UIPrefab[id]);
        //ui.transform.SetParent(gameObject.transform);
        Canvas canvas = ui.GetComponent<Canvas>();
        canvas.sortingOrder = currSort;
        ISceneUI uiScript = ui.GetComponent<ISceneUI>();
        if (uiScript != null) uiScript.OnEnter();
        ++currSort;

        uiQueue.Add(ui);
        return ui;
    }

    public void CloseUI(){
        int index = uiQueue.Count - 1;
        Destroy(uiQueue[index]);
        uiQueue.RemoveAt(index);
        --currSort;
    }

    public void OpenCreatureCard(CreatureFightData data,Vector3 location){
        if(cardPrefab == null){
            cardPrefab = Instantiate(creatureCardPrefab);
            cardPrefab.transform.parent = GameRoot.GetInstance().InfoUI.transform;
            cardPrefab.transform.position = new Vector3(Screen.width/2,Screen.height/2);
        }
        cardPrefab.SetActive(true);
        GameRoot.GetInstance().InfoUI.SetActive(true);
        cardPrefab.transform.position = location;
        cardPrefab.transform.localScale = Vector3.one * 0.2f;
        cardPrefab.GetComponent<CreatureCardUI>().InjectData(data);
        cardPrefab.transform.DOMove(new Vector3(Screen.width / 2, Screen.height / 2, 0), 0.3f);
        cardPrefab.transform.DOScale(Vector3.one*1.05f, 0.3f).onComplete +=()=>{
            cardPrefab.transform.DOScale(Vector3.one, 0.05f);
        };
    }

    public void OpenTip(List<string> contents,Vector3 location,float topEdge,float bottomEdge){
        if (showTip == true) return;
        showTip = true;
        for (int i = 0; i < contents.Count; ++i)
        {
            GameObject obj = Instantiate(tipPrefab);
            obj.transform.parent = GameRoot.GetInstance().TipUI.transform;
            Tip script = obj.GetComponent<Tip>();
            script.InjectContent(contents[i]);
            obj.GetComponent<RectTransform>().position = location;
            tipCache.Add(obj);
        }
        showTop = location + new Vector3(0, topEdge*sizeFactor, 0);
        showBottom = location - new Vector3(0, bottomEdge*sizeFactor, 0);
    }

    public void CleanInfoUI(){
        if(cardPrefab!=null)
        cardPrefab.SetActive(false);
        GameRoot.GetInstance().InfoUI.SetActive(false);
    }

    public void OpenScene(List<int> uiList,Action completedFunc,string sceneName,int spriteId){
        GameObject loading = Instantiate(LoadingPrefab);
        LoadingUI uiScript = loading.GetComponent<LoadingUI>();
        loading.transform.parent = GameRoot.GetInstance().LoadingUI.transform;
        loading.transform.localPosition = Vector3.zero;
        //loading.GetComponent<RectTransform>().position = new Vector3(Screen.width/2, Screen.height, 0);
        loading.transform.localScale = Vector3.one;
        loadingCache.Insert(0,loading);
        GameObject loadingImage = uiScript.loadingImage;
        loadingImage.transform.localScale = Vector3.one * ((float)Screen.height/(float)Screen.width) / (1334f / 750f);
        GameObject siteBand = uiScript.siteBand;
        Text siteName = uiScript.siteName;

        loadingImage.SetActive(true);
        siteBand.SetActive(false);
        loadingImage.GetComponent<Image>().sprite = loadingSprite[spriteId];
        siteName.text = sceneName;
        loadingImage.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        loadingImage.GetComponent<Image>().DOColor(new Color(1, 1, 1, 1), 0.8f).onComplete += ()=>{
            //loading at here
            HideUI(true);
            sceneFuncList.Add(completedFunc);
           
            loadingImage.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0), 0.8f).onComplete += () =>
            {
                siteBand.SetActive(true);
                siteBand.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                siteBand.GetComponent<Image>().DOColor(new Color(1,1,1,1),0.5f).onComplete +=()=>{

                        siteBand.GetComponent<Image>().DOColor(new Color(1, 1, 1, 1), 2f).onComplete += () =>
                        {
                            siteBand.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 0.5f).onComplete += () =>
                            {
                                siteBand.SetActive(false);
                            };
                        };
                };
                siteName.GetComponent<Text>().color = new Color(1, 0.9f, 0.65f, 0);
                siteName.GetComponent<Text>().DOColor(new Color(1,0.9f, 0.65f, 1), 0.5f).onComplete += () => {
                        siteName.GetComponent<Text>().DOColor(new Color(1, 0.9f, 0.65f, 1), 2f).onComplete += () =>
                        {
                            siteName.GetComponent<Text>().DOColor(new Color(1, 0.9f, 0.65f, 0), 0.5f).onComplete += ()=>{
                            loadingCache.Remove(loading);
                            Destroy(loading);
                            };
                        };
                };
                foreach (var ui in uiList)
                {
                    OpenUI(ui);
                    ++sceneCache;
                }
                loadingImage.SetActive(false);
            };
            };
    }

    public void CloseScene(){
        while(sceneCache>0){
            --sceneCache;
            CloseUI();
            CleanInfoUI();
        }

        foreach (var obj in loadingCache) obj.SetActive(false);
        HideUI(false);
    }

    public void CleanTip(){
        for (int i = 0; i < tipCache.Count;++i){
            Destroy(tipCache[i]);
        }
        tipCache.Clear();
    }

    private void Update()
    {
        if(sceneFuncList.Count>0){
            foreach(var func in sceneFuncList){
                func();
            }
            sceneFuncList.Clear();
        }
        if(useMouse){
            if(!Input.GetMouseButton(0)&&showTip==true){
                showTip = false;
                CleanTip();
            }
        }else{
            if(Input.touchCount == 0 && showTip == true){
                showTip = false;
                CleanTip();
            }
        }
        //auto size
        if(tipCache.Count!=0){
            float wholeSize = 0;
            foreach(var obj in tipCache){
                wholeSize += obj.GetComponent<RectTransform>().sizeDelta.y+30*sizeFactor+10*sizeFactor;
            }
            if(showTop.y+wholeSize<Screen.height){
                float start = showTop.y+wholeSize;
                for (int i = 0; i < tipCache.Count;++i){
                    tipCache[i].GetComponent<RectTransform>().position = new Vector3(tipCache[i].GetComponent<RectTransform>().position.x, start - tipCache[i].GetComponent<RectTransform>().sizeDelta.y / 2-15*sizeFactor, 0);
                    start -= tipCache[i].GetComponent<RectTransform>().sizeDelta.y+30*sizeFactor+10*sizeFactor;
                }
            }else{
                float start = showBottom.y -10*sizeFactor;
                for (int i = 0; i < tipCache.Count; ++i)
                {
                    tipCache[i].GetComponent<RectTransform>().position = new Vector3(tipCache[i].GetComponent<RectTransform>().position.x, start - tipCache[i].GetComponent<RectTransform>().sizeDelta.y / 2-15*sizeFactor, 0);
                    start -= tipCache[i].GetComponent<RectTransform>().sizeDelta.y+30*sizeFactor+10*sizeFactor;
                }
            }

        }
    }
}
