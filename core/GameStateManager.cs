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

[Insert]
public class GameStateManager : MonoBehaviour
{
    private FsmState currState;
    private Dictionary<GameState,FsmState> allStates;

    //private bool isNeedHelp = false;
    private bool isInited = false;

    public CardEntity selectCard;

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

    }
    [OnInjected]
    void AddRootAction(){
        GameRoot.init += Init;
        GameRoot.clean += CleanUp;
    }
    public void Init(){

            currState = allStates[GameState.EnterState];
            allStates[GameState.EnterState].OnEnter();

        isInited = true;
    }
    public void CleanUp(){
        isInited = false;
    }
    private void Update()
    {
        if (isInited)
        {
            GameState stateIndex = currState.OnUpdate();
            if (stateIndex >= 0)
            {
                Run(allStates[stateIndex]);
            }
        }
    }
    private void Run(FsmState nextState){
        currState.OnLeave();
        currState = nextState;
        nextState.OnEnter();
    }

};
