using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Lua2CSharpAPI
    {

        public static void AddUnit(int id, float x, float y)
        {
            GameRoot.GetInstance().BattleField.AddUnit(id, x, y);
        }

    }
}
