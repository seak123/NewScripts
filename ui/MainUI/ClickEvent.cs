using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ClickEvent : MonoBehaviour,IPointerClickHandler,IPointerDownHandler {

    public Action clickAction;

    float time;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        time = Time.time;
    }

    public void OnPointerClick(PointerEventData eventData){
        if (Time.time - time > 0.1f) return;
        if(clickAction!=null){
            clickAction();
        }
    }
}
