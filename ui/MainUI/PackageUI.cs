using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PackageType{
    IdleCreature = 1,
}

public class PackageUI : MonoBehaviour {

    public Action<List<CreatureFightData>> SelectAction;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(PackageType type,int num)
    {

    }
}
