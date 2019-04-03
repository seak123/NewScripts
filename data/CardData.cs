using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType{
    Creature = 1,
    Structure = 2,
    Magic = 3,
    Hero =4
}

public class CardData : ScriptableObject
{
    public int cardId;

    public string cardName;

    public CardType cardType;

    public int race;

    public Sprite icon;

    public GameObject entityPrefab;

    public int skillId;

    public int cost;

    public float liveTime = -1;

    //public float lost;

    public int[] decorator;
    
    //create prop
    public int unitId;

    public int num;

    //structure prop
    public int size;
}
   
