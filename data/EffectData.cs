using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectData : ScriptableObject
{
    public int effectId;

    public string effectSocket = "S_Bottom";

    public bool isAutoClean = true;

    public GameObject effectPrefab;
}
