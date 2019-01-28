using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;
using Data;

namespace Utils
{
    public static class Lua2CSharpAPI
    {
        public static MapField GetMapField(){
            return GameRoot.GetInstance().MapField;
        }

        public static EffectManager GetEffectManager(){
            return GameRoot.GetInstance().EffectMng;
        }

        public static PlayerManager GetPlayerManager(){
            return GameRoot.GetInstance().PlayerMng;
        }

        public static AssetManager GetAssetManager(){
            return GameRoot.GetInstance().BattleField.assetManager;
        }

        public static void BattleCompleted(int res){
            Debug.Log("Game is Over");
            GameRoot.GetInstance().CompleteBattle(res);
        }

        public static UnitData GetUnitData(int id){
            AssetManager mng;
            mng = GameRoot.GetInstance().BattleField.assetManager;
            return AssetManager.PackCreatureData(mng.GetCreatureData(id));
        }
    }
}
