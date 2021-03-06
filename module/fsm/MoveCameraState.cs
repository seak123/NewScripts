﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveCameraStateFSM{
    Idle,
    Single,
    Double
}

public class MoveCameraState : FsmState
{
    private readonly bool useMouse = BattleDef.useMouse;
    private bool m_IsSingleFinger;
    private Vector2 lastSingleTouchPosition;
    private CamaraManager camareMng;
    private Vector2 oldPosition1;
    private Vector2 oldPosition2;

    private MoveCameraStateFSM state;

    public MoveCameraState(){
        stateType = GameState.MoveCameraState;
    }
    public override void OnEnter()
    {
        Debug.Log("Enter MoveCameraState");
        camareMng = GameRoot.GetInstance().CameraMng;
        state = MoveCameraStateFSM.Idle;
        lastSingleTouchPosition = useMouse ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
    }

    public override void OnLeave()
    {

    }

    public override GameState OnUpdate()
    {
        //if(BattleDef.useMouse){
        //    if (Input.GetMouseButtonUp(0)) return GameState.IdleState;
        //}
        //return GameState.KeepRunning;

        //判断触摸数量为单点触摸
        if (Input.touchCount == 1)
        {
            camareMng.touchLeaving = false;
            if (state == MoveCameraStateFSM.Idle|| state == MoveCameraStateFSM.Double)
            {
                //在开始触摸或者从两字手指放开回来的时候记录一下触摸的位置
                lastSingleTouchPosition = Input.GetTouch(0).position;
            }
            if (state == MoveCameraStateFSM.Single && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector3 lastTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(lastSingleTouchPosition.x, lastSingleTouchPosition.y, -1));
                float deltaY = Input.GetTouch(0).position.y - lastSingleTouchPosition.y;
                Vector3 currTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, lastSingleTouchPosition.y+deltaY*1.41f, -1));
                Vector3 delta = lastTouchPos - currTouchPos;
                camareMng.MoveCamera(delta);
                lastSingleTouchPosition = Input.GetTouch(0).position;
            }
            //m_IsSingleFinger = true;
            state = MoveCameraStateFSM.Single;
        }
        else if (Input.touchCount > 1)
        {
            camareMng.touchLeaving = false;
            //当从单指触摸进入多指触摸的时候,记录一下触摸的位置
            //保证计算缩放都是从两指手指触碰开始的
            if (state == MoveCameraStateFSM.Idle||state == MoveCameraStateFSM.Single)
            {
                oldPosition1 = Input.GetTouch(0).position;
                oldPosition2 = Input.GetTouch(1).position;
            }

            if (state == MoveCameraStateFSM.Double && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
            {
                var tempPosition1 = Input.GetTouch(0).position;
                var tempPosition2 = Input.GetTouch(1).position;


                float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
                float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);

                camareMng.ScaleCamera(currentTouchDistance, lastTouchDistance);


                oldPosition1 = tempPosition1;
                oldPosition2 = tempPosition2;
                camareMng.MoveCamera(Vector3.zero);
            }

            //m_IsSingleFinger = false;
            state = MoveCameraStateFSM.Double;
        }
        else if(Input.touchCount == 0 && !useMouse){
            camareMng.touchLeaving = true;
            state = MoveCameraStateFSM.Idle;
            return GameState.IdleState;
        }


        //用鼠标的
        if (useMouse)
        {
            //camareMng.size -= Input.GetAxis("Mouse ScrollWheel") * camareMng.scaleFactor;
            //camareMng.size = Mathf.Clamp(camareMng.size, camareMng.minSize, camareMng.maxSize);
            //if (Input.GetMouseButtonDown(0))
            //{
            //    camareMng.touchLeaving = false;
            //    lastSingleTouchPosition = Input.mousePosition;
            //    //Debug.Log("GetMouseButtonDown:" + lastSingleTouchPosition);
            //}
            //if (Input.GetMouseButton(0))
            //{
               
            //    Vector3 lastTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(lastSingleTouchPosition.x, lastSingleTouchPosition.y, -1));
            //    float deltaY = Input.mousePosition.y - lastSingleTouchPosition.y;
            //    Vector3 currTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, lastSingleTouchPosition.y+deltaY*1.4f, -1));

            //    Vector3 delta = lastTouchPos - currTouchPos;
            //    camareMng.MoveCamera(delta);
            //    lastSingleTouchPosition = Input.mousePosition;
            //}
            //if (Input.GetMouseButtonUp(0)){
            //    camareMng.touchLeaving = true;
            //    return GameState.IdleState;
            //}
        }

        return GameState.KeepRunning;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
