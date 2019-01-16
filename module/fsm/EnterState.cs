using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterState : FsmState
{
    private float enterTime = -1;
    private CamaraManager cameraMng;
    private BattleUIManager uiMng;

    public EnterState(){
        stateType = GameState.EnterState;
    }

    private bool battleIsStart = false;

    public override void OnEnter()
    {
        GameRoot.BattleStartAction += BattleEnter;
        GameRoot.BattleStartDelayAction += BattleStart;
        cameraMng = GameRoot.GetInstance().Camara.GetComponent<CamaraManager>();
        uiMng = GameRoot.GetInstance().battleUI.GetComponent<BattleUIManager>();
    }

    public override void OnLeave()
    {
        enterTime = -1;
        //GameRoot.BattleStartAction -= BattleEnter;
        //GameRoot.BattleStartDelayAction -= BattleStart;
    }

    public override GameState OnUpdate()
    {
        if (enterTime >= 0)
        {
            enterTime += Time.deltaTime;
            BattleStartAnim();
        }



        return battleIsStart == true ? GameState.IdleState : GameState.KeepRunning;

    }

    private void BattleEnter(){
        GameRoot.GetInstance().battleUI.transform.Find("StartPanal").gameObject.SetActive(false);
        enterTime = 0;
        uiMng.talkPanel.GetComponent<TalkUIManager>().StartAnim();
    }

    private void BattleStart(){
        battleIsStart = true;
    }

    private void BattleStartAnim(){
        float wholeDelay = GameRoot.GetInstance().GetBattleEnterDelay();

        if (enterTime <= wholeDelay - 3f) return;
        if(enterTime<=wholeDelay-2f){
            float startDelta = enterTime - wholeDelay + 3f;
            float delta = (startDelta - 0.2f) / 0.6f * 6f;
            cameraMng.size = Mathf.Clamp(2f + delta,2f,8);
        }else if(enterTime<=wholeDelay - 1f){
            cameraMng.MoveCameraDirect(new Vector3(-Time.deltaTime*(BattleDef.columnGridNum-BattleDef.StructBound*16*1.5f)/25, 0, -Time.deltaTime * BattleDef.rowGridNum / 4 / 25));
        }else{
            float startDelta = enterTime - wholeDelay + 1f;
            float delta = (startDelta - 0.2f) / 0.6f * 1.6f;
            cameraMng.size = Mathf.Clamp(8 - delta,6.4f,8);
        }
    }
}
