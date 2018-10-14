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

        public Vector2 GetGridPosition(int r,int c){

            float x = (c - BattleDef.columnGridNum / 2)/Transfer2GridFactor;
            float y = (r - BattleDef.rowGridNum / 2) / Transfer2GridFactor+ centerOffset;

            return new Vector2(x, y);
        }

        //public bool CheckPosionAvalible(float x,float y){
          //  
        //}
    }
}
