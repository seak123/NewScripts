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
    public GameObject loadingImage;
    public GameObject siteBand;
    public Text siteName;

    private int currSort;
    private GameObject cardPrefab;
    private List<GameObject> uiQueue;
    private List<Action> sceneFuncList;
	// Use this for initialization
	void Start () {
        currSort = 10;
        uiQueue = new List<GameObject>();
        sceneFuncList = new List<Action>();
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
        CleanInfoUI();
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
        cardPrefab.transform.DOScale(Vector3.one, 0.3f);
    }

    public void CleanInfoUI(){
        if(cardPrefab!=null)
        cardPrefab.SetActive(false);
        GameRoot.GetInstance().InfoUI.SetActive(false);
    }

    public void ChangeScene(List<int> uiList,Action completedFunc,string sceneName,int spriteId){
 
        loadingImage.SetActive(true);
        loadingImage.GetComponent<Image>().sprite = loadingSprite[spriteId];
        siteName.text = sceneName;
        loadingImage.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        loadingImage.GetComponent<Image>().DOColor(new Color(1, 1, 1, 1), 0.8f).onComplete += ()=>{

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
                            siteName.GetComponent<Text>().DOColor(new Color(1, 0.9f, 0.65f, 0), 0.5f);
                        };
                };
                foreach (var ui in uiList)
                {
                    OpenUI(ui);
                }
                loadingImage.SetActive(false);
            };
            };
    }

    private void Update()
    {
        if(sceneFuncList.Count>0){
            foreach(var func in sceneFuncList){
                func();
            }
            sceneFuncList.Clear();
        }
    }
}
