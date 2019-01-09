using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DungeonUIMaker : MonoBehaviour {

    public GameObject dungeonUnit;
    public GameObject dungeonHero;
    public GameObject map;

    public Vector2 startPos;
    public float sideLength;
    private int sideNum;

    private Vector2Int currPos;
    private Vector2Int selectPos;

    private Dictionary<int, DungeonUnit> dungeonUnits;
    private Dictionary<int, GameObject> dungeonViews;
    private GameObject hero;
	// Use this for initialization
	void Start () {
        dungeonUnits = new Dictionary<int, DungeonUnit>();
        dungeonViews = new Dictionary<int, GameObject>();
        sideNum = GameRoot.GetInstance().DungeonMng.GetSize();

        //init map
        int line = 4 * sideNum - 3;
        for (int index = 1; index <= line;++index){
            Vector2 lineStartPos;
            int gridNum;
            if(index<=sideNum){
                gridNum = index;
                lineStartPos = new Vector2(startPos.x-(index-1)*1.5f*sideLength, startPos.y+(index-1)*Mathf.Sqrt(3)/2*sideLength);
            }else if(index>(line-sideNum)){
                gridNum = line - index + 1;
                lineStartPos = new Vector2(startPos.x - (gridNum - 1) * 1.5f * sideLength, startPos.y + (index - 1) * Mathf.Sqrt(3) / 2 * sideLength);
            }else{
                gridNum = sideNum - (index - sideNum) % 2;
                lineStartPos = new Vector2(startPos.x - (gridNum - 1) * 1.5f * sideLength, startPos.y + (index - 1) * Mathf.Sqrt(3) / 2 * sideLength);
            }
            for (int i = 1; i <= gridNum;++i){
                GameObject obj = Instantiate(dungeonUnit);
                DungeonUnit _unit = obj.GetComponent<DungeonUnit>();
                _unit.Init(GameRoot.GetInstance().DungeonMng.GetDungeonData(index * 100 + i));
                dungeonUnits.Add(index * 100 + i, _unit);
                obj.transform.SetParent(map.transform);
                obj.transform.localPosition = new Vector3(lineStartPos.x+(i-1)*sideLength*3,lineStartPos.y, 0);
                obj.transform.localScale = Vector3.one * 0.5f;
                dungeonViews.Add(index * 100 + i, obj);
            }
        }

        //init hero view
        hero = Instantiate(dungeonHero);
        hero.transform.SetParent(map.transform);
        SetHeroPos();


        GameRoot.GetInstance().DungeonMng.SetMaker(this);

    }

    void SetHeroPos(){
        Vector2Int pos = GameRoot.GetInstance().DungeonMng.GetCurrPos();
        if(pos.x>0&&pos.y>0){
            hero.transform.position = dungeonViews[pos.x * 100 + pos.y].transform.position;
        }else{
            hero.transform.position = dungeonViews[101].transform.position + new Vector3(0, -80f, 0);
        }
    }

    public void SetSelectPos(Vector2Int _pos){
        selectPos = _pos;
    }

    public void ForwardUnit(){
        currPos = selectPos;
        hero.transform.DOMove(dungeonViews[selectPos.x * 100 + selectPos.y].transform.position, 1f).onComplete +=UpdateDungenInfo;
    }

    private void UpdateDungenInfo(){
        GameRoot.GetInstance().DungeonMng.SetCurrPos(currPos);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
