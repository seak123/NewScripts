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
    }
}
