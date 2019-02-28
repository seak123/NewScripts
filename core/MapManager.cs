using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PowerInject;


[Insert]
public class MapManager : MonoBehaviour {

    private MapSite currSite;
   
    [OnInjected]
    public void AddRootAction()
    {
        GameRoot.moduleInit += Init;
    }

    public MapSite GetCurrSite(){
        return currSite;
    }

    private void Init()
    {
        Debug.Log("MapManager Init");
        InitMap();

    }


    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitMap(){
        //temp data
        currSite = MapSite.SiteCreator(SiteType.Wilds);
    }

}
