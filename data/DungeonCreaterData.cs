using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungeonFieldType
{
    Plain
}

public enum DungeonUnitType
{
    Close,
    Empty,
    Fight
}


public class DungeonCreaterData : ScriptableObject
{
    public Vector2Int pos;
    public DungeonFieldType fieldType;
    public DungeonUnitType unitType;
    public int dungeonLevel;
}
