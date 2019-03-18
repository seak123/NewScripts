using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleDef
{
    //sys data
    public static string language = "chinese";

    //map data
    public static bool useCollision = false;

    public static int rowGridNum = 620;

    public static int columnGridNum = 620;


    public static int maxSpeed = 64;

    public static int aStarUpdateFrame = 20;

    public static float Transfer2GridFactor = 25;

    public static int UnitBound = 352;

    public static int StructBound = 22;

    public static int roomBound = 140;

    public static int roomInterval = 50;

    //logic data
    public static float MaxSaving = 20;

    public static float MaxBaseIncome = 1;

    public static float KillEarnFactor = 1.2f;

    public static float StructureEarnFactor = 0.04f;

    public static float UpdateIncomeDelta = 5;

    public static int CardBoxNum = 5;

    public static float CardPushSpeed = 2f;

    //utils
#if UNITY_EDITOR
    public static bool useMouse = true;
#else
    public static bool useMouse = false;
#endif

    public static float cardPanalViewFactor = 1 / 4.8f;

    //card background
    //midcreatrue, natural, people, monster, demon
    public static Color[] backColor = new Color[5] { new Color(0.492f,0.386f,0.207f,1), new Color(0.293f, 0.396f, 0.26f,1), new Color(0.333f,0.41f,0.547f,1), new Color(0.445f,0.308f,0.492f,1), new Color(0.445f,0.105f,0.06f,1) };
}
