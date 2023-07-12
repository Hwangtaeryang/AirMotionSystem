using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairSliderBarCtrl : MonoBehaviour
{
    public static float ref_SliderValue;
    public Slider g_Slider;
    
    public Transform hairHandleCube;    //색상 조절 슬라이더바 핸들 큐브
    public Transform moveFinger;    //손가락 움직임 위치
    
    public static float val;


    void Start()
    {
        ref_SliderValue = 0.65f;
    }

    
    void Update()
    {
        //g_Slider.value = ref_SliderValue;
        //Debug.Log(hairHandleCube.position);
        //Debug.Log(moveFinger.position.x);
        //색상 슬라이더바 움직일 때
        if (TouchSystem.hairHandleState)
        {
            //Debug.Log(hairHandleCube.position);
            //슬라이더바 범위에 있을 때
            if (hairHandleCube.position.x >= -147f && hairHandleCube.position.x <= -72f)
            {
                //손가락 움직이는 곳으로 x좌표 변경
                hairHandleCube.position = new Vector3(moveFinger.position.x, hairHandleCube.position.y, hairHandleCube.position.z);
                //Debug.Log(hairHandleCube.position.x);
                
                val = Mathf.Lerp(0, 1, Mathf.InverseLerp(-147f, -72f, hairHandleCube.position.x));
                g_Slider.value = val;   //슬라이더바 값 넣어줌
            }

            //슬라이더바 최소범위 넘어서면
            else if (hairHandleCube.position.x < -147f)
                hairHandleCube.position = new Vector3(-147f, hairHandleCube.position.y, hairHandleCube.position.z);

            //슬라이더바 최대범위 넘어서면
            else if (hairHandleCube.position.x > -72f)
                hairHandleCube.position = new Vector3(-72f, hairHandleCube.position.y, hairHandleCube.position.z);
            
        }
    }

    // On Value Change
    public void HairSliderBar()
    {
        ref_SliderValue = g_Slider.value;
    }
}