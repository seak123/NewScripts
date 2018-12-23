using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class MapObstacle : MonoBehaviour {

    // Use this for initialization
    public int maxX;
    public int maxY;
    public int size;
    public int radius;

	void Start () {
        GameRoot.BattleStartAction += RegisterObstacle;
        gameObject.transform.position = new Vector3(maxX * 0.64f - 0.32f, 0, maxY * 0.64f - 0.32f);
	}
	
    void RegisterObstacle(){
        MapField mapField = GameRoot.GetInstance().MapField;
        mapField.CreateStructure(maxX, maxY, size,radius);
    }
}
