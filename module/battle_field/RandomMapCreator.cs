﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Map;

public class RandomMapCreator : MonoBehaviour {
    public float basePercent;
    public int opTimes;
    public GameObject[] baseObstacle;
    public float transferPercent;
    public float appearPercent;
    public GameObject[] decorator;

    // -2:buildArea,-1:backObstacle,0:empty,1-x:base&x_type,2-x:decorator&x_type
    private int[,] mapGrids;
    private int col = BattleDef.columnGridNum / 16;
    private int row = BattleDef.rowGridNum / 16;
    private int bound = 10;
    private int mCol;
    private int mRow;

    private List<GameObject> objCache;
    // Use this for initialization
    void Start()
    {
        //init map grids
        mCol = col+bound*2;
        mRow = row+bound*2;
        mapGrids = new int[mCol, mRow];
        objCache = new List<GameObject>();

        for (int i = 0; i < mCol;++i){
            for (int j = 0; j < mRow;++j){
                mapGrids[i, j] = 0;
            }
        }



        InitBaseObstacle();

        //OptimizeObstacle(opTimes);

        //CreateDecorator();

        InstantiateObstacle();
       

        GameRoot.BattleStartAction += RegisterObstacle;
        GameRoot.BattleEndAction += CleanUp;
    }

    public void CleanUp(){
        foreach(var obj in objCache){
            Destroy(obj);
        }
    }

    private void InitBaseObstacle(){
        int count = baseObstacle.Length;
        int curr_type = UnityEngine.Random.Range(1,count+1);
        for (int i = 0; i < mCol; ++i)
        {
            for (int j = 0; j < mRow; ++j)
            {
                if (i >= bound && i < mCol - bound && j >= bound && j < mRow - bound) continue;
                if (mapGrids[i, j] == 0)
                {
                    float flag = UnityEngine.Random.Range(0f,1f);
                    if (flag < basePercent)
                    {
                        if(flag<0.1f){
                            curr_type = UnityEngine.Random.Range(1,count+1);
                        }
                        mapGrids[i, j] = 10+curr_type-1;
                    }
                }
            }
        }
    }

    private void OptimizeObstacle(int times){
        for (int count = 0; count < times; ++count)
        {
            for (int i = 0; i < col; ++i)
            {
                for (int j = 0; j < row; ++j)
                {
                    int type = 0;
                    if (mapGrids[i, j] == 0)
                    {
                        if (FindRoundGrids(i, j, false,out type) > 4)
                        {
                            mapGrids[i, j] = 10+type;
                        }
                    }
                    else if (mapGrids[i, j] > 0)
                    {
                        if (FindRoundGrids(i, j, false,out type) <2)
                        {
                            mapGrids[i, j] = 0;
                        }
                    }
                }
            }
            for (int i = 0; i < col; ++i)
            {
                for (int j = 0; j < row; ++j)
                {
                    int type = 0;
                    if (mapGrids[i, j] == 0)
                    {
                        if (FindRoundGrids(i, j, false, out type) > 4)
                        {
                            mapGrids[i, j] = 10 + type;
                        }
                    }
                    else if (mapGrids[i, j] > 0)
                    {
                        if (FindRoundGrids(i, j, false, out type) > 2)
                        {
                            mapGrids[i, j] = 0;
                        }
                    }
                }
            }
        }
    }

    private void CreateDecorator(){
        int count = decorator.Length;
        for (int i = 0; i < col; ++i)
        {
            for (int j = 0; j < row; ++j)
            {
                int type = 0;
                int num = FindRoundGrids(i,j,false,out type);
                if(num<4 && num>0){
                    if(UnityEngine.Random.Range(0f,1f)<transferPercent){
                        mapGrids[i,j] =20+ UnityEngine.Random.Range(1, count+1)-1;
                    }
                }else if(num == 0){
                    if (UnityEngine.Random.Range(0f, 1f) < appearPercent)
                    {
                        mapGrids[i, j] = 20 + UnityEngine.Random.Range(1, count+1) - 1;
                    }
                }
            }
        }
    }

    private void InstantiateObstacle(){
        for (int i = 0; i < mCol; ++i)
        {
            for (int j = 0; j < mRow; ++j)
            {
                int value = mapGrids[i, j];
                if(value>0&&value<20 ){
                    GameObject obj = Instantiate(baseObstacle[value%10]);
                    objCache.Add(obj);
                    obj.transform.position = new Vector3((i+1) * 0.64f - 0.32f-bound*0.64f, 0, (j+1) * 0.64f - 0.32f - bound * 0.64f);
                }else if(value>=20){
                    //Debug.Log("index:" +value%10);
                    GameObject obj = Instantiate(decorator[value % 10]);
                    objCache.Add(obj);
                    obj.transform.position = new Vector3((i + 1) * 0.64f - 0.32f - bound * 0.64f, 0, (j + 1) * 0.64f - 0.32f - bound * 0.64f);
                }
            }
        }
    }

    int FindRoundGrids(int centerX,int centerY,bool Empty,out int type){
        int num = 0;
        
        Vector2Int[] direct = new Vector2Int[8];
        direct[0] = new Vector2Int(-1, 1);
        direct[1] = new Vector2Int(0, 1);
        direct[2] = new Vector2Int(1, 1);
        direct[3] = new Vector2Int(-1, 0);
        direct[4] = new Vector2Int(1, 0);
        direct[5] = new Vector2Int(-1, -1);
        direct[6] = new Vector2Int(0, -1);
        direct[7] = new Vector2Int(1, -1);

        for (int i = 0; i < 8; ++i)
        {
            Vector2Int roundPos = new Vector2Int(centerX, centerY) + direct[i];
            if(roundPos.x>=0&&roundPos.x<col&&roundPos.y>=0&&roundPos.y<row){
                if ((mapGrids[roundPos.x, roundPos.y] >0)!=Empty) { ++num; type = mapGrids[roundPos.x, roundPos.y] % 10; }
            }
        }
        type = 0;
        return num;
    }

    void RegisterObstacle(){
        MapField mapField = GameRoot.GetInstance().MapField;
        for (int i = 0; i < col; ++i)
        {
            for (int j = 0; j < row; ++j)
            {
                int value = mapGrids[i, j];
                if (value== -1)
                {
                    mapField.CreateStructure(i+1, j+1, 1, 6);
                }else if(value>0 && value <20){
                    mapField.CreateStructure(i + 1, j + 1, 1, 6);
                }
            }
        }
       
    }
	
}
