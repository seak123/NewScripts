using UnityEngine;
using System.Collections;
using System;
using PowerInject;

public enum GameState{
    IdleState = 1,
    SelectPosState = 2,
    MoveCameraState = 3
};

[Insert]
public class GameStateManager : MonoBehaviour
{
    private FsmState currState;
    private Dictionary<GameState,FsmState> allStates;

    void Start()
    {
        allStates.Add(GameState.IdleState,IdleState.new());
        allStates.Add(GameState.SelectPosState,SelectPosState.new());
        allStates.Add(GameState.MoveCameraState,MoveCameraState.new());
        currState = allStates[GameState.IdleState];
        allStates[GameState.IdleState].OnEnter();
    }
    [OnInjected]
    void AddRootAction(){
        GameRoot.init += Init;
    }
    public void Init(){

    }
    private void Update()
    {
        GameState stateIndex = currState.OnUpdate();
        if(stateIndex>=0){
            Run(allStates[stateIndex]);
        }
    }
    private void Run(FsmState nextState){
        currState.OnLeave();
        currState = nextState;
        nextState.OnEnter();
    }

};
