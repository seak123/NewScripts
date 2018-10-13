using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using PowerInject;
using Data;

[Insert]
    public class LuaBridge : MonoBehaviour
    {

        private LuaState luaState;
        private LuaFunction luaInit;
        private LuaFunction luaUpdate;

        private LuaTable luaRoot;

        // Use this for initialization
        void Start()
        {
            if (luaState == null)
            {
                luaState = new LuaState();
                luaState.Start();
                luaState.LuaCheckStack(200);
                luaState.CheckTop();
                LuaBinder.Bind(luaState);
                luaState.AddSearchPath(Application.dataPath + "\\Scripts/lua");
                luaState.DoFile("lua_bridge.lua");
                luaInit = luaState.GetFunction("lua_init");
                luaUpdate = luaState.GetFunction("lua_update");

                int _ref = luaInit.BeginPCall();
                luaInit.PCall();
                object[] tb = luaState.CheckObjects(_ref);
                luaInit.EndPCall();

                luaRoot = tb[0] as LuaTable;
            }
        Init();

    }
       
        public void Init()
        {
            GameRoot.GetInstance().Schedular.onUpdate += OnUpdate;
        }

        public void OnUpdate(float delta){
            luaUpdate.BeginPCall();
            luaUpdate.Push(delta);
            luaUpdate.PCall();
            luaUpdate.EndPCall();
        }

    public void AddUnit(UnitData data){
        LuaFunction func = luaState.GetFunction("add_unit");
        func.BeginPCall();
        func.Push(data);
        func.PCall();
        func.EndPCall();

    }



}
