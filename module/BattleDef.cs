using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleDef
{

    //map data
    public static int rowGridNum = 448;

    public static int columnGridNum = 992;


    public static int maxSpeed = 64;

    public static int aStarUpdateFrame = 20;

    public static float Transfer2GridFactor = 25;

    public static int UnitBound = 144;

    public static int StructBound = 9;


    //utils
#if UNITY_EDITOR
    public static bool useMouse = true;
#else
    public static bool useMouse = false;
#endif

    public static float cardPanalViewFactor = 1 / 4.8f;

    //card background
    public static Color[] backColor = new Color[5] { Color.grey, new Color(0, 255, 60), Color.blue, Color.red, Color.black };
}
