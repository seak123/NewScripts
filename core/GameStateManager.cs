using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using PowerInject;

public enum GameState{
    KeepRunning = -1,
    EnterState = 0,
    IdleState = 1,
    SelectPosState = 2,
    MoveCameraState = 3,
    HelperState = 9
};

public enum BattleState{
    KeepRunning = -1,
    Waiting = 0,
    Caster = 1,
    Fight = 2
}

[Insert]
public class GameStateManager : MonoBehaviour
{
    private GameState currState;
    private BattleState currBattleState;
    private Dictionary<GameState,FsmState> allStates;
    private Dictionary<BattleState, BattleFsm> battleStates;

    //private bool isNeedHelp = false;
    private bool isInited = false;

    public CardEntity selectCard;

    public BattleState GetCurrBattleState(){
        return currBattleState;
    }

    void Start()
    {
        allStates = new Dictionary<GameState, FsmState>
        {
            { GameState.EnterState, new EnterState() },
            { GameState.IdleState, new IdleState() },
            { GameState.SelectPosState, new SelectPosState() },
            { GameState.MoveCameraState, new MoveCameraState() },
            { GameState.HelperState, new HelperState() }
        };
        battleStates = new Dictionary<BattleState, BattleFsm>
        {
            {BattleState.Waiting,new BattleWaitingState()},
            {BattleState.Caster,new BattleCasterState()},
            {BattleState.Fight,new BattleFightState()}
        };
        currState = GameState.KeepRunning;
        currBattleState = BattleState.KeepRunning;

    }
    [OnInjected]
    void AddRootAction(){
        GameRoot.moduleInit += Init;
        GameRoot.BattleEndAction += CleanUp;
    }
    public void Init(){

        Run(GameState.EnterState);
        RunBattle(BattleState.Waiting);
        isInited = true;
    }
    public void CleanUp(){
        isInited = false;
    }
    private void Update()
    {
        if (isInited)
        {
            GameState stateIndex = allStates[currState].OnUpdate();
            BattleState battleIndex = battleStates[currBattleState].OnUpdate();
            if (stateIndex >= 0)
            {
                Run(stateIndex);
            }
            if(battleIndex>=0){
                RunBattle(battleIndex);
            }
        }
    }
    private void Run(GameState nextState){
        if(currState>=0)
            allStates[currState].OnLeave();
        currState = nextState;
        allStates[nextState].OnEnter();
    }
    private void RunBattle(BattleState nextState){
        if (currBattleState >= 0)
            battleStates[currBattleState].OnLeave();
        currBattleState = nextState;
        battleStates[nextState].OnEnter();
    }

};
