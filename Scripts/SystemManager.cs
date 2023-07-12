using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    //지정된 창의 표시 상태 설정
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

    //활성화된 윈도우-함수를 호출한 쓰레드와 연동된 녀석의 핸들을 받는다.
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();


    public static bool shipCloseClick;  //3D배 닫기 버튼

    public GameObject listSliderView;   //앨범리스트 뷰(앨범페이지)
    public GameObject singSliderView;   //가사 뷰(앨범페이지)
    public Image lineImge;  //탭 밑 라인(앨범페이지)


    

    private void Start()
    {
        shipCloseClick = false; //3D배 닫기 버튼 안누름
        listSliderView.SetActive(true); //앨범뷰 활성화
        singSliderView.SetActive(false);    //가사뷰 비활성화
    }

    private void Update()
    {
        //음악페이지에서 앨범리스트탭 버튼 터치 시
        if(TouchSystem.listBtnOnClick)
        {
            listSliderView.SetActive(true);
            singSliderView.SetActive(false);
            lineImge.sprite = Resources.Load<Sprite>("MusicImage/ListImg");
            TouchSystem.listBtnOnClick = false;
        }

        //음악페이지에서 노래가사탭 버튼 터치 시
        else if (TouchSystem.singBtnOnClick)
        {
            listSliderView.SetActive(false);
            singSliderView.SetActive(true);
            lineImge.sprite = Resources.Load<Sprite>("MusicImage/SingImg");
            TouchSystem.singBtnOnClick = false;
        }
    }


    //3D배 닫기 버튼 클릭 이벤트
    public void ShipCloseBtnOnClick()
    {
        shipCloseClick = true;  //3D배 닫기 버튼 누름
    }

    //앨범페이지에 있는 리스트탭버튼 클릭 시
    public void ListBtnOnClick()
    {
        listSliderView.SetActive(true);
        singSliderView.SetActive(false);
        lineImge.sprite = Resources.Load<Sprite>("MusicImage/ListImg");
    }

    //앨범페이지에 있는 가사탭버튼 클릭 시
    public void SingBtnOnClick()
    {
        listSliderView.SetActive(false);
        singSliderView.SetActive(true);
        lineImge.sprite = Resources.Load<Sprite>("MusicImage/SingImg");
    }


    /// <summary>
    /// 시스템 종료
    /// </summary>
    public void OnWindwCloseButtonClick()
    {
        Application.Quit();
    }

    /// <summary>
    /// 윈도우폼 최소화 함수
    /// </summary>
    public void OnMiniMizeButtonClick()
    {
        ShowWindow(GetActiveWindow(), 2);
        TouchSystem.mainMiniBtn = false;
    }


}
