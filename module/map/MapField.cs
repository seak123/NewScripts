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
        public GameObject assitField;
        public GameObject temRoom;
        public GameObject temWall;
        public GameObject temPath;
        public GameObject temBossRoom;
        public GameObject temPortal;
        private bool assistActive=false;

        private int[,] grids;
        private bool[,] structureGrids;
        //private bool[,] naviGrids;

        private readonly float Transfer2GridFactor = BattleDef.Transfer2GridFactor;
        private readonly float DiagoFactor = 1.4f;//(float)Math.Sqrt(2)
        private readonly int AStarCalcFrame = 10;
        //private float centerOffset = 0.365f;
        private Dictionary<int,Entity> entityMap;
        private Dictionary<int, List<Vector2>> structureMap;
        private Dictionary<Vector2,Vector2> roomMap;
        private List<Vector2> entityRemoveCache;
        // private List<int> aStarRequestList;
        private List<GameObject> mapObjList;

        private AssetManager mng;

        private int structureUid = 0;
        private bool isInited = false;

        private void Start()
        {
           
            // aStarRequestList = new List<int>();
        }

        private void Update()
        {
            if (isInited)
            {
                float alpha = assitField.GetComponent<SpriteRenderer>().material.GetFloat("_Alpha");
                if (assistActive)
                {
                    assitField.GetComponent<SpriteRenderer>().material.SetFloat("_Alpha", Math.Max(alpha - 0.5f * Time.deltaTime, 0.8f));
                }
                else
                {
                    assitField.GetComponent<SpriteRenderer>().material.SetFloat("_Alpha", Math.Min(1, alpha + 0.5f * Time.deltaTime));
                }

                ///////remove entity
                List<float> removeCache = new List<float>();
                if (entityRemoveCache.Count != 0)
                {
                    for (int i = entityRemoveCache.Count - 1; i >= 0; --i)
                    {
                        float restTime = entityRemoveCache[i].y - Time.deltaTime;
                        if (restTime <= 0)
                        {
                            removeCache.Add(entityRemoveCache[i].x);
                            entityRemoveCache.RemoveAt(i);
                        }
                        else
                        {
                            entityRemoveCache[i] = new Vector2(entityRemoveCache[i].x, restTime);
                        }
                    }
                }
                foreach (var key in removeCache)
                {
                    entityMap.Remove((int)key);
                }
            }

        }
        //>>>>>>>>>>>>>>>>>map effect>>>>>>>>>>>>>>>>>>>
        public void SetAssitActive(bool active)
        {
            if (active) assistActive = true;
            else assistActive = false;
        }

        //>>>>>>>>>>>>>>>>>map entity>>>>>>>>>>>>>>>>>>>

        public void CreatePortal(int gridX, int gridY)
        {
            float x, y;
            GetViewPos(gridX, gridY, out x, out y);
            GameObject obj = Instantiate(temPortal, new Vector3(x, 1.7f, y), Quaternion.identity);
            obj.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            mapObjList.Add(obj);
        }

        public void CreateBossRoom(int gridX,int gridY){
            float x, y;
            GetViewPos(gridX, gridY, out x, out y);
            GameObject obj = Instantiate(temBossRoom, new Vector3(x, 1.7f, y), Quaternion.identity);
            obj.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            mapObjList.Add(obj);
        }

        public void CreateRoom(int gridX,int gridY){
            float x, y;
            GetViewPos(gridX, gridY, out x, out y);
            GameObject obj = Instantiate(temRoom, new Vector3(x, 1.7f, y), Quaternion.identity);
            obj.transform.rotation = Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0, 4) * 90, 0));
            mapObjList.Add(obj);
        }

        public void CreateWall(int gridX,int gridY,int rotation){
            float x, y;
            GetViewPos(gridX, gridY, out x, out y);
            GameObject obj = Instantiate(temWall, new Vector3(x, 1.7f, y), Quaternion.identity);
            obj.transform.rotation = Quaternion.Euler(0,rotation,0);
            mapObjList.Add(obj);
        }

        public void CreatePath(int gridX, int gridY, int rotation)
        {
            float x, y;
            GetViewPos(gridX, gridY, out x, out y);
            GameObject obj = Instantiate(temPath, new Vector3(x, 1.9f, y), Quaternion.identity);
            obj.transform.rotation = Quaternion.Euler(0, rotation, 0);
            mapObjList.Add(obj);
        }

        public Entity CreateEntity(int id,int uid,int side,int gridX,int gridY,int room_id){
            float x, y;
            Vector2 pos = FindInitPos(gridX,gridY, mng.GetCreatureData(id).radius,room_id);
            CreatureData data = mng.GetCreatureData(id);
            GetViewPos((int)pos.x, (int)pos.y, out x, out y);

            MarkMovable((int)pos.x, (int)pos.y, data.radius, true);
           
            GameObject obj = Instantiate(mng.GetUnitPrefab(data.prefab), new Vector3(x, 0, y), Quaternion.identity);
            var entity = obj.AddComponent<Entity>();
            //init entity
            entity.id = id;
            entity.uid = uid;
            entity.type = data.type;
            entity.side = side;
            entity.genus = data.genus;
            entity.radius = data.radius;
            entity.cost = data.cost;
            entity.posX = (int)pos.x;
            entity.posY = (int)pos.y;
            entity.animator = obj.GetComponentInChildren<Animator>();

            entityMap.Add(uid, entity);
            entity.gameObject.SetActive(false);
            return entity;
        }

        public void PortalEntity(Entity entity,int _x,int _y){
            float x, y;
            GetViewPos(_x, _y, out x, out y);
            MarkMovable(entity.posX, entity.posY, entity.radius, false);
            MarkMovable(_x, _y, entity.radius, true);
            entity.gameObject.transform.position = new Vector3(x, 0, y);
            entity.posX = _x;
            entity.posY = _y;
        }

        public void RemoveEntity(Entity entity,float delay){

            MarkMovable(entity.posX, entity.posY, entity.radius, false);
            //entityMap.Remove(entity.uid);

            entityRemoveCache.Add(new Vector2(entity.uid,delay));
        }

        public Entity FindEntity(int uid){
            return entityMap[uid];
        }

        [OnInjected]
        public void AddRootAction(){
            GameRoot.moduleInit += Init;
            GameRoot.BattleStartAction += StartBattle;
            GameRoot.BattleEndAction += CleanUp;
        }

        public void Init()
        {
            Debug.Log("BattleMap Init");
            mng = GameRoot.GetInstance().BattleField.assetManager;
        }

        public void StartBattle(){
            isInited = true;
            entityMap = new Dictionary<int, Entity>();
            structureMap = new Dictionary<int, List<Vector2>>();
            roomMap = new Dictionary<Vector2,Vector2>();
            entityRemoveCache = new List<Vector2>();
            mapObjList = new List<GameObject>();

            grids = new int[BattleDef.columnGridNum, BattleDef.rowGridNum];
            for (int i = 0; i < BattleDef.columnGridNum; ++i)
            {
                for (int j = 0; j < BattleDef.rowGridNum; ++j)
                {
                    grids[i, j] = 0;
                }
            }
            structureGrids = new bool[BattleDef.columnGridNum / 16, BattleDef.rowGridNum / 16];
            for (int i = 0; i < BattleDef.columnGridNum / 16; ++i)
            {
                for (int j = 0; j < BattleDef.rowGridNum / 16; ++j)
                {
                    structureGrids[i, j] = false;
                }
            }
           
            for (int x = BattleDef.columnGridNum / 16 - 4; x < BattleDef.columnGridNum / 16; ++x)
            {
                for (int y = BattleDef.rowGridNum / 32 - 2; y < BattleDef.rowGridNum / 32 + 2; ++y)
                {
                    structureGrids[x, y] = true;
                }
            }


        }

        public void CleanUp(){
            Debug.Log("Clean BattleMap");
            foreach(var entity in entityMap){
                Destroy(entity.Value.gameObject);
            }
            foreach(var obj in mapObjList){
                Destroy(obj);
            }
            grids = null;
            structureGrids = null;
            entityMap = null;
            structureMap = null;
            entityRemoveCache = null;
            mapObjList = null;
            isInited = false;
            // remove main castle grids

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

        public Vector2 GetRoute(float s_x,float s_y,float e_x,float e_y,float value){
            int s_X, s_Y;
            int e_X, e_Y;
            GetGridPos(s_x, s_y, out s_X, out s_Y);
            GetGridPos(e_x, e_y, out e_X, out e_Y);
            Vector2Int startRoom = GetRoomId(s_X,s_Y);
            Vector2Int endRoom = GetRoomId(e_X,e_Y);
            if(startRoom.x == endRoom.x && startRoom.y == endRoom.y){
                float nowViewX = s_x;
                float nowViewY = s_y;

                float toViewX = e_x;
                float toViewY = e_y;

                float factor = value / BattleDef.Transfer2GridFactor / Vector2.Distance(new Vector2(toViewX, toViewY), new Vector2(nowViewX, nowViewY));
                float nextViewX = Mathf.Max(0, Mathf.Min(BattleDef.columnGridNum - 1, nowViewX + (toViewX - nowViewX) * factor));
                float nextViewY = Mathf.Max(0, Mathf.Min(BattleDef.rowGridNum - 1, nowViewY + (toViewY - nowViewY) * factor));
                return new Vector2(nextViewX,nextViewY);
            }
            else if(startRoom.x == 0 && startRoom.y == 0 && endRoom.x != 0){
                int maxX = endRoom.x * (BattleDef.roomBound + BattleDef.roomInterval);
                int minX = maxX - BattleDef.roomBound;
                int maxY = endRoom.y * (BattleDef.roomBound + BattleDef.roomInterval);
                int minY = maxY - BattleDef.roomBound;
                int desX, desY;
                float des_x, des_y;
                desX = Mathf.Clamp(s_X, minX + 8, maxX - 8);
                desY = Mathf.Clamp(s_Y, minY + 8, maxY - 8);
                GetViewPos(desX, desY, out des_x, out des_y);

                float nowViewX = s_x;
                float nowViewY = s_y;

                float toViewX = des_x;
                float toViewY = des_y;

                float factor = value / BattleDef.Transfer2GridFactor / Vector2.Distance(new Vector2(toViewX, toViewY), new Vector2(nowViewX, nowViewY));
                float nextViewX = Mathf.Max(0, Mathf.Min(BattleDef.columnGridNum - 1, nowViewX + (toViewX - nowViewX) * factor));
                float nextViewY = Mathf.Max(0, Mathf.Min(BattleDef.rowGridNum - 1, nowViewY + (toViewY - nowViewY) * factor));
                return new Vector2(nextViewX, nextViewY);
            }
            else if(endRoom.x == 0 && endRoom.y == 0 && startRoom.x != 0){
                int maxX = startRoom.x * (BattleDef.roomBound + BattleDef.roomInterval);
                int minX = maxX - BattleDef.roomBound;
                int maxY = startRoom.y * (BattleDef.roomBound + BattleDef.roomInterval);
                int minY = maxY - BattleDef.roomBound;
                int desX, desY;
                float des_x, des_y;
                desX = Mathf.Clamp(e_X, minX + 8, maxX - 8);
                desY = Mathf.Clamp(e_Y, minY + 8, maxY - 8);
                GetViewPos(desX, desY, out des_x, out des_y);

                float nowViewX = s_x;
                float nowViewY = s_y;

                float toViewX = des_x;
                float toViewY = des_y;

                float factor = value / BattleDef.Transfer2GridFactor / Vector2.Distance(new Vector2(toViewX, toViewY), new Vector2(nowViewX, nowViewY));
                float nextViewX = Mathf.Max(0, Mathf.Min(BattleDef.columnGridNum - 1, nowViewX + (toViewX - nowViewX) * factor));
                float nextViewY = Mathf.Max(0, Mathf.Min(BattleDef.rowGridNum - 1, nowViewY + (toViewY - nowViewY) * factor));
                return new Vector2(nextViewX, nextViewY);
            }
            else{
                int maxX = startRoom.x * (BattleDef.roomBound + BattleDef.roomInterval);
                int minX = maxX - BattleDef.roomBound;
                int maxY = startRoom.y * (BattleDef.roomBound + BattleDef.roomInterval);
                int minY = maxY - BattleDef.roomBound;
                int desX, desY;
                float des_x, des_y;
                desX = Mathf.Clamp(e_X, minX , maxX);
                desY = Mathf.Clamp(e_Y, minY, maxY);
                GetViewPos(desX, desY, out des_x, out des_y);

                float nowViewX = s_x;
                float nowViewY = s_y;

                float toViewX = des_x;
                float toViewY = des_y;

                float factor = value / BattleDef.Transfer2GridFactor / Vector2.Distance(new Vector2(toViewX, toViewY), new Vector2(nowViewX, nowViewY));
                float nextViewX = Mathf.Max(0, Mathf.Min(BattleDef.columnGridNum - 1, nowViewX + (toViewX - nowViewX) * factor));
                float nextViewY = Mathf.Max(0, Mathf.Min(BattleDef.rowGridNum - 1, nowViewY + (toViewY - nowViewY) * factor));
                return new Vector2(nextViewX, nextViewY);
            }

        }

        public Vector2Int GetRoomId(int _x,int _y){
            int indexX = _x / (BattleDef.roomBound + BattleDef.roomInterval);
            int indexY = _y / (BattleDef.roomBound + BattleDef.roomInterval);
            int maxX = (indexX + 1) * (BattleDef.roomBound + BattleDef.roomInterval);
            int maxY = (indexY + 1) * (BattleDef.roomBound + BattleDef.roomInterval);
            if(_x<maxX && _x>maxX-BattleDef.roomBound && _y<maxY && _y>maxY-BattleDef.roomBound){
                return new Vector2Int(indexX + 1, indexY + 1);
            }else{
                return Vector2Int.zero;
            }

        }

        public Vector2Int GetRoomViewId(int _x,int _y){

            GameDataManager mng = GameRoot.GetInstance().gameDataManager;
            Vector2Int temId = GetRoomId(_x,_y);
            int start_row = 3;
            int end_row = 2 + mng.roomRow;
            int start_col = 3 - (int)(mng.roomCol/2);
            int end_col = 2 + mng.roomCol;
            if(temId.x <= end_row && temId.x >= start_row && temId.y <= end_col && temId.y >= start_col){
                return temId;
            }else{
                int boss_room_max_x = 2*(BattleDef.roomBound + BattleDef.roomInterval);
                int boss_room_max_y = 3*(BattleDef.roomBound + BattleDef.roomInterval) + (int)0.1*BattleDef.roomBound;
                int boss_room_min_x = boss_room_max_x - (int)1.2*BattleDef.roomBound;
                int boss_room_min_y = boss_room_max_y - (int)1.2*BattleDef.roomBound;
                if(_x <= boss_room_max_x && _x >= boss_room_min_x && _y <= boss_room_max_y && _y >= boss_room_min_y){
                    return new Vector2Int(2,3);
                }
            }
            return Vector2Int.zero;
        }

        public Vector2Int GetRoomCenter(int x,int y){
            if(x==2&&y==3){
                return new Vector2Int((x+1) * (BattleDef.roomBound + BattleDef.roomInterval) - (int)(BattleDef.roomBound*1.1)-BattleDef.roomInterval,
                                      (y - 1) * (BattleDef.roomBound + BattleDef.roomInterval) + BattleDef.roomBound / 2 + BattleDef.roomInterval
                                     );
            }
            else{
                return new Vector2Int((x - 1) * (BattleDef.roomBound + BattleDef.roomInterval) + BattleDef.roomBound / 2 + BattleDef.roomInterval,
                                      (y - 1) * (BattleDef.roomBound + BattleDef.roomInterval) + BattleDef.roomBound / 2 + BattleDef.roomInterval
                                     );
            }
        }

        // public void AddAStarRequestList(int uid){
        //     if(!aStarRequestList.Contains(uid)){
        //         aStarRequestList.Add(uid);
        //     }
        // }

        // public HeapNode GetAStarRoute(int unit_id,int s_x,int s_y,int e_x,int e_y,int factor){

        //     //bool[,] prioGrids = new bool[BattleDef.columnGridNum,BattleDef.rowGridNum];
        //     //for(int x=0;x<BattleDef.columnGridNum;++x){
        //     //    for(int y=0;y<BattleDef.rowGridNum;++y){
        //     //        prioGrids[x,y] = false;
        //     //    }
        //     //}
        //     //GetPrioAStarRoute(s_x,s_y,e_x,e_y,factor,ref prioGrids);

        //     int radius = mng.GetCreatureData(unit_id).radius;
        //     //int maxG = BattleDef.maxSpeed / 30 * BattleDef.aStarUpdateFrame*factor;
        //     int maxG = (int)(factor*1.4);

        //     MapMinHeap heap = new MapMinHeap();
        //     heap.SetBound(BattleDef.columnGridNum,BattleDef.rowGridNum);
        //     HeapNode root = new HeapNode();
        //     HeapNode currNode = null;
        //     List<HeapNode> CloseList = new List<HeapNode>();
        //     Dictionary<int,bool> IsCanMoveCache = new Dictionary<int,bool>();
        //     heap.Push(s_x, s_y, 0f,Distance(s_x, s_y, e_x, e_y),root);
        //     while (heap.Count() > 0)
        //     {
        //             int count = heap.Count();
        //             currNode = heap.Pop();
        //             CloseList.Add(currNode);

        //             if (currNode.X == e_x && currNode.Y == e_y || currNode.G > maxG) break;
        //             // execute 8-round grids
        //             Vector2Int[] direct = new Vector2Int[8];
        //             direct[0] = new Vector2Int(-1, 1);
        //             direct[1] = new Vector2Int(0, 1);
        //             direct[2] = new Vector2Int(1, 1);
        //             direct[3] = new Vector2Int(-1, 0);
        //             direct[4] = new Vector2Int(1, 0);
        //             direct[5] = new Vector2Int(-1, -1);
        //             direct[6] = new Vector2Int(0, -1);
        //             direct[7] = new Vector2Int(1, -1);

        //             for (int i = 0; i < 8; ++i)
        //             {
        //                 Vector2Int roundPos = new Vector2Int(currNode.X, currNode.Y) + direct[i];
        //                 if (CloseList.FindIndex(node => node.X == roundPos.x && node.Y == roundPos.y) == -1)
        //                 {
        //                     int key = roundPos.x * 1000 + roundPos.y;
        //                     bool flag = false;
        //                     if (IsCanMoveCache.ContainsKey(key))
        //                     {
        //                         flag = IsCanMoveCache[key];
        //                     }
        //                     else
        //                     {
        //                         flag = IsCanMove(roundPos.x, roundPos.y, radius);
        //                         IsCanMoveCache.Add(key, flag);
        //                     }
        //                 if (flag)
        //                 {
        //                     if(direct[i].x*direct[i].y!=0)heap.Find(roundPos.x, roundPos.y, currNode.G + DiagoFactor, Distance(roundPos.x, roundPos.y, e_x, e_y), currNode);
        //                     else heap.Find(roundPos.x, roundPos.y, currNode.G + 1, Distance(roundPos.x, roundPos.y, e_x, e_y), currNode);
        //                 }
        //                 }
        //             }


        //         }   

        //         while(currNode.Parent != null){
        //             currNode.Parent.Next = currNode;
        //             currNode = currNode.Parent;
        //         }

        //         return currNode;

        // }


        public float Distance(int s_x,int s_y,int e_x,int e_y){
            float dis_x = Math.Abs(e_x - s_x);
            float dis_y = Math.Abs(e_y - s_y);
            //return (float)Math.Sqrt(dis_x * dis_x + dis_y * dis_y);
            float width = Math.Max(dis_x, dis_y);
            float height = Math.Min(dis_x, dis_y);
            return height * DiagoFactor + width - height;
        }

        public Vector2 FindInitPos(int initX,int initY,int radius,int room_id){
            if(IsCanMove(initX,initY,radius)){
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
                if(index > 200){
                    return new Vector2(initX, initY);
                }
            }
           
        }

        public bool IsCanMove(int grid_x,int grid_y,int radius){
            if (grid_x < 0 || grid_x >= BattleDef.columnGridNum || grid_y < 0 || grid_y >= BattleDef.rowGridNum) return false;
            for (int x = Math.Max(0, grid_x - radius); x < Math.Min(BattleDef.columnGridNum, grid_x + radius);++x){
                for (int y = Math.Max(0, grid_y - radius); y < Math.Min(BattleDef.rowGridNum, grid_y + radius);++y){
                    if (grids[x, y] > 0) return false;
                }
            }
            return true;
        }

        public void MarkMovable(int grid_x,int grid_y,int radius,bool cannotMove){
            int flag = 0;
            if(cannotMove)flag += 1;
            else flag -= 1;
            for (int x = Math.Max(0, grid_x - radius); x < Math.Min(BattleDef.columnGridNum, grid_x + radius); ++x)
            {
                for (int y = Math.Max(0, grid_y - radius); y < Math.Min(BattleDef.rowGridNum, grid_y + radius); ++y)
                {
                    grids[x, y] += flag;
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
                        if (grids[x, y] >0) return false;
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
                if (startX < 0 || startX + 48 > BattleDef.columnGridNum || startY < 0 || startY + 48 > BattleDef.rowGridNum) return false;
                for (int x = startX; x < startX + 48;++x){
                    for (int y = startY; y < startY + 48;++y){
                        if (grids[x, y] >0) return false;
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

        public int CreateStructure(int maxX,int maxY,int size,int radius=-1){
            structureUid = structureUid + 1;
            List<Vector2> structGrids = new List<Vector2>();
            for (int x = maxX - size; x < maxX;++x){
                for (int y = maxY - size; y < maxY;++y){
                    structureGrids[x, y] = true;
                    structGrids.Add(new Vector2(x,y));
                }
            }
            structureMap.Add(structureUid, structGrids);
            //mark cannot move
            if (radius != -1)
            {
                int centerX = maxX * 16 - (int)Mathf.Floor(size * 16 / 2);
                int centerY = maxY * 16 - (int)Mathf.Floor(size * 16 / 2);
                MarkMovable(centerX, centerY, radius, true);
            }

            return structureUid;
        }


        };
    }
