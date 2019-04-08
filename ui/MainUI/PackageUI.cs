using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum PackageType{
    IdleCreature = 1,
    AllCreature = 2,
    IdleConstructure = 3,
}

public class PackageUI : MonoBehaviour {

    public Action<List<int>> SelectAction;
    public GameObject iconPrefab;

    private List<CreatureFightData> dataList;
    private List<GameObject> entities;
    private List<int> SelectUid;
    private int needNum;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(PackageType type,int selectNum)
    {
        CleanUp();
        dataList = new List<CreatureFightData>();
        entities = new List<GameObject>();
        SelectUid = new List<int>();
        needNum = selectNum;

        var creatures = GameRoot.GetInstance().gameDataManager.GetPackageList(type);
        //gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(690, height);
        ScrollRect scroll = gameObject.GetComponentInChildren<ScrollRect>();
        int creatureNum = creatures.Count;
        scroll.content.sizeDelta = new Vector2(690, Mathf.CeilToInt((float)creatureNum/5f)*133+13);
        for (int i = 0; i < creatureNum;++i){
            dataList.Add(creatures[i]);
            int row = i / 5;
            int col = i - row * 5 + 1;
            GameObject entity = Instantiate(iconPrefab);
            entity.GetComponent<ClickEvent>().clickActionObj += IconClicked;
            entity.transform.parent = scroll.content.gameObject.transform;
            entity.transform.localScale = Vector3.one;
            entity.GetComponent<RectTransform>().localPosition = new Vector2((col - 1) * 133 + 73,-row * 133 - 73);
            entities.Add(entity);
        }
        RefreshView();
    }

    public void IconClicked(GameObject obj){
        int uid = obj.GetComponent<CreatureIconUI>().creatureData.uid;
        if (SelectUid.Contains(uid)){
            SelectUid.Remove(uid);
            RefreshView();
            return;
        }
        if(SelectUid.Count<needNum)
        SelectUid.Add(obj.GetComponent<CreatureIconUI>().creatureData.uid);
        else if(needNum == 1){
            SelectUid[0] = uid;
        }
        RefreshView();
    }

    public void Confirm(){
        SelectAction(SelectUid);
    }

    public void CleanUp(){
        if (entities != null)
        {
            foreach (var obj in entities)
            {
                Destroy(obj);
            }
        }
    }

    public void RefreshView(){
        for (int i = 0; i < dataList.Count;++i){
            entities[i].GetComponent<CreatureIconUI>().InjectData(dataList[i]);
        }
        foreach(var entity in entities){
            CreatureIconUI iconUI = entity.GetComponent<CreatureIconUI>();
            if(SelectUid.Contains(iconUI.creatureData.uid)){
                iconUI.MarkSelected(true);
            }else{
                iconUI.MarkSelected(false);
            }
        }
    }
}
