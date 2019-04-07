using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ClickEvent : MonoBehaviour,IPointerClickHandler,IPointerDownHandler,IPointerUpHandler {

    public Action clickAction;
    public Action<GameObject> clickActionObj;
    public Action longClickAction;

    float time;
    Vector2 clickPos;
    private bool isUp = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        time = Time.time;
        clickPos = eventData.position;
        isUp = false;
        StartCoroutine(Grow());
        if (Time.time - time < 0.3f) return;
        if (longClickAction != null)
        {
            longClickAction();
        }
    }

    public void OnPointerClick(PointerEventData eventData){
        float dis = Vector2.Distance(eventData.position, clickPos);
        if (Time.time - time > 0.15f) return;
        if (dis > 5) return;
        if(clickAction!=null){
            clickAction();
        }
        if(clickActionObj!=null){
            clickActionObj(gameObject);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isUp = true;
    }

    private IEnumerator Grow(){
        while(true){
            if (isUp) break;
            if(Time.time - time > 0.3f){
                if(longClickAction!=null){
                    longClickAction();
                }
                break;
            }
            yield return null;
        }
    }
}
