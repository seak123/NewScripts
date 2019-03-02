using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CamaraManager : MonoBehaviour {

    //摄像机View Size
    public float size = 6.4f;
    //缩放系数
    public float scaleFactor = 0;

    //滚动系数
    public float scrollFactor = 0.1f;


    public float maxSize = 12.8f;
    public float minSize = 3.2f;


    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势
    private Vector2 oldPosition1;
    private Vector2 oldPosition2;


    public Vector2 lastSingleTouchPosition;

    private Vector3 m_CameraOffset;
    public Camera m_Camera;

    public bool useMouse = BattleDef.useMouse;

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
        m_Camera = this.GetComponent<Camera>();
        m_CameraOffset = m_Camera.transform.position;
    }


    public void Init()
    {
        m_CameraOffset = new Vector3(-1.97f, 19.94f, 5.8f);
        //size = 16f;
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
        size -= (currentTouchDistance - lastTouchDistance) * scaleFactor * Time.deltaTime;


        //把距离限制住在min和max之间
        size = Mathf.Clamp(size, minSize, maxSize);


        ////备份上一次触摸点的位置，用于对比
        //oldPosition1 = tempPosition1;
        //oldPosition2 = tempPosition2;
    }


    //Update方法一旦调用结束以后进入这里算出重置摄像机的位置
    private void LateUpdate()
    {
        if(touchLeaving==true&&speed.magnitude>=0.1f){
            MoveCamera(speed * Time.deltaTime);
            speed = speed / 1.3f;
        }

        var position = m_CameraOffset;
        m_Camera.transform.position = position;
        m_Camera.orthographicSize = size;
        if (UpdateUI != null)
        {
            UpdateUI();
        }
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
        m_CameraOffset += new Vector3(v.x, 0, v.z) * m_Camera.transform.position.y;

        //Debug.Log(lastTouchPostion + "|" + currentTouchPosition + "|" + v);
        //lastSingleTouchPosition = scenePos;
        float offset = (6.4f / size - 1) / Mathf.Sqrt(2) * 16;
      
        if ((m_CameraOffset.x + m_CameraOffset.z) > topLevelValue+offset)
        {
            float cut = m_CameraOffset.x + m_CameraOffset.z - topLevelValue-offset;
            m_CameraOffset.x -= cut / 2;
            m_CameraOffset.z -= cut / 2;
        }

        if ((m_CameraOffset.x + m_CameraOffset.z) < floorLevelValue - offset)
        {
            float cut = floorLevelValue - offset - (m_CameraOffset.x + m_CameraOffset.z) ;
            m_CameraOffset.x += cut / 2;
            m_CameraOffset.z += cut / 2;
        }

        if ((m_CameraOffset.x - m_CameraOffset.z) > rightLevelValue + offset)
        {
            float cut = (m_CameraOffset.x - m_CameraOffset.z)- rightLevelValue-offset;
            m_CameraOffset.x -= cut / 2;
            m_CameraOffset.z += cut / 2;
        }

        if ((m_CameraOffset.x - m_CameraOffset.z) < leftLevelValue - offset)
        {
            float cut = -(m_CameraOffset.x - m_CameraOffset.z) + leftLevelValue-offset;
            m_CameraOffset.x += cut / 2;
            m_CameraOffset.z -= cut / 2;
        }

        //把摄像机的位置控制在范围内lastSingleTouchPosition
        m_CameraOffset = new Vector3(Mathf.Clamp(m_CameraOffset.x, xMin, xMax), m_CameraOffset.y, Mathf.Clamp(m_CameraOffset.z, zMin, zMax));
    }

}
