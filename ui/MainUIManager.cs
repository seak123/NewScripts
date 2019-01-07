using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MainUIManager : MonoBehaviour {

    public GameObject[] UIPrefab;

    private int currSort;
    private List<GameObject> uiQueue;
	// Use this for initialization
	void Start () {
        currSort = 10;
        uiQueue = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenUI(int id){
        GameObject ui = Instantiate(UIPrefab[id]);
        ui.transform.SetParent(gameObject.transform);
        Canvas canvas = ui.GetComponent<Canvas>();
        canvas.sortingOrder = currSort;
        ++currSort;

        uiQueue.Add(ui);

    }

    public void CloseUI(){
        int index = uiQueue.Count - 1;
        Destroy(uiQueue[index]);
        uiQueue.RemoveAt(index);
        --currSort;
    }

    public void EnterBattle(){
        foreach(var obj in uiQueue){
            obj.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
