using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CardEntityState{
    Empty = 1,
}

public class CardEntity : MonoBehaviour, IPointerDownHandler{

    private CreatureData baseData;
    private CreatureData data;
    private Button button;


    private void Start()
    {
        //button = GetComponent<Button>();
        //button.on
        //button.onClick.AddListener(NotifyGameState);
    }

    public void NotifyGameState(){
        //Debug.Log("Press!");
        //GameRoot.GetInstance().StateManager
    }

    public void InjectData(){

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameRoot.GetInstance().StateManager.selectCard = this;
    }
}
