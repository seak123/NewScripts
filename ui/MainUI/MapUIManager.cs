using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUIManager : MonoBehaviour,ISceneUI {


	// Use this for initialization
	void Start () {

        Init();
	}

    private void Init()
    {
       

    }

    public void RefreshView(){

    }

    public void OnEnter(){

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void EnterDailyPlan(){
        GameRoot.GetInstance().mainUIMng.OpenUI(15);
    }

    public void EnterStrategy(){
        GameRoot.GetInstance().StartStrategy();
    }
}
