using UnityEngine;
using System.Collections;
using System;

public class CameraFollow : MonoBehaviour
{
    
    public Transform Watchtarget;
    public Transform CenterTarget;

    public float x;
    public float y;
    public float PrevX;
    public float PrevY;
    public float xSpeed;
    public float ySpeed;

    public float distence;
    public float disSpeed;

    public float minDistence;
    public float maxDistence;


    Quaternion rotationEuler;
    Vector3 cameraPosition;
    void Start()
    {
        
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        // Early out if we don't have a Watchtarget
        if (!Watchtarget)
            return;

        var xlen = Input.mousePosition.x;
        var ylen = Input.mousePosition.y;
        if (Input.GetMouseButton(1))
        {

            x -= 360 * ( (PrevX - xlen) / (float)Screen.width);
            y += 360 * ((PrevY - ylen) / (float)Screen.height);
            
            x = x % 360;
            y = y % 360;
        }

        PrevX = xlen;
        PrevY = ylen;


        distence -= Input.GetAxis("Mouse ScrollWheel") * disSpeed * Time.deltaTime;

        distence = Mathf.Clamp(distence , minDistence, maxDistence);

        rotationEuler = Quaternion.Euler(y, x, 0);
        cameraPosition = rotationEuler * new Vector3(0, 0, -distence) + CenterTarget.position;

        transform.rotation = rotationEuler;
        transform.position = cameraPosition;

        //这里是计算射线的方向，从主角发射方向是射线机方向
        Vector3 aim = cameraPosition;
        //得到方向
        Vector3 ve = (CenterTarget.position - cameraPosition).normalized;
        float an = rotationEuler.y;
        aim -= an * ve;
        //在场景视图中可以看到这条射线
        Debug.DrawLine(CenterTarget.position, aim, Color.red);
        //主角朝着这个方向发射射线
        RaycastHit hit;
        if (Physics.Linecast(CenterTarget.position, aim, out hit))
        {
            
            
            string name = hit.collider.gameObject.tag;            
            if (name != "MainCamera" && name != "Actor")
            {
                //当碰撞的不是摄像机也不是地形 那么直接移动摄像机的坐标
                transform.position = hit.point;

            }
        }
       
        // 让射线机永远看着主角
        transform.LookAt(Watchtarget);

    }

    
}




