using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMouseCtrl : MonoBehaviour
{
    public Transform palm;  //값 받아올 오브젝트'

    float posFirstX, posFirstY, posFirstZ;
    float posLastX, posLastY, posLastZ;
    float _posLastX, _posLastY, _posLastZ;


    void Start()
    {
        posFirstX = palm.localPosition.x;
        posFirstY = palm.localPosition.y;
        posFirstZ = palm.localPosition.z;
    }

    
    void Update()
    {
        posLastX = palm.localPosition.x;
        _posLastX = Mathf.Lerp(-332, 153, Mathf.InverseLerp(-0.3f, 0.27f, posLastX));

        posLastY = palm.localPosition.y;
        _posLastY = Mathf.Lerp(-136f, 74f, Mathf.InverseLerp(0.11f, 0.54f, posLastY));

        posLastZ = palm.localPosition.z;
        _posLastZ = Mathf.Lerp(390f, 470f, Mathf.InverseLerp(-0.23f, 0.2f, posLastZ));

        HandMouseMove(_posLastX, _posLastY, _posLastZ);
    }

    public void HandMouseMove(float _moveX, float _moveY, float _moveZ)
    {
        this.transform.localPosition = new Vector3(_moveX, _moveY, _moveZ);
        //posFirstX = _moveX;
        //posFirstY = _moveY;
        //posFirstZ = _moveZ;
    }
}
