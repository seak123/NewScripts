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
        private readonly float Transfer2GridFactor = 25;
        private readonly float DiagoFactor = (float)Math.Sqrt(2);
        //private float centerOffset = 0.365f;

        private AssetManager mng;

        private void Start()
        {

        }

        public Entity CreateEntity(int id,float x,float y){
            GameObject obj = Instantiate(mng.GetCreatureData(id).prefab, new Vector3(x, y, 0), Quaternion.identity);
            return obj.AddComponent<Entity>();
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

        public void GetLogicPos(int grid_x,int grid_y,out float pos_x,out float pos_y){
            pos_x = grid_x/Transfer2GridFactor;
            pos_y = grid_y/ Transfer2GridFactor;
        }

        public void TryMove(int unit_id,int s_x,int s_y,int e_x,int e_y,float value,out bool canMove,out int nextX,out int nextY,out float offset,out bool completed){
            int radius = mng.GetCreatureData(unit_id).radius;

            //init
            for (int x = Math.Max(0, s_x - radius); x < Math.Min(BattleDef.columnGridNum, s_x + radius); ++x)
            {
                for (int y = Math.Max(0, s_y - radius); y < Math.Min(BattleDef.rowGridNum, s_y + radius); ++y)
                {
                    grids[x, y] = false;
                }
            }
            //completed
            if(Distance(s_x,s_y,e_x,e_y)<=value){
                if(IsCanMove(e_x,e_y,radius)){
                    canMove = true;
                    nextX = e_x;
                    nextY = e_y;
                    offset = 0;
                    completed = true;
                }else{
                    canMove = false;
                    nextX = s_x;
                    nextY = s_y;
                    offset = 0;
                    completed = true;
                }
                return;
            }
            canMove = true;
            nextX = 0;
            nextY = 0;
            offset = 0;
            completed = false;
        }

        private float Distance(int s_x,int s_y,int e_x,int e_y){
            int dis_x = Math.Abs(e_x - s_x);
            int dis_y = Math.Abs(e_y - s_y);
            int length = Math.Max(dis_x, dis_y);
            int width = Math.Min(dis_x, dis_y);
            return width * DiagoFactor + length - width;
        }

        private bool IsCanMove(int grid_x,int grid_y,int radius){
            for (int x = Math.Max(0, grid_x - radius); x < Math.Min(BattleDef.columnGridNum, grid_x + radius);++x){
                for (int y = Math.Max(0, grid_y - radius); y < Math.Min(BattleDef.rowGridNum, grid_y + radius);++y){
                    if (grids[x, y] == true) return false;
                }
            }
            return true;
        }
    }
}
