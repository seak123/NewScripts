using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PowerInject;
using DG.Tweening;

[Insert]
public class CamaraManager : MonoBehaviour {

    private readonly bool useMouse = BattleDef.useMouse;
    private Vector2 lastSingleTouchPosition;
    private CamaraManager camareMng;

    private MoveCameraStateFSM state;

    public GameObject cameraObj;
    //摄像机View Size
    public float viewField = 60f;
    //缩放系数
    public float scaleFactor = 1f;

    //滚动系数
    public float scrollFactor = 0.1f;


    public float maxSize = 80;
    public float minSize = 20;

    public bool active = false;
    public bool closing = false;
    public bool closeIn = false;
    private readonly float closingTime = 0.6f;
    private readonly float closingView = 30;

    private Vector3 closingPos;
    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势
    private Vector2 oldPosition1;
    private Vector2 oldPosition2;


    private Vector3 m_CameraOffset;
    public Camera m_Camera;


    //定义摄像机可以活动的范围
    public float xMin = -100;
    public float xMax = 100;
    public float zMin = -100;
    public float zMax = 100;
    public float topLevelValue = 30;
    public float floorLevelValue = -30;
    public float rightLevelValue = 30;
    public float leftLevelValue = -30;

    //更新UI
    public Action UpdateUI;

    //这个变量用来记录单指双指的变换
    private bool m_IsSingleFinger;
    private Vector3 speed;
    public bool touchLeaving = false;

    //初始化游戏信息设置
    void Start()
    {
        
    }

    public float GetViewSize(){
        return 60f/viewField;
    }

    [OnInjected]
    public void AddRootAction()
    {
        GameRoot.moduleInit += Init;
        GameRoot.BattleStartAction += StartBattle;
        GameRoot.BattleEndAction += CleanUp;
    }


    public void Init()
    {
        Debug.Log("CameraManager Init");
    }

    public void StartBattle(){
        state = MoveCameraStateFSM.Idle;
        camareMng = gameObject.GetComponent<CamaraManager>();
        //lastSingleTouchPosition = useMouse ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
        m_Camera = cameraObj.GetComponent<Camera>();
        m_CameraOffset = new Vector3(16.65f, 29.88f, 20f);
        xMin = 1;
        xMax = 23;
        zMin = 12.5f;
        zMax = 27.8f;
        viewField = 60f;
        maxSize = 80f;
        minSize = 40f;
        scaleFactor = 0.5f;
        scrollFactor = 0.45f;
        active = true;
    }

    public void CleanUp(){
        active = false;
    }

    public void MoveClose(Vector2 pos){
        active = false;
        closing = true;
        closeIn = true;
        closingPos = new Vector3(pos.x - 13, 29.88f, pos.y);
        m_Camera.transform.DOMove(closingPos, closingTime).onComplete += () => { closing = false; m_Camera.fieldOfView = closingView; };
    }

    public void MoveRecover(DungeonUI ui){
        closeIn = false;
        closing = true;
        m_Camera.transform.DOMove(m_CameraOffset,closingTime).onComplete += () => { active = true; closing = false; m_Camera.fieldOfView = viewField; ui.state = DungeonUIState.Idle; speed = Vector3.zero; };
    }
   
    /// <summary>
    /// 触摸缩放摄像头
    /// </summary>
    public void ScaleCamera(float currentTouchDistance, float lastTouchDistance)
    {
        ////计算出当前两点触摸点的位置
        //var tempPosition1 = Input.GetTouch(0).position;
        //var tempPosition2 = Input.GetTouch(1).position;


        //float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
        //float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);

        //计算上次和这次双指触摸之间的距离差距
        //然后去更改摄像机的距离
        //size -= (currentTouchDistance - lastTouchDistance) * scaleFactor * Time.deltaTime;


        ////把距离限制住在min和max之间
        //size = Mathf.Clamp(size, minSize, maxSize);


        ////备份上一次触摸点的位置，用于对比
        //oldPosition1 = tempPosition1;
        //oldPosition2 = tempPosition2;
        viewField -= (currentTouchDistance - lastTouchDistance) * scaleFactor*Time.deltaTime*3f;
    }




    public void MoveCameraDirect(Vector3 delta){
        Vector3 v = delta * 0.1f;
        m_CameraOffset += new Vector3(v.x, 0, v.z) * m_Camera.transform.position.y;
    }


    public void MoveCamera(Vector3 delta)
    {
        //Vector3 lastTouchPostion = m_Camera.ScreenToWorldPoint(new Vector3(lastSingleTouchPosition.x, lastSingleTouchPosition.y, -1));
        //Vector3 currentTouchPosition = m_Camera.ScreenToWorldPoint(new Vector3(scenePos.x, scenePos.y, -1));
        //Debug.Log(delta);
        Vector3 v = delta*scrollFactor;
        //if(v!=Vector3.zero) Debug.Log(v);
        speed = delta / Time.deltaTime;
        m_CameraOffset += new Vector3(-v.x*0.8f, 0, -v.z) * m_Camera.transform.position.y;


        //把摄像机的位置控制在范围内lastSingleTouchPosition
        m_CameraOffset = new Vector3(Mathf.Clamp(m_CameraOffset.x, xMin, xMax), m_CameraOffset.y, Mathf.Clamp(m_CameraOffset.z, zMin, zMax));
    }

    private void Update()
    {
        if (active)
        {
            if (Input.touchCount == 1)
            {
                camareMng.touchLeaving = false;
                if (state == MoveCameraStateFSM.Idle || state == MoveCameraStateFSM.Double)
                {
                    //在开始触摸或者从两字手指放开回来的时候记录一下触摸的位置
                    lastSingleTouchPosition = Input.GetTouch(0).position;
                }
                if (state == MoveCameraStateFSM.Single && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector3 lastTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(lastSingleTouchPosition.x, lastSingleTouchPosition.y, -1));
                    float deltaY = Input.GetTouch(0).position.y - lastSingleTouchPosition.y;
                    Vector3 currTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, lastSingleTouchPosition.y + deltaY * 1.41f, -1));
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
                if (state == MoveCameraStateFSM.Idle || state == MoveCameraStateFSM.Single)
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
            else if (Input.touchCount == 0 && !useMouse)
            {
                camareMng.touchLeaving = true;
                state = MoveCameraStateFSM.Idle;
                return;
            }


            //用鼠标的
            if (useMouse)
            {
                camareMng.viewField -= Input.GetAxis("Mouse ScrollWheel") * camareMng.scaleFactor;
                camareMng.viewField = Mathf.Clamp(camareMng.viewField, camareMng.minSize, camareMng.maxSize);
                if (Input.GetMouseButtonDown(0))
                {
                    camareMng.touchLeaving = false;
                    lastSingleTouchPosition = Input.mousePosition;
                    //Debug.Log("GetMouseButtonDown:" + lastSingleTouchPosition);
                }
                if (Input.GetMouseButton(0))
                {

                    Vector3 lastTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(lastSingleTouchPosition.x, lastSingleTouchPosition.y, -1));
                    float deltaY = Input.mousePosition.y - lastSingleTouchPosition.y;
                    Vector3 currTouchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, lastSingleTouchPosition.y + deltaY * 1.4f, -1));

                    Vector3 delta = lastTouchPos - currTouchPos;
                    camareMng.MoveCamera(delta);
                    lastSingleTouchPosition = Input.mousePosition;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    camareMng.touchLeaving = true;
                    return;
                }
            }

            return;
        }

    }
    //Update方法一旦调用结束以后进入这里算出重置摄像机的位置
    private void LateUpdate()
    {
        if (active)
        {
            if (touchLeaving == true && speed.magnitude >= 0.1f)
            {
                MoveCamera(speed * Time.deltaTime);
                speed = speed / 1.3f;
            }

            var position = m_CameraOffset;
            m_Camera.transform.position = position;
            viewField = Mathf.Clamp(viewField, camareMng.minSize, camareMng.maxSize);
            m_Camera.fieldOfView = viewField;
            if (UpdateUI != null)
            {
                UpdateUI();
            }
        }
        if(closing){
            if(closeIn){
                m_Camera.fieldOfView -= (viewField-closingView) / closingTime*Time.deltaTime;
                m_Camera.fieldOfView = Mathf.Max(m_Camera.fieldOfView, closingView);
            }else{
                m_Camera.fieldOfView += (viewField-closingView) / closingTime*Time.deltaTime;
                m_Camera.fieldOfView = Mathf.Min(m_Camera.fieldOfView, viewField);
            }
        }
    }
}
