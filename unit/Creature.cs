using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : BaseUnit {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetAnimation(string name,bool value){
        animator.SetBool(name, value);
    }
}
