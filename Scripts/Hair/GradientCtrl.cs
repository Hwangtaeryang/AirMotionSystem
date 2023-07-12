using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using static HairSliderBarCtrl;
using System.Collections;

public class GradientCtrl : MonoBehaviour
{
    public Gradient gradient1;
    public Gradient gradient2;
    public Gradient gradient3;
    public Gradient gradient4;
    public Gradient gradient5;

    [Range(0, 1)]
    public float t1;
    [Range(0, 1)]
    public float t2;
    [Range(0, 1)]
    public float t3;
    [Range(0, 1)]
    public float t4;
    [Range(0, 1)]
    public float t5;

    [Header("Buttons Color")]
    public Image img1;
    public Image img2;
    public Image img3;
    public Image img4;
    public Image img5;
    [Header("Hairs Color")]
    public Image Hair_img1;
    public Image Hair_img2;
    public Image Hair_img3;
    public Image Hair_img4;
    public Image Hair_img5;
    //private GradientCtrl gradientCtrl;
    public static bool IsChecked;

    private static string gs_pre_img = "O_Background";  // 이전 선택 배경
    private static string gs_cur_img = "O_Background";  // 현재 선택 배경

    // Start is called before the first frame update
    //void Start()
    //{
    //    //img = transform.GetComponent<Image>();
    //    //gradientCtrl = transform.GetComponent<GradientCtrl>();
    //}

    // Update is called once per frame
    void Update()
    {
        if (gs_cur_img.Equals("O_Background"))
        {
            // 초기값
            if (TouchSystem.hairOrangeCube || IsChecked)
            {
                HairSliderBarCtrl.ref_SliderValue = t1;
                TouchSystem.hairOrangeCube = false;
                IsChecked = false;
            }

            t1 = HairSliderBarCtrl.ref_SliderValue;

            // Evaluate()라는 함수는 시간 변화에 따른 값을 산출해주는 함수입니다. 
            //(그러니까 매개변수로 넣어준 t에 해당하는 컬러를 반환하겠죠.  범위: 0 ~1)
            img1.color = gradient1.Evaluate(t1);
            Hair_img1.color = gradient1.Evaluate(t1);

        }
        else if (gs_cur_img.Equals("G_Background"))
        {
            // 초기값
            if (TouchSystem.hairGreenCube || IsChecked)
            {
                HairSliderBarCtrl.ref_SliderValue = t2;
                TouchSystem.hairGreenCube = false;
                IsChecked = false;
            }

            t2 = HairSliderBarCtrl.ref_SliderValue;

            img2.color = gradient2.Evaluate(t2);
            Hair_img2.color = gradient2.Evaluate(t2);
        }
        else if (gs_cur_img.Equals("P_Background"))
        {
            // 초기값
            if (TouchSystem.hairPurpleCube || IsChecked)
            {
                HairSliderBarCtrl.ref_SliderValue = t3;
                TouchSystem.hairPurpleCube = false;
                IsChecked = false;
            }

            t3 = HairSliderBarCtrl.ref_SliderValue;

            img3.color = gradient3.Evaluate(t3);
            Hair_img3.color = gradient3.Evaluate(t3);
        }
        else if (gs_cur_img.Equals("R_Background"))
        {
            // 초기값
            if (TouchSystem.hairRedCube || IsChecked)
            {
                HairSliderBarCtrl.ref_SliderValue = t4;
                TouchSystem.hairRedCube = false;
                IsChecked = false;
            }

            t4 = HairSliderBarCtrl.ref_SliderValue;

            img4.color = gradient4.Evaluate(t4);
            Hair_img4.color = gradient4.Evaluate(t4);
        }
        else if (gs_cur_img.Equals("B_Background"))
        {
            // 초기값
            if (TouchSystem.hairBlueCube || IsChecked)
            {
                HairSliderBarCtrl.ref_SliderValue = t5;
                TouchSystem.hairBlueCube = false;
                IsChecked = false;
            }

            t5 = HairSliderBarCtrl.ref_SliderValue;

            img5.color = gradient5.Evaluate(t5);
            Hair_img5.color = gradient5.Evaluate(t5);
        }

        //터치 시 칼라 색상 배치
        if(TouchSystem.hairOrangeCube)
        {
            SetSliderBar("O_Background");
        }
        else if(TouchSystem.hairRedCube)
        {
            SetSliderBar("R_Background");
        }
        else if (TouchSystem.hairGreenCube)
        {
            SetSliderBar("G_Background");
        }
        else if (TouchSystem.hairPurpleCube)
        {
            SetSliderBar("P_Background");
        }
        else if (TouchSystem.hairBlueCube)
        {
            SetSliderBar("B_Background");
        }
    }

    public void SetSliderBar(string _backgroundName)
    {
        IsChecked = true;
        // 다른 컴포넌트 비활성화
        gs_cur_img = _backgroundName;
        //Debug.Log("현재 선택된 배경색 : " + gs_cur_img);

        // Set False : 이전 선택했던 배경
        GameObject.Find("ref_Main_Hair_Slider").transform.Find(gs_pre_img).gameObject.SetActive(false);
        GameObject.Find("ref_Hair_Image").transform.Find(gs_pre_img).gameObject.SetActive(false);
        
        gs_pre_img = _backgroundName;
        //Debug.Log("이전 선택된 배경색 : " + gs_pre_img);

        // 현재 선택된 이미지 컴포넌트 활성화

        // Set True
        GameObject.Find("ref_Main_Hair_Slider").transform.Find(_backgroundName).gameObject.SetActive(true);
        GameObject.Find("ref_Hair_Image").transform.Find(_backgroundName).gameObject.SetActive(true);
    }
}
