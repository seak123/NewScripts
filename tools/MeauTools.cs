using UnityEngine;
using UnityEditor;
using System;
public class MeauTools
{
    [MenuItem("Tools/Create assets")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<ScriptableObject>();
    }
}
