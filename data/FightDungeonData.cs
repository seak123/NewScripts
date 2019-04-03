using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightDungeonData : ScriptableObject
{
    public Sprite icon;

    public int baseLevel;

    public int FieldId;

    public int mainCastleId;

    public float mainCastleHp;

    public string[] enemys;
}
