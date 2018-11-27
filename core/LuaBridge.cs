using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using PowerInject;
using Data;

[Insert]
public class LuaBridge : LuaClient
    {

        //private LuaState luaState;
        private LuaFunction luaInit;
        private LuaFunction startBattle;
        private LuaFunction luaUpdate;

        private LuaTable luaRoot;
        private LuaEngine luaEngine;

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

   
    // Use this for initialization
    void Start()
        {

        if (luaState != null)
            {
                //luaState = new LuaState();
                //luaState.Start();
                luaState.LuaCheckStack(200);
                //luaState.CheckTop();
                //LuaBinder.Bind(luaState);
                //luaState.AddSearchPath(Application.dataPath + "\\Scripts/lua");
                //luaState.DoFile("lua_bridge.lua");
                luaInit = luaState.GetFunction("lua_init");
                startBattle = luaState.GetFunction("start_battle");
                luaUpdate = luaState.GetFunction("lua_update");

                int _ref = luaInit.BeginPCall();
                luaInit.PCall();
                object[] tb = luaState.CheckObjects(_ref);
                luaInit.EndPCall();

                luaRoot = tb[0] as LuaTable;
            }

    }

    [OnInjected]
    public void AddRootAction(){
        GameRoot.init += OnInit;
    }
       
        public void OnInit()
        {
            GameRoot.GetInstance().Schedular.onUpdate += OnUpdate;
        }

        public void OnUpdate(float delta){
            luaUpdate.BeginPCall();
            luaUpdate.Push(delta);
            luaUpdate.PCall();
            luaUpdate.EndPCall();
        }
    public void StartBattle(BattleData data){
        startBattle.BeginPCall();
        startBattle.Push(data);
        startBattle.PCall();
        startBattle.EndPCall();
    }

    public void CasterSkill(int side,int skill_id,int pos_x,int pos_y,UnitData data =null,int num = 0){
        LuaFunction func = luaState.GetFunction("caster_skill");
        func.BeginPCall();
        func.Push(side);
        func.Push(skill_id);
        func.Push(pos_x);
        func.Push(pos_y);
        func.Push(data);
        func.Push(num);
        func.PCall();
        func.EndPCall();

    }



}
