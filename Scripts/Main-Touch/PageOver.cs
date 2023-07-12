using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageOver : MonoBehaviour
{
    public Vector2[] page;  //0 : 메인(0), 1 : 배(-1920), 2:음악(-3840), 3:머리(-5760)

    public bool check = true;



    private void Update()
    {
        //각 페이지에서 닫기 버튼 눌렀을 때(배, 음악, 머리 Close Btn)
        if(TouchSystem.pageOverBtn)
        {
            Debug.Log("들어옴");
            if (check)
            {
                TouchSystem.touchState = false; //컨텐츠 오브젝트 터치 여부 놉
                check = false;
                StartCoroutine("Page_Change_", 0);  //메인 화면으로 이동
                TouchSystem.pageOverBtn = false;    //닫기 버튼 안누름으로 변경ㄴ
            }
            
        }
    }

    //화면 이동 함수(0~4)
    public void Page_Change(int num)
    {
        if (check)
        {
            TouchSystem.touchState = false;
            check = false;
            StartCoroutine("Page_Change_", num);
        }
    }

    IEnumerator Page_Change_(int num)
    {
        //num = 0일때, 페이지 움직이지 않느다.
        if(num == 0)
        {
            //메인 화면으로 돌아가면 
            ContentObjCtrl.pageMove = false;    //페이지 이동 없음

            //선택했던 머리 스타일 해제
            ContentObjCtrl.hairstyle1 = false;  
            ContentObjCtrl.hairstyle2 = false;
            ContentObjCtrl.hairstyle3 = false;
        }

        while (check == false)
        {
            //각페이지 위치로 이동
            this.transform.localPosition = new Vector2(page[num].x, this.transform.localPosition.y);
            
            check = true;

            //부드럽게 넘어가는 화면(옆으로 스르르 넘어가는 화면)
            //transform.localPosition = Vector2.Lerp(this.transform.localPosition, page[num], Time.deltaTime * 7);

            //if (Mathf.Abs(this.transform.localPosition.x - page[num].x) <= 2)
            //{
            //    Debug.Log("???");
            //    this.transform.localPosition = new Vector2(page[num].x, this.transform.localPosition.y);
            //    check = true;
            //}

            yield return null;
        }
    }
}
