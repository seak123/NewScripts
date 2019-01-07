using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonUIMaker : MonoBehaviour {

    public GameObject dungeonUnit;
    public GameObject map;

    public Vector2 startPos;
    public float sideLength;
    public int sideNum;

    private Dictionary<int, DungeonUnit> dungeonUnits;
	// Use this for initialization
	void Start () {
        dungeonUnits = new Dictionary<int, DungeonUnit>();
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
            }
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
