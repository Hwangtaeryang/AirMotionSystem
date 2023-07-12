using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class TouchSystem : MonoBehaviour
{

    [DllImport("user32")]
    static extern Int32 GetCursorPos(out POINT pt);

    [DllImport("user32")]
    static extern Int32 SetCursorPos(Int32 x, Int32 y);

    [DllImport("user32.dll")]
    static extern void mouse_event(int dwFlage, int dx, int dy, int dwData, int dwExtraInfo);

    [DllImport("user32.dll")]
    static extern void mouse_event(int dwFlage);

    [Flags()]
    public enum MouseEventFlag : int
    {
        Absolute = 0x8000,
        LeftDown = 0x0002,
        LeftUp = 0x0004,
        MiddleDown = 0x0020,
        MiddleUp = 0x0040,
        Move = 0x0001,
        RightDown = 0x0008,
        RightUp = 0x0010,
        Wheel = 0x0800,
        XDown = 0x0080,
        XUp = 0x0100,
        HWheel = 0x1000,
    }

    public struct POINT
    {
        public Int32 x;
        public Int32 y;
    }


    // 절대적인 좌표로 마우스를 움직일때, 약간 특수한 계산이 필요합니다.
    #region Constant
    const int ABSOLUTE_SIZE = 65535;
    #endregion

    public Camera camera;

    public static bool touchState;  //컨텐츠큐브 터치 여부
    public static string touchName; //나중에 클릭한 컨텐츠
    public static string firstName = "None";    // 처음 클릭한 컨텐츠
    public static int touchNum = 0; //클릭 횟수(더블클릭)
    public static bool leftshipClick = false;   //왼손 배 클릭여부
    public static bool rightshipClick = false;  //오른손 배 클릭여부

    //배클릭 좌표
    public static Vector3 screenStartLeftShipPos;
    public static Vector3 screenStartRightShipPos;
    public static Vector3 screenMoveLeftShipPos;
    public static Vector3 screenMoveRightShipPos;
    float startPos, movePos;

    //음악 리모컨 버튼 클릭여부
    public static bool loopState;
    public static bool unLoopState;
    public static bool playState;
    public static bool pauseState;
    public static bool prevState;
    public static bool nextState;
    public static bool randomState;
    public static bool unRandomState;

    //음악 리모컨 버튼에서 뜬 여부 
    public static bool loopUnClick;
    public static bool unLoopUnClick;
    public static bool playUnClick;
    public static bool pauseUnClick;
    public static bool randomUnClick;
    public static bool unRandomUnClik;
    

    //시스템 버튼 클릭 여부
    public static bool mainCloseBtn;
    public static bool mainMiniBtn;
    public static bool pageOverBtn;

    //헤어 핸들러 움직임 여부
    public static bool hairHandleState;
    public static bool hairOrangeCube;
    public static bool hairRedCube;
    public static bool hairGreenCube;
    public static bool hairPurpleCube;
    public static bool hairBlueCube;

    //음악 버튼 클릭 여부
    public static bool listBtnOnClick;
    public static bool singBtnOnClick;




    private void Awake()
    {
        touchState = false;

        //음악 리모컨 버튼 세팅
        loopState = false; unLoopState = false; playState = false; pauseState = false;
        prevState = false; nextState = false; randomState = false; unRandomState = false;

        //음악 리모컨 활성화 비활성화 확인 여부 세팅
        loopUnClick = true; unLoopUnClick = true; playUnClick = true; pauseUnClick = true;
        randomUnClick = true; unRandomUnClik = true;

        //헤어 핸들러 세팅
        hairHandleState = false;
        hairOrangeCube = false; hairRedCube = false; hairGreenCube = false; hairPurpleCube = false; hairBlueCube = false;

         //시스템 버튼 세팅
         mainCloseBtn = false; mainMiniBtn = false; pageOverBtn = false;

        //음악 버튼 세팅
        listBtnOnClick = false; singBtnOnClick = false;
    }



    void Start()
    {

    }


    // 마우스를 제어하는 함수들입니다.
    #region Moving
    /// <summary>
    /// 현재좌표를 기준으로 마우스 움직임
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    public void Move(int _x, int _y)
    {
        MouseEventFlag Flag = MouseEventFlag.Move;
        mouse_event((int)Flag, (int)_x, (int)_y, 0, 0);
    }

    /// <summary>
    /// 모니터를 기준으로 마우스를 움직임
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    public void MoveAt(int _x, int _y)
    {
        MouseEventFlag Flag = MouseEventFlag.Move | MouseEventFlag.Absolute;

        int X = (int)(ABSOLUTE_SIZE * _x / 1920);
        int Y = (int)(ABSOLUTE_SIZE * _y / 1080);
        //Debug.Log(X + ":::" + Y + ":::");

        mouse_event((int)Flag, X, Y, 0, 0);
    }

    #endregion


    void Update()
    {
        POINT pt;
        Vector3 screenPos = camera.WorldToScreenPoint(transform.position);
        //Debug.Log(transform.position.y+"target is " + screenPos.y + " pixels from the left");

        //float valx = Mathf.Lerp(0f, 1920f, Mathf.InverseLerp(-130f, -28f, this.transform.position.x));
        //float valy = Mathf.Lerp(0f, 1080f, Mathf.InverseLerp(-14f, 35f, this.transform.position.y));
        float valx = screenPos.x;
        float valy = 1080 - screenPos.y;
        SetCursorPos((int)valx, (int)valy);
        GetCursorPos(out pt);
        MoveAt(pt.x, pt.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        //MouseEventFlag Flag = MouseEventFlag.Move | MouseEventFlag.LeftDown;
        MouseEventFlag Flag = MouseEventFlag.LeftDown;  // 마우스 다운 이벤트 

        //메인에서 컨텐츠 움직이게 하는 판넬
        if (other.CompareTag("VIEWPANEL"))
        {
            mouse_event((int)Flag);
        }

        //컨텐츠
        if (other.CompareTag("CONTENT"))
        {
            touchState = true;  //터치했음
            touchName = other.name; //컨텐츠 이름

            //처음 클릭한 컨텐츠 이름과 두번째 누른 컨텐츠 이름이 같으면
            if (firstName == other.name)
            {
                touchNum++; //더블 클릭을 확인을 위해서 카운트를 올려준다.(접촉 한번에 ++해줌)
            }
            //처음 누른 컨텐츠와 두번째 누른 컨텐츠가 다를 경우
            else if (firstName != other.name)
            {
                touchNum = 0;   //터치횟수는 0
            }
        }

        //닫기 버튼 터치
        if (other.CompareTag("CLOSEBTN"))
        {
            //mouse_event((int)Flag, 0, 0, 0, 0);
            mouse_event((int)Flag);
        }

        //음악 리모컨 터치 시
        if (other.CompareTag("REMOTE"))
        {
            mouse_event((int)Flag);

            //Debug.Log(other.name);
            //이전/다음 버튼 터치
            if (other.name == "PrevCube")
            {
                prevState = true;
            }
            else if (other.name == "NextCube")
            {
                nextState = true;
            }
        }

        //배 터치 판넬 터치 
        if (other.CompareTag("SHIP"))
        {
            //mouse_event((int)Flag, 0, 0, 0, 0);
            mouse_event((int)Flag);

            //왼쪽 손가락이 터치가 되었따.
            if (this.gameObject.name == "LeftTouchPos")
            {
                leftshipClick = true;   //왼손 배 클릭 했음.
                screenStartLeftShipPos = camera.WorldToScreenPoint(transform.position);
                startPos = screenStartLeftShipPos.x;    //좌표 
                //Debug.Log("Left클릭좌표" + startPos);
            }

            //오른쪾 손가락이 터치가 되었따.
            if (this.gameObject.name == "RightTouchPos")
            {
                rightshipClick = true;  //오른손 배 클릭
                screenStartRightShipPos = camera.WorldToScreenPoint(transform.position);
                startPos = screenStartRightShipPos.x;
                //Debug.Log("Right클릭좌표" + startPos);
            }
        }

        //각각의 버튼 터치
        if (other.CompareTag("BUTTON"))
        {
            //시스템 닫기 
            if (other.name == "CloseBtnPanel")
            {
                mainCloseBtn = true;
            }
            //최소화 버튼
            else if (other.name == "MiniBtnPanel")
            {
                mainMiniBtn = true;
            }
            //각각의 페이지의 닫기 버튼
            else if (other.name == "ShipCloseBtnPanel" || other.name == "MusicCloseBtnPanel" || other.name == "HairCloseBtnPanel")
            {
                pageOverBtn = true;
            }
            //음악페이지 앨범리스트탭 버튼
            else if(other.name == "ListBtnCube")
            {
                Debug.Log("첩촉");
                listBtnOnClick = true;
            }
            //음악페이지 가사탭 버튼
            else if(other.name == "SingBtnCube")
            {
                singBtnOnClick = true;
            }
        }

        //머리 색 조절 버튼
        if(other.CompareTag("HAIR"))
        {
            //색 조절하는 슬라이더 바
            if(other.name == "SliderHandleCube")
            {
                //Debug.Log("접촉");
                hairHandleState = true;
            }
            //오렌지 색
            else if(other.name == "OrangeCube")
            {
                hairOrangeCube = true; hairRedCube = false; hairGreenCube = false; hairPurpleCube = false; hairBlueCube = false;
            }
            //빨간색
            else if (other.name == "RedCube")
            {
                hairOrangeCube = false; hairRedCube = true; hairGreenCube = false; hairPurpleCube = false; hairBlueCube = false;
            }
            //녹색
            else if (other.name == "GreenCube")
            {
                hairOrangeCube = false; hairRedCube = false; hairGreenCube = true; hairPurpleCube = false; hairBlueCube = false;
            }
            //보라색
            else if (other.name == "PurpleCube")
            {
                hairOrangeCube = false; hairRedCube = false; hairGreenCube = false; hairPurpleCube = true; hairBlueCube = false;
            }
            //파란색
            else if (other.name == "BlueCube")
            {
                hairOrangeCube = false; hairRedCube = false; hairGreenCube = false; hairPurpleCube = false; hairBlueCube = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //배 터치 판넬 터치
        if (other.CompareTag("SHIP"))
        {
            //왼손가락 터치
            if (this.gameObject.name == "LeftTouchPos")
            {
                leftshipClick = true;   //배 클릭 했음.
                screenMoveLeftShipPos = camera.WorldToScreenPoint(transform.position);
                startPos = screenMoveLeftShipPos.x;
                //Debug.Log("Left클릭좌표Stay" + startPos);
            }

            //오른손가락 터치
            if (this.gameObject.name == "RightTouchPos")
            {
                rightshipClick = true;
                screenMoveRightShipPos = camera.WorldToScreenPoint(transform.position);
                startPos = screenMoveRightShipPos.x;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //MouseEventFlag Flag = MouseEventFlag.Move | MouseEventFlag.LeftUp;
        MouseEventFlag Flag = MouseEventFlag.LeftUp;    //클릭 땜

        //메인 컨텐츠 움직일 판넬
        if (other.CompareTag("VIEWPANEL"))
        {
            //mouse_event((int)Flag, 0, 0, 0, 0);
            mouse_event((int)Flag);
        }

        //컨텐츠
        if (other.CompareTag("CONTENT"))
        {
            //mouse_event((int)Flag, 0, 0, 0, 0);
            mouse_event((int)Flag);
            touchState = false;
            firstName = other.name; //터치한 오브젝트 이름
        }

        //시스템버튼
        if (other.CompareTag("CLOSEBTN"))
        {
            //mouse_event((int)Flag, 0, 0, 0, 0);
            mouse_event((int)Flag);
        }

        //음악 리모컨 
        if (other.CompareTag("REMOTE"))
        {
            mouse_event((int)Flag);

            
            if (other.name == "LoopCube")
            {
                loopState = true;   //loopcube 터치
                loopUnClick = true; //loop버튼 누름
                //unLoopUnClick = false;
            }
            else if (other.name == "UnLoopCube")
            {
                unLoopState = true;
                unLoopUnClick = true;
                //loopUnClick = false;
            }
            else if (other.name == "PlayCube")
            {
                playState = true;
                playUnClick = true;
                //pauseUnClick = false;
            }
            else if (other.name == "PauseCube")
            {
                pauseState = true;
                pauseUnClick = true;
                //playUnClick = false;
            }
            else if (other.name == "RandomCube")
            {
                randomState = true;
                randomUnClick = true;
                //unRandomUnClik = false;
            }
            else if (other.name == "UnRandomCube")
            {
                unRandomState = true;
                unRandomUnClik = true;
                //randomUnClick = false;
            }
        }


        if (other.CompareTag("SHIP"))
        {
            //mouse_event((int)Flag, 0, 0, 0, 0);
            mouse_event((int)Flag);
            leftshipClick = false;   //배 클릭 안했음.
            rightshipClick = false;
        }

        //머리 색상 조절 터치
        if (other.CompareTag("HAIR"))
        {
            //색상조절 큐브
            if (other.name == "SliderHandleCube")
            {
                hairHandleState = false; //노터치
            }
        }
    }


}