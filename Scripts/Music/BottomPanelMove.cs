using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomPanelMove : MonoBehaviour
{
    private Animator animator;     //업다운 애니메이션
    //private bool IsUp = false;      //업 상태
    private bool DownState; //다운 상태

    bool remoteCtrlState = false;   //리모컨 누름 상태
    public static bool remoteMusic = false; //해당 앨범에 리스트를 가지고 오기 위한 변수
    public GameObject musicObj; //음악 리모컨
    public GameObject musicLock;
    //public GameObject hairObj;  //머리 리모컨


    void Start()
    {
        animator = GetComponent<Animator>();
        DownState = true;

        //클릭 전까지 비활성화 시킨다.
        musicLock.SetActive(true);
        musicObj.SetActive(false);  
        //hairObj.SetActive(false);
    }

    private void Update()
    {
        //리모컨이 올라와있을때 
        if(remoteCtrlState)
        {
            //컨텐츠를 돌리면 리모컨이 해당 컨텐츠에 맞게 변경되는 조건
            if (ContentRaycast.raycastName == "ClickView1" || ContentRaycast.raycastName == "ClickView2" ||
                ContentRaycast.raycastName == "ClickView3" || ContentRaycast.raycastName == "ClickView7" || ContentRaycast.raycastName == "ClickView8" ||
                ContentRaycast.raycastName == "ClickView9")
            {
                remoteCtrlState = false;    //리모컨 안누름
                remoteMusic = false;    //선택한 앨범의 리스트를 가지고 오기 위한 변수로 리모컨 안누름
                animator.SetBool("IsUp", false);
                DownState = true;
                Invoke("UnLookRemoteCtrl", 0.8f);
            }
            else if (ContentRaycast.raycastName == "ClickView4" || ContentRaycast.raycastName == "ClickView5" ||
                    ContentRaycast.raycastName == "ClickView6")
            {
                musicObj.SetActive(true);
                musicLock.SetActive(false);
                //hairObj.SetActive(false);
            }
            //머리일 때 누르면 머리리모컨이 나오게
            //else if (ContentRaycast.raycastName == "ClickView7" || ContentRaycast.raycastName == "ClickView8" ||
            //    ContentRaycast.raycastName == "ClickView9")
            //{
            //    musicObj.SetActive(false);
                //hairObj.SetActive(true);
            //}
        }
    }

    public void OnClick()
    {
        //음악, 머리 빼곤 리모컨이 올라오지 못하게 하는 조건
        if(ContentRaycast.raycastName == "ClickView1" || ContentRaycast.raycastName == "ClickView2" ||
                ContentRaycast.raycastName == "ClickView3" ||
                ContentRaycast.raycastName == "ClickView7" || ContentRaycast.raycastName == "ClickView8" ||
                    ContentRaycast.raycastName == "ClickView9")
        {
            remoteCtrlState = false;    //리모컨 안누름
            remoteMusic = false;
            animator.SetBool("IsUp", false);
            DownState = true;
        }
        //음악, 머리일 때 리모컨이 올라오게 하는 조건
        else
        {
            if (DownState)
            {
                remoteCtrlState = true; //리모컨 누름
                remoteMusic = true;

                // DefaultPanel 감추기
                animator.SetBool("IsUp", true);
                DownState = false;
                
                //음악일 때 누르면 음악리모컨이 나오게
                if (ContentRaycast.raycastName == "ClickView4" || ContentRaycast.raycastName == "ClickView5" ||
                    ContentRaycast.raycastName == "ClickView6")
                {
                    musicObj.SetActive(true);
                    musicLock.SetActive(false);
                    //hairObj.SetActive(false);
                }
                //머리일 때 누르면 머리리모컨이 나오게
                //else if (ContentRaycast.raycastName == "ClickView7" || ContentRaycast.raycastName == "ClickView8" ||
                //    ContentRaycast.raycastName == "ClickView9")
                //{
                    //musicObj.SetActive(false);
                    //hairObj.SetActive(true);
                //}
            }
            else
            {
                remoteCtrlState = false;    //리모컨 안누름
                remoteMusic = false;
                animator.SetBool("IsUp", false);
                DownState = true;

                Invoke("UnLookRemoteCtrl", 0.8f);

                // DefaultPanel 보이기
                //DefaultPanel.SetActive(true);
            }
        }
    }

    //모든 리모컨 비활성화 함순
    void UnLookRemoteCtrl()
    {
        musicObj.SetActive(false);
        musicLock.SetActive(true);
        //hairObj.SetActive(false);
    }
    
}
