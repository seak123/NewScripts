using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

public class LuaEngine : LuaClient {

    protected override LuaFileUtils InitLoader()
    {
        return new LuaResLoader();
    }

    protected override void CallMain()
    {
        //LuaFunction func = luaState.GetFunction("Test");
        //func.Call();
        //func.Dispose();
    }

    protected override void StartMain()
    {
        luaState.DoFile("lua_bridge.lua");
        //CallMain();
    }
}
