using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingCtrl : MonoBehaviour
{
    [Range(1, 50)]
    [Header("Controllers")]
    public int panCount;    //생성될 오브젝트 갯수

    [Range(0, 500)]
    public int panOffset;   

    [Range(0f, 20f)]
    public float snapSpeed;

    [Range(0f, 5f)]
    public float scaleOffset;

    [Range(1f, 20f)]
    public float scaleSpeed;

    [Header("Other Objects")]
    public GameObject panPrefab;    //복사할 프리팹 오브젝트

    public ScrollRect scrollRect;

    GameObject[] instPans;  //생성될 오브젝트
    Sprite[] mainSprites;
    Vector2[] pansPos;
    Vector2[] pansScale;    // 생성된 오브젝트 사이즈

    RectTransform contentRect;
    Vector2 contentVector;

    int selectedPanID;
    bool isScrolling;
    public GameObject viwPanel;


    void Start()
    {
        contentRect = GetComponent<RectTransform>();
        instPans = new GameObject[panCount];
        pansPos = new Vector2[panCount];
        pansScale = new Vector2[panCount];
        mainSprites = new Sprite[9];

        //for(int i = 0; i < panCount; i++)
        //{
        //    instPans[i] = Instantiate(panPrefab, transform, false);

        //    if (i == 0) continue;
        //    instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x + 
        //        panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset, instPans[i].transform.localPosition.y);
        //    pansPos[i] = -instPans[i].transform.localPosition;  //이동하는 위치
        //}
        StartMainImageLoadSetting();
    }


    private void FixedUpdate()
    {

        MovingShow();

        if (ContentObjCtrl.pageMove == true)
        {
            //Debug.Log("들옴?0");
            viwPanel.SetActive(false);
            for (int i = 0; i < panCount; i++)
            {
                instPans[i].SetActive(false);
            }
        }
        else
        {
            viwPanel.SetActive(true);
            for (int i = 0; i < panCount; i++)
            {
                instPans[i].SetActive(true);
            }
        }
    }

    //생성할 오브젝트 세팅 함수
    void StartMainImageLoadSetting()
    {
        for (int i = 0; i < panCount; i++)
        {
            instPans[i] = Instantiate(panPrefab, transform, false);
            instPans[i].name = "ContentObj" + (i+1);    //생성한 오브젝트 이름 변경
            instPans[i].transform.GetChild(0).name = "ClickView" + (i + 1); //자식 오브젝트 이름 변경
            instPans[i].transform.GetChild(1).name = "CheckLineImage" + (i + 1);    //자식 오브젝트 이름 변경
            MainImageSetting(i);



            if (i == 0) continue;
            instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x +
                panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset, instPans[i].transform.localPosition.y);
            pansPos[i] = -instPans[i].transform.localPosition;  //이동하는 위치
        }
    }

    //메인 이미지 세팅 함수
    //여기 수정하세요!!!!!!! 메인 컨텐츠 이미지
    void MainImageSetting(int _i)
    {
        mainSprites[_i] = Resources.Load<Sprite>("MainImage/Test/" + (_i+1)); //Test/
        instPans[_i].GetComponent<Image>().sprite = mainSprites[_i];
    }

    //화면에 보이는 페이지 움직이는 함수
    void MovingShow()
    {
        if (contentRect.anchoredPosition.x >= pansPos[0].x && !isScrolling ||
            contentRect.anchoredPosition.x <= pansPos[pansPos.Length - 1].x && !isScrolling)
        {
            scrollRect.inertia = false;
        }

        float nearestPos = float.MaxValue;
        for (int i = 0; i < panCount; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - pansPos[i].x);

            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
            }
            //메인 오브젝트 양옆 사이드에 잇는 오브젝트 스케일 작게 한다.
            float scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.5f, 1f);

            //작게 조정한 스케일은 오브젝트 사이즈에 적용 자연스럽게 변하게
            pansScale[i].x = Mathf.SmoothStep(instPans[i].transform.localScale.x, scale + 0.3f, scaleSpeed * Time.fixedDeltaTime);
            pansScale[i].y = Mathf.SmoothStep(instPans[i].transform.localScale.y, scale + 0.3f, scaleSpeed * Time.fixedDeltaTime);
            instPans[i].transform.localScale = pansScale[i];
        }
        float scrollVeloctiy = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVeloctiy < 400 && !isScrolling)
            scrollRect.inertia = false;

        if (isScrolling || scrollVeloctiy > 400) return;
        contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, pansPos[selectedPanID].x, snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll)
            scrollRect.inertia = true;
    }
}
