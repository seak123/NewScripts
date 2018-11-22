﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerInject;
using System;

namespace Map
{
    [Insert]
    public class MapField : MonoBehaviour
    {
        public GameObject assitField;
        private bool assistActive=false;

        private bool[,] grids;
        private bool[,] structureGrids;

        private readonly float Transfer2GridFactor = BattleDef.Transfer2GridFactor;
        private readonly float DiagoFactor = 1.4f;//(float)Math.Sqrt(2)
        private readonly int AStarCalcFrame = 10;
        //private float centerOffset = 0.365f;
        private Dictionary<int,Entity> entityMap;
        private Dictionary<int, List<Vector2>> structureMap;
        private List<Vector2> entityRemoveCache;
        private List<int> aStarRequestList;

        private AssetManager mng;

        private int structureUid = 0;

        private void Start()
        {
            entityMap = new Dictionary<int, Entity>();
            structureMap = new Dictionary<int, List<Vector2>>();
            entityRemoveCache = new List<Vector2>();
            aStarRequestList = new List<int>();
        }

        private void Update()
        {
            float alpha = assitField.GetComponent<SpriteRenderer>().material.GetFloat("_Alpha");
            if(assistActive){
                assitField.GetComponent<SpriteRenderer>().material.SetFloat("_Alpha", Math.Max(alpha - 0.5f*Time.deltaTime,0.8f));
            }else{
                assitField.GetComponent<SpriteRenderer>().material.SetFloat("_Alpha", Math.Min(1,alpha + 0.5f*Time.deltaTime));
            }

            ///////remove entity
            List<float> removeCache = new List<float>();
            if(entityRemoveCache.Count!=0){
                for (int i = entityRemoveCache.Count-1; i >= 0;--i){
                    float restTime = entityRemoveCache[i].y - Time.deltaTime;
                    if(restTime<=0){
                        removeCache.Add(entityRemoveCache[i].x);
                        entityRemoveCache.RemoveAt(i);
                    }else{
                        entityRemoveCache[i] = new Vector2(entityRemoveCache[i].x,restTime);
                    }
                }
            }
            foreach(var key in removeCache){
                entityMap.Remove((int)key);
            }
            ///////calculate astar
            for (int i = 0; i < AStarCalcFrame;++i){
                if (aStarRequestList.Count == 0) break;
                int uid = aStarRequestList[0];
                Entity entity = entityMap[uid];
                if(entity != null){
                    Debug.Log("reset");
                    entity.ResetMapNode();
                }
                aStarRequestList.RemoveAt(0);
            }

        }
        //>>>>>>>>>>>>>>>>>map effect>>>>>>>>>>>>>>>>>>>
        public void SetAssitActive(bool active)
        {
            if (active) assistActive = true;
            else assistActive = false;
        }

        //>>>>>>>>>>>>>>>>>map entity>>>>>>>>>>>>>>>>>>>

        public Entity CreateEntity(int id,int uid,int side,int gridX,int gridY,int structUid){
            float x, y;
            Vector2 pos = FindInitPos(gridX,gridY, mng.GetCreatureData(id).radius);
            GetViewPos((int)pos.x, (int)pos.y, out x, out y);

            MarkMovable((int)pos.x, (int)pos.y, mng.GetCreatureData(id).radius, true);
           
            GameObject obj = Instantiate(mng.GetCreatureData(id).prefab, new Vector3(x, 0, y), Quaternion.identity);
            var entity = obj.AddComponent<Entity>();
            //init entity
            entity.id = id;
            entity.uid = uid;
            entity.structUid = structUid;
            entity.side = side;
            entity.radius = mng.GetCreatureData(id).radius;
            entity.posX = (int)pos.x;
            entity.posY = (int)pos.y;
            entity.animator = obj.GetComponentInChildren<Animator>();

            entityMap.Add(uid, entity);
            return entity;
        }

        public void RemoveEntity(Entity entity,float delay){

            MarkMovable(entity.posX, entity.posY, entity.radius, false);
            //entityMap.Remove(entity.uid);
            if(entity.structUid != -1){
                foreach (var point in structureMap[entity.structUid])
                {
                    structureGrids[(int)point.x, (int)point.y] = false;
                }
                structureMap.Remove(entity.structUid);
            }
            entityRemoveCache.Add(new Vector2(entity.uid,delay));
        }

        public Entity FindEntity(int uid){
            return entityMap[uid];
        }

        [OnInjected]
        public void AddRootAction(){
            GameRoot.init += Init;
        }

        public void Init()
        {
            Debug.Log("BattleMap Init");
            grids = new bool[BattleDef.columnGridNum,BattleDef.rowGridNum];
            for (int i = 0; i < BattleDef.columnGridNum;++i){
                for (int j = 0; j < BattleDef.rowGridNum;++j){
                    grids[i,j] = false;
                }
            }
            structureGrids = new bool[BattleDef.columnGridNum/16,BattleDef.rowGridNum/16];
            for (int i = 0; i < BattleDef.columnGridNum/16; ++i){
                for (int j = 0; j < BattleDef.rowGridNum / 16;++j){
                    grids[i, j] = false;
                }
            }
            // remove main castle grids
            for (int x = 0; x < 4;++x){
                for (int y = BattleDef.rowGridNum / 32 - 2; y < BattleDef.rowGridNum / 32 + 2;++y){
                    structureGrids[x, y] = true;
                }
            }
            for (int x = BattleDef.columnGridNum/16-4; x < BattleDef.columnGridNum/16; ++x)
            {
                for (int y = BattleDef.rowGridNum / 32 - 2; y < BattleDef.rowGridNum / 32 + 2; ++y)
                {
                    structureGrids[x, y] = true;
                }
            }

            mng = GameRoot.GetInstance().BattleField.assetManager;
        }

        public void GetGridPos(float x,float y,out int grid_x,out int grid_y){
            grid_x = (int)Math.Floor(x * Transfer2GridFactor);
            grid_y = (int)Math.Floor(y * Transfer2GridFactor);
        }

        public void GetViewPos(int grid_x,int grid_y,out float pos_x,out float pos_y){
            pos_x = grid_x/Transfer2GridFactor;
            pos_y = grid_y/ Transfer2GridFactor;
        }

        public void GetLargeGridPos(int gridX,int gridY,out int lGridX,out int lGridY){
            lGridX = (int)Mathf.Floor(gridX / 16);
            lGridY = (int)Mathf.Floor(gridY / 16);
        }

        public void AddAStarRequestList(int uid){
            if(!aStarRequestList.Contains(uid)){
                aStarRequestList.Add(uid);
            }
        }

        public HeapNode GetAStarRoute(int unit_id,int s_x,int s_y,int e_x,int e_y,int factor){
            Debug.Log("getroute!!!!!" + s_x + " " + s_y + " " + e_x + " " + e_y);
            int radius = mng.GetCreatureData(unit_id).radius;
            int maxG = BattleDef.maxSpeed / 30 * BattleDef.aStarUpdateFrame*factor;

            MapMinHeap heap = new MapMinHeap();
            HeapNode root = new HeapNode();
            HeapNode currNode = null;
            List<HeapNode> CloseList = new List<HeapNode>();
            Dictionary<int,bool> IsCanMoveCache = new Dictionary<int,bool>();
            heap.Push(s_x, s_y, 0f,Distance(s_x, s_y, e_x, e_y),root);
            while (heap.Count() > 0)
            {
                int count = heap.Count();
                currNode = heap.Pop();
                CloseList.Add(currNode);


                if (currNode.X == e_x && currNode.Y == e_y || currNode.G > maxG) break;
                // execute 8-round grids
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
                    Vector2Int roundPos = new Vector2Int(currNode.X, currNode.Y) + direct[i];
                    if (CloseList.FindIndex(node => node.X == roundPos.x && node.Y == roundPos.y) == -1)
                    {
                        int key = roundPos.x * 1000 + roundPos.y;
                        bool flag = false;
                        if (IsCanMoveCache.ContainsKey(key))
                        {
                            flag = IsCanMoveCache[key];
                        }
                        else
                        {
                            flag = IsCanMove(roundPos.x, roundPos.y, radius);
                            IsCanMoveCache.Add(key, flag);
                        }
                        if (flag)
                            heap.Find(roundPos.x, roundPos.y, currNode.G + DiagoFactor, Distance(roundPos.x, roundPos.y, e_x, e_y), currNode);
                    }
                }


            }   
            //     roundX = currNode.X - 1;
            //     roundY = currNode.Y + 1;
            //     if(CloseList.FindIndex(node=>node.X ==roundX&&node.Y==roundY)==-1 && IsCanMove(roundX,roundY,radius)){
            //     heap.Find(roundX, roundY, currNode.G + DiagoFactor, Distance(roundX, roundY, e_x, e_y), currNode);
            //     }

            //     roundX = currNode.X;
            //     roundY = currNode.Y + 1;
            //     if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
            //         heap.Find(roundX, roundY, currNode.G + 1f, Distance(roundX, roundY, e_x, e_y), currNode);

            //     roundX = currNode.X + 1;
            //     roundY = currNode.Y + 1;
            //         if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
            //         heap.Find(roundX, roundY, currNode.G + DiagoFactor, Distance(roundX, roundY, e_x, e_y), currNode);

            //     roundX = currNode.X - 1;
            //     roundY = currNode.Y;
            //         if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
            //         heap.Find(roundX, roundY, currNode.G + 1f, Distance(roundX, roundY, e_x, e_y), currNode);

            //     roundX = currNode.X + 1;
            //     roundY = currNode.Y;
            //         if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
            //         heap.Find(roundX, roundY, currNode.G + 1f, Distance(roundX, roundY, e_x, e_y), currNode);

            //     roundX = currNode.X - 1;
            //     roundY = currNode.Y - 1;
            //         if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
            //         heap.Find(roundX, roundY, currNode.G + DiagoFactor, Distance(roundX, roundY, e_x, e_y), currNode);

            //     roundX = currNode.X;
            //     roundY = currNode.Y - 1;
            //         if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
            //         heap.Find(roundX, roundY, currNode.G + 1f, Distance(roundX, roundY, e_x, e_y), currNode);

            //     roundX = currNode.X + 1;
            //     roundY = currNode.Y - 1;
            //         if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
            //         heap.Find(roundX, roundY, currNode.G + DiagoFactor, Distance(roundX, roundY, e_x, e_y), currNode);
            // }
            while(currNode.Parent != null){
                currNode.Parent.Next = currNode;
                currNode = currNode.Parent;
            }

            return currNode;
        }


        public float Distance(int s_x,int s_y,int e_x,int e_y){
            float dis_x = Math.Abs(e_x - s_x);
            float dis_y = Math.Abs(e_y - s_y);
            //return (float)Math.Sqrt(dis_x * dis_x + dis_y * dis_y);
            float width = Math.Max(dis_x, dis_y);
            float height = Math.Min(dis_x, dis_y);
            return height * DiagoFactor + width - height;
        }

        public Vector2 FindInitPos(int initX,int initY,int radius){
            if(IsCanMove(initX,initY,radius)){
                Debug.Log("canmove");
                return new Vector2(initX,initY);
            }

            Vector2 res = new Vector2(initX, initY);
            Vector2[] direct = new Vector2[4];
            direct[0] = new Vector2(0,1);
            direct[1] = new Vector2(1,0);
            direct[2] = new Vector2(0,-1);
            direct[3] = new Vector2(-1,0);
            int index = 0;
            while(true){
                int length = (int)(index / 2) + 1;
                for(int i = 1;i<= length;++i){
                    res = res + direct[index % 4];
                    if(IsCanMove((int)res.x,(int)res.y,radius)){
                        return res;
                    }
                }
                ++index;
            }
           
        }

        public bool IsCanMove(int grid_x,int grid_y,int radius){
            if (grid_x < 0 || grid_x >= BattleDef.columnGridNum || grid_y < 0 || grid_y >= BattleDef.rowGridNum) return false;
            for (int x = Math.Max(0, grid_x - radius); x < Math.Min(BattleDef.columnGridNum, grid_x + radius);++x){
                for (int y = Math.Max(0, grid_y - radius); y < Math.Min(BattleDef.rowGridNum, grid_y + radius);++y){
                    if (grids[x, y] == true) return false;
                }
            }
            return true;
        }

        public void MarkMovable(int grid_x,int grid_y,int radius,bool cannotMove){
            for (int x = Math.Max(0, grid_x - radius); x < Math.Min(BattleDef.columnGridNum, grid_x + radius); ++x)
            {
                for (int y = Math.Max(0, grid_y - radius); y < Math.Min(BattleDef.rowGridNum, grid_y + radius); ++y)
                {
                    grids[x, y] = cannotMove;
                }
            }
        }

        public static bool CheckPosValiable(int gridX,int gridY){
            if (gridX >= 0 && gridX <= BattleDef.columnGridNum && gridY >= 0 && gridY <= BattleDef.rowGridNum) return true;
            return false;
        }
        

        public bool CheckStructurePosValiable(int gridX,int gridY,int size,out int maxX,out int maxY){
            maxX = 0;
            maxY = 0;
            if(size == 2){
                int centerX = (int)Mathf.Floor((Mathf.Floor(gridX / 8) + 1) / 2)*16;
                int centerY = (int)Mathf.Floor((Mathf.Floor(gridY / 8) + 1) / 2)*16;
                maxX = (int)Mathf.Floor((Mathf.Floor(gridX / 8) + 1) / 2) + 1;
                maxY = (int)Mathf.Floor((Mathf.Floor(gridY / 8) + 1) / 2) + 1;
                if (centerX - 16 < 0 || centerX + 16 > BattleDef.columnGridNum || centerY - 16 < 0 || centerY - 16 > BattleDef.rowGridNum) return false;
                for (int x = centerX - 16; x < centerX + 16;++x){
                    for (int y = centerY - 16; y < centerY + 16;++y){
                        if (grids[x, y] == true) return false;
                    }
                }
                for (int x = maxX - size; x < maxX; ++x)
                {
                    for (int y = maxY - size; y < maxY; ++y)
                    {
                        if (structureGrids[x, y] == true) return false;
                    }
                }
            }
            else if(size == 3){
                int startX = (int)(Mathf.Floor(gridX / 16) - 1) * 16;
                int startY = (int)(Mathf.Floor(gridY / 16) - 1) * 16;
                maxX = (int)(Mathf.Floor(gridX / 16) - 1) + 3;
                maxY = (int)(Mathf.Floor(gridY / 16) - 1) + 3;
                if (startX < 0 || startX + 48 > BattleDef.columnGridNum || startY < 0 || startY > BattleDef.rowGridNum) return false;
                for (int x = startX; x < startX + 48;++x){
                    for (int y = startY; y < startY + 48;++y){
                        if (grids[x, y] == true) return false;
                    }
                }
                for (int x = maxX - size; x < maxX; ++x)
                {
                    for (int y = maxY - size; y < maxY; ++y)
                    {
                        if (structureGrids[x, y] == true) return false;
                    }
                }
            }

            return true;
        }

        public int CreateStructure(int maxX,int maxY,int size){
            structureUid = structureUid + 1;
            List<Vector2> structGrids = new List<Vector2>();
            for (int x = maxX - size; x < maxX;++x){
                for (int y = maxY - size; y < maxY;++y){
                    structureGrids[x, y] = true;
                    structGrids.Add(new Vector2(x,y));
                }
            }
            structureMap.Add(structureUid, structGrids);
            return structureUid;
        }
    }
}
