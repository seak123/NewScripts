using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MainUIManager : MonoBehaviour {

    public GameObject[] UIPrefab;
    public GameObject creatureCardPrefab;

    private int currSort;
    private GameObject cardPrefab;
    private List<GameObject> uiQueue;
	// Use this for initialization
	void Start () {
        currSort = 10;
        uiQueue = new List<GameObject>();
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

    public void EnterBattle(){
        foreach(var obj in uiQueue){
            obj.SetActive(false);
        }
        gameObject.SetActive(false);
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
}
