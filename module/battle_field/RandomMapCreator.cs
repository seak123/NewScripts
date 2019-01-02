using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Map;

public class RandomMapCreator : MonoBehaviour {
    public Vector2Int[] baseObstacle;
    public float treePercent;
    public GameObject tree;

    // -1-baseObstacle,0-empty,1-tree
    private int[,] mapGrids;
    private int col = BattleDef.columnGridNum / 16;
    private int row = BattleDef.rowGridNum / 16;
    // Use this for initialization
    void Start()
    {
        mapGrids = new int[col, row];
        for (int i = 0; i < col; ++i)
        {
            for (int j = 0; j < row; ++j)
            {
                mapGrids[i, j] = 0;
            }
        }
        foreach (var b in baseObstacle)
        {
            mapGrids[b.x - 1, b.y - 1] = -1;
        }


        for (int i = 0; i < col; ++i)
        {
            for (int j = 0; j < row; ++j)
            {
                if (mapGrids[i, j] == 0)
                {
                    if (UnityEngine.Random.Range(0f, 1f) < treePercent)
                    {
                        mapGrids[i, j] = 1;
                    }
                }
            }
        }
        //for (int count = 0; count < 1; ++count)
        //{
            for (int i = 0; i < col; ++i)
            {
                for (int j = 0; j < row; ++j)
                {
                    if (mapGrids[i, j] == 0)
                    {
                        if (FindRoundGrids(i, j, 1) > 4)
                        {
                            mapGrids[i, j] = 1;
                        }
                    }
                    else if (mapGrids[i, j] == 1)
                    {
                        if (FindRoundGrids(i, j, 1) <4)
                        {
                            mapGrids[i, j] = 0;
                        }
                    }
                }
            }
       // }
        for (int i = 0; i < col; ++i)
        {
            for (int j = 0; j < row; ++j)
            {
                if(mapGrids[i,j]==1){
                    GameObject obj = Instantiate(tree);
                    obj.transform.position = new Vector3((i+1) * 0.64f - 0.32f, 0, (j+1) * 0.64f - 0.32f);
                }
            }
        }

        GameRoot.BattleStartAction += RegisterObstacle;
    }

    int FindRoundGrids(int centerX,int centerY,int attr){
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
                if (mapGrids[roundPos.x, roundPos.y] == attr) ++num;
            }
        }
        return num;
    }

    void RegisterObstacle(){
        MapField mapField = GameRoot.GetInstance().MapField;
        for (int i = 0; i < col; ++i)
        {
            for (int j = 0; j < row; ++j)
            {
                if (mapGrids[i, j] == 1)
                {
                    mapField.CreateStructure(i+1, j+1, 1, 6);
                }
            }
        }
       
    }
	
}
