using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCameraMoving : MonoBehaviour
{
    public Camera camera;
    float speed = 10f;
    public Transform target;    //카메라 회전할 중심 타갯

    float lastPos, firstPos;    //(확대축소)나중 위치, 시작 위치
    float lastXPos, firstXPos, lastYPos, firstYPos; //(회전)나중 위치, 시작 위치
    Vector3 cameraStartPos; //카메라 처음 시작 포지션

    void Start()
    {
        cameraStartPos = this.transform.position;   //카메라 처음 시작 포지션
        lastPos = TouchSystem.screenMoveRightShipPos.x; //나중 위치에 값이 없어 그냥 처음 위치 세팅
        lastXPos = TouchSystem.screenMoveRightShipPos.x;
    }


    void LateUpdate()
    {
        //3D배 화면을 새로 켰을 때(이전에 한번 킨적이 있을 경우)
        if (SystemManager.shipCloseClick)
        {
            //카메라 처음 위치 세팅(포지션, 로테이션, fieldOfview 
            camera.transform.position = cameraStartPos;
            camera.transform.localRotation = Quaternion.Euler(0, 0, 0);
            camera.fieldOfView = 60f;
            SystemManager.shipCloseClick = false;   //3D배 화면 닫기 버튼 안눌렀다고 변경
        }

        //두 손 클릭 시에만 적용 ( 확대/축소)
        if (TouchSystem.leftshipClick && TouchSystem.rightshipClick)
        {
            //if (SystemManager.zoominClick)
            //{
            firstPos = TouchSystem.screenMoveRightShipPos.x;    //움직이는 위치 받음

            if (firstPos != lastPos)    //처음 위치 나중 위치가 달랐을때
            {
                float dir = (lastPos - firstPos) / 5;    //움직이는 거리
                                                         //Debug.Log((firstPos - lastPos) + ":::" + firstPos + ":::" + lastPos);
                //최대줌아웃
                if (camera.fieldOfView <= 7f && dir < 0)
                {
                    camera.fieldOfView = 7f;
                }
                else if (camera.fieldOfView >= 158f && dir > 0) //최대줌
                {
                    camera.fieldOfView = 158f;
                }
                else
                {
                    camera.fieldOfView += dir;  //카메라 움직임
                }

                lastPos = firstPos; //한번 적용한 후 나중 위치값 저장=
            }
            //}
        }
        //한 손 적용 시 회전 
        else if (TouchSystem.rightshipClick)
        {
            firstXPos = TouchSystem.screenMoveRightShipPos.x;    //움직이는 위치 받음
            firstYPos = TouchSystem.screenMoveRightShipPos.y;

            //처음위치랑 나중 위치가 다르면(즉 움직였을 때)
            if (firstXPos != lastXPos)
            {
                float dirX = firstXPos - lastXPos;    //움직이는 거리
                //Debug.Log((firstXPos - lastXPos) + ":::" + firstXPos + ":::" + lastXPos);
                transform.RotateAround(target.transform.position, new Vector3(0f, dirX, 0f), 40f * Time.deltaTime);
            }

            if (firstYPos != lastYPos)
            {
                float dirY = lastYPos - firstYPos;
                //Debug.Log((firstYPos - lastYPos) + ":::" + firstYPos + ":::" + lastYPos);
                transform.RotateAround(target.transform.position, new Vector3(dirY, 0f, 0f), 40f * Time.deltaTime);
            }

            lastXPos = firstXPos; //한번 적용한 후 나중 위치값 저장
            lastYPos = firstYPos;
        }

    }
}
