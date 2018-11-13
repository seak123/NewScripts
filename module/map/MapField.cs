using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerInject;
using System;

namespace Map
{
    [Insert]
    public class MapField : MonoBehaviour
    {
        private bool[,] grids;
        private readonly float Transfer2GridFactor = BattleDef.Transfer2GridFactor;
        private readonly float DiagoFactor = 1.4f;//(float)Math.Sqrt(2)
        //private float centerOffset = 0.365f;
        private Dictionary<int,Entity> entityMap;

        private AssetManager mng;

        private void Start()
        {
            entityMap = new Dictionary<int, Entity>();
        }

        public Entity CreateEntity(int id,int uid,int side,int gridX,int gridY){
            float x, y;
            Vector2 pos = FindInitPos(gridX,gridY, mng.GetCreatureData(id).radius);
            GetViewPos((int)pos.x, (int)pos.y, out x, out y);

            MarkMovable((int)pos.x, (int)pos.y, mng.GetCreatureData(id).radius, true);
           
            GameObject obj = Instantiate(mng.GetCreatureData(id).prefab, new Vector3(x, 0, y), Quaternion.identity);
            var entity = obj.AddComponent<Entity>();
            //init entity
            entity.id = id;
            entity.uid = uid;
            entity.side = side;
            entity.radius = mng.GetCreatureData(id).radius;
            entity.posX = (int)pos.x;
            entity.posY = (int)pos.y;
            entity.animator = obj.GetComponentInChildren<Animator>();

            entityMap.Add(uid, entity);
            return entity;
        }

        public void RemoveEntity(Entity entity){
            MarkMovable(entity.posX, entity.posY, entity.radius, false);
            entityMap.Remove(entity.uid);
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

        public HeapNode GetAStarRoute(int unit_id,int s_x,int s_y,int e_x,int e_y){
            int radius = mng.GetCreatureData(unit_id).radius;
            int maxG = BattleDef.maxSpeed / 30 * BattleDef.aStarUpdateFrame;

            MapMinHeap heap = new MapMinHeap();
            HeapNode root = new HeapNode();
            HeapNode currNode = null;
            List<HeapNode> CloseList = new List<HeapNode>();
            heap.Push(s_x, s_y, 0f,Distance(s_x, s_y, e_x, e_y),root);
            while (heap.Count()>0){
                int count = heap.Count();
                currNode = heap.Pop();
                CloseList.Add(currNode);
                int roundX, roundY;

                if (currNode.X == e_x && currNode.Y == e_y || currNode.G>maxG) break;

                // execute 8-round grids
                roundX = currNode.X - 1;
                roundY = currNode.Y + 1;
                if(CloseList.FindIndex(node=>node.X ==roundX&&node.Y==roundY)==-1 && IsCanMove(roundX,roundY,radius))
                heap.Find(roundX, roundY, currNode.G + DiagoFactor, Distance(roundX, roundY, e_x, e_y), currNode);

                roundX = currNode.X;
                roundY = currNode.Y + 1;
                if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
                    heap.Find(roundX, roundY, currNode.G + 1f, Distance(roundX, roundY, e_x, e_y), currNode);

                roundX = currNode.X + 1;
                roundY = currNode.Y + 1;
                    if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
                    heap.Find(roundX, roundY, currNode.G + DiagoFactor, Distance(roundX, roundY, e_x, e_y), currNode);

                roundX = currNode.X - 1;
                roundY = currNode.Y;
                    if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
                    heap.Find(roundX, roundY, currNode.G + 1f, Distance(roundX, roundY, e_x, e_y), currNode);

                roundX = currNode.X + 1;
                roundY = currNode.Y;
                    if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
                    heap.Find(roundX, roundY, currNode.G + 1f, Distance(roundX, roundY, e_x, e_y), currNode);

                roundX = currNode.X - 1;
                roundY = currNode.Y - 1;
                    if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
                    heap.Find(roundX, roundY, currNode.G + DiagoFactor, Distance(roundX, roundY, e_x, e_y), currNode);

                roundX = currNode.X;
                roundY = currNode.Y - 1;
                    if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
                    heap.Find(roundX, roundY, currNode.G + 1f, Distance(roundX, roundY, e_x, e_y), currNode);

                roundX = currNode.X + 1;
                roundY = currNode.Y - 1;
                    if (CloseList.FindIndex(node => node.X == roundX && node.Y == roundY) == -1 && IsCanMove(roundX, roundY, radius))
                    heap.Find(roundX, roundY, currNode.G + DiagoFactor, Distance(roundX, roundY, e_x, e_y), currNode);
            }
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
                        Debug.Log(res.x);
                        Debug.Log(res.y);
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
    }
}
