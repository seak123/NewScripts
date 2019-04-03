using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCompleted : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Close(){
        GameRoot.GetInstance().mainUIMng.CloseUI();
        GameRoot.GetInstance().ClearBattle();
    }
}
