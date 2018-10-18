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
        private float Transfer2GridFactor = 116;
        private float centerOffset = 0.365f;

        private void Start()
        {

        }

        public Entity CreateEntity(int id,float x,float y){
            AssetManager mng = GameRoot.GetInstance().BattleField.assetManager;
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
            grids = new bool[BattleDef.rowGridNum,BattleDef.columnGridNum];
            for (int i = 0; i < BattleDef.rowGridNum;++i){
                for (int j = 0; j < BattleDef.columnGridNum;++j){
                    grids[i,j] = false;
                }
            }

        }

        public void GetGridPos(float x,float y,out int grid_x,out int grid_y){
            grid_x = (int)Math.Floor(x * Transfer2GridFactor + BattleDef.columnGridNum / 2);
            grid_y = (int)Math.Floor((y - centerOffset) * Transfer2GridFactor + BattleDef.rowGridNum / 2);
        }

        public void GetLogicPos(int grid_x,int grid_y,out float pos_x,out float pos_y){
            pos_x = (grid_x - BattleDef.columnGridNum / 2)/Transfer2GridFactor;
            pos_y = (grid_y - BattleDef.rowGridNum / 2) / Transfer2GridFactor+ centerOffset;
        }

        public void TryMove(int unit_id,int s_x,int s_y,int e_x,int e_y,float value,out bool canMove,out int nextX,out int nextY,out float offset){
            //canMove = true;
            //nextX = (int)Math.Floor(s_x + value);
            //nextY = s_y;
            //offset = value-(float)(nextX-s_x);
        }


        //public Vector2 GetGridPosition(int r,int c){
        //
         //   float x = (c - BattleDef.columnGridNum / 2)/Transfer2GridFactor;
          //  float y = (r - BattleDef.rowGridNum / 2) / Transfer2GridFactor+ centerOffset;
        //
         //   return new Vector2(x, y);
        //}

        //public bool CheckPosionAvalible(float x,float y){
          //  
        //}
    }
}
