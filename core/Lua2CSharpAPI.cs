using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

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

        public static void BattleCompleted(){
            Debug.Log("Game is Over");
        }
    }
}
