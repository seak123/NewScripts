﻿using System.Collections;
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
        private LuaFunction startStrategy;
        private LuaFunction luaUpdate;
        private LuaFunction removeEntity;
        private LuaFunction addEntity;

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
                startStrategy = luaState.GetFunction("start_strategy");
                luaUpdate = luaState.GetFunction("lua_update");
                removeEntity = luaState.GetFunction("remove_entity");
                addEntity = luaState.GetFunction("add_entity");

                
            }

    }

    [OnInjected]
    public void AddRootAction(){
        GameRoot.moduleInit += OnInit;
        GameRoot.BattleStartAction += BattleStart;
        GameRoot.BattleEndAction += CleanUp;
    }
       
    public void OnInit()
    {
        int _ref = luaInit.BeginPCall();
        luaInit.PCall();
        object[] tb = luaState.CheckObjects(_ref);
        luaInit.EndPCall();

        luaRoot = tb[0] as LuaTable;
    }
    public void BattleStart(){
        GameRoot.GetInstance().Schedular.onUpdate += OnUpdate;
    }
    void CleanUp(){
        GameRoot.GetInstance().Schedular.onUpdate -= OnUpdate;
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

    public void StartStrategy(BattleData data){
        startStrategy.BeginPCall();
        startStrategy.Push(data);
        startStrategy.PCall();
        startStrategy.EndPCall();
    }

    public void CasterSkill(int side,int skill_id,int pos_x,int pos_y,UnitData data =null,int num = 0,int structId = -1){
        LuaFunction func = luaState.GetFunction("caster_skill");
        func.BeginPCall();
        func.Push(side);
        func.Push(skill_id);
        func.Push(pos_x);
        func.Push(pos_y);
        func.Push(data);
        func.Push(num);
        func.Push(structId);
        func.PCall();
        func.EndPCall();

    }

    public void RemoveEntity(int uid){
        removeEntity.BeginPCall();
        removeEntity.Push(uid);
        removeEntity.PCall();
        removeEntity.EndPCall();
    }

    public void AddEntity(UnitData data){
        addEntity.BeginPCall();
        addEntity.Push(data);
        addEntity.PCall();
        addEntity.EndPCall();
    }

    public int GetEnemyNum(){
        LuaFunction func = luaState.GetFunction("enemy_num");
        func.BeginPCall();
        func.PCall();
        double res = func.CheckNumber();
        func.EndPCall();

        int num = (int)res;

        return num;
    }



}
