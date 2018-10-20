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
            float wholeDistance = Distance(s_x,s_y,e_x,e_y);

            //init
            for (int x = Math.Max(0, s_x - radius); x < Math.Min(BattleDef.columnGridNum, s_x + radius); ++x)
            {
                for (int y = Math.Max(0, s_y - radius); y < Math.Min(BattleDef.rowGridNum, s_y + radius); ++y)
                {
                    grids[x, y] = false;
                }
            }

            //completed
            if (wholeDistance <= value)
            {
                if (IsCanMove(e_x, e_y, radius))
                {
                    canMove = true;
                    nextX = e_x;
                    nextY = e_y;
                    offset = 0;
                    completed = true;
                }
                else
                {
                    canMove = false;
                    nextX = s_x;
                    nextY = s_y;
                    offset = 0;
                    completed = true;
                }
                return;
            }
            //try move
            int keyX = 0;
            int keyY = 0;
            int interval = (int)Math.Floor((value * Transfer2GridFactor));
            keyX = (int)Math.Floor((e_x - s_x) / wholeDistance * interval) + s_x;
            keyY = (int)Math.Floor((e_y - s_y) / wholeDistance * interval) + s_y;
    
            for (int i = 0; i <= 2 * interval;++i){

                int delta_X1 = Mod(keyX+i-s_x,interval);
                int temX1 = s_x+delta_X1;
                int delta_Y1 = (int)Math.Floor(Math.Sqrt(interval * interval - delta_X1 * delta_X1));
                int flag1 = keyY > s_y ? 1 : -1;
                delta_Y1 = Math.Abs(keyX+i-s_x) > interval ? -flag1 * delta_Y1 : flag1 * delta_Y1;
                int temY1 = s_y + delta_Y1;
                float temDistance1 = Distance(temX1, temY1, e_x, e_y);
                bool canMove1 = IsCanMove(temX1, temY1, radius);

                int delta_X2 = Mod(keyX - i - s_x, interval);
                int temX2 = s_x + delta_X2;
                int delta_Y2 = (int)Math.Floor(Math.Sqrt(interval * interval - delta_X2 * delta_X2));
                int flag2 = keyY > s_y ? 1 : -1;
                delta_Y2 = Math.Abs(keyX + i - s_x) > interval ? -flag2 * delta_Y2 : flag2 * delta_Y2;
                int temY2 = s_y + delta_Y2;
                float temDistance2 = Distance(temX2, temY2, e_x, e_y);
                bool canMove2 = IsCanMove(temX2, temY2, radius);

                if(canMove1&&canMove2){
                    if(temDistance1<temDistance2){
                        canMove = true;
                        nextX = temX1;
                        nextY = temY1;
                        offset = value-Distance(s_x, s_y, nextX, nextY);
                        completed = false;
                        MarkCannotMove(nextX, nextY, radius);
                        return;
                    }
                    else{
                        canMove = true;
                        nextX = temX2;
                        nextY = temY2;
                        offset = value - Distance(s_x, s_y, nextX, nextY);
                        completed = false;
                        MarkCannotMove(nextX, nextY, radius);
                        return;
                    }
                }else if(canMove1){
                    canMove = true;
                    nextX = temX1;
                    nextY = temY1;
                    offset = value - Distance(s_x, s_y, nextX, nextY);
                    completed = false;
                    MarkCannotMove(nextX, nextY, radius);
                    return;
                }
                else if(canMove2){
                    canMove = true;
                    nextX = temX2;
                    nextY = temY2;
                    offset = value - Distance(s_x, s_y, nextX, nextY);
                    completed = false;
                    MarkCannotMove(nextX, nextY, radius);
                    return;
                }

            }


            canMove = false;
            nextX = s_x;
            nextY = s_y;
            offset = 0;
            completed = false;
            MarkCannotMove(nextX, nextY, radius);
            return;
        }

        private int Mod(int value,int round){
            int flag = value > 0 ? 1:-1;
            value = Math.Abs(value);
            if (value > round) value = 2 * round - value;
            return value * flag;
        }

        private float Distance(int s_x,int s_y,int e_x,int e_y){
            float dis_x = Math.Abs(e_x - s_x);
            float dis_y = Math.Abs(e_y - s_y);
            return (float)Math.Sqrt(dis_x * dis_x + dis_y * dis_y);
        }

        private bool IsCanMove(int grid_x,int grid_y,int radius){
            if (grid_x < 0 || grid_x >= BattleDef.columnGridNum || grid_y < 0 || grid_y >= BattleDef.rowGridNum) return false;
            for (int x = Math.Max(0, grid_x - radius); x < Math.Min(BattleDef.columnGridNum, grid_x + radius);++x){
                for (int y = Math.Max(0, grid_y - radius); y < Math.Min(BattleDef.rowGridNum, grid_y + radius);++y){
                    if (grids[x, y] == true) return false;
                }
            }
            return true;
        }

        private void MarkCannotMove(int grid_x,int grid_y,int radius){
            for (int x = Math.Max(0, grid_x - radius); x < Math.Min(BattleDef.columnGridNum, grid_x + radius); ++x)
            {
                for (int y = Math.Max(0, grid_y - radius); y < Math.Min(BattleDef.rowGridNum, grid_y + radius); ++y)
                {
                    grids[x, y] = true;
                }
            }
        }
    }
}
