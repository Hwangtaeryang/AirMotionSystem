using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentObjCtrl : MonoBehaviour
{
    public Image checkLineImage;    // 컨텐츠 이미지

    int contentNum = 9; //컨텐츠 갯수
    string objName; //선택된 컨텐츠 이름

    public static int PrefabNum;    //3d모델링 번호
    public static bool pageMove = false;    //화면 전환 여부

    //헤어 스타일 선택 여부
    public static bool hairstyle1;
    public static bool hairstyle2;
    public static bool hairstyle3;

    //앨범 종류 선택 여부
    public static bool music1;
    public static bool music2;
    public static bool music3;

    GameObject spawnPos;    //3d 배 생성 위치
    GameObject copyObj; //배 복사할 오브젝트
 

    void Start()
    {
        spawnPos = GameObject.Find("SpawnPos").gameObject;  //배 생성 포인트 오브젝트
        copyObj = gameObject.GetComponent<GameObject>();    //컨텐츠 복사할 오브젝트

        //머리 스타일 선택 여부 초기값
        hairstyle1 = false; hairstyle2 = false; hairstyle3 = false;

        //음악 선택 여부 초기값
        music1 = false; music2 = false; music3 = false;
    }

    
    void Update()
    {
        //컨텐츠 클릭 시 
        if (TouchSystem.touchState)
        {
            OnClickLineColorChage();    //선택 했다고 색 변환해줌
        }

    }
    

    //선택 시 색 변환 함수
    public void OnClickLineColorChage()
    {
        for(int i = 0; i < contentNum; i++)
        {
            //선택된 오브젝트가 어떤건지 알기위한 조건식
            if (TouchSystem.touchName == "ClickView"+(i+1))
            {
                if (checkLineImage.name == "CheckLineImage" + (i + 1))
                {
                    //선택했을 때 변경 색상
                    checkLineImage.color = new Color32(53, 235, 71, 255);

                    // 페이지 전환 시 이름을 기억못 테두리 색 변화를 못해서 자체 이름 저장해서 뿌려줌
                    objName = checkLineImage.name;
                    
                    //업데이트문에 들어가면 여러번 적용이 되기때문에 한번만 실행하기 위한 조건
                    if (TouchSystem.touchNum == 2)
                    {
                        // 2020-03-23
                        // 여기서 이미지 챙기기!
                        // 1. 앨범이미지 1개
                        // 2. 인물 1개, 헤어 5개, 설명 1개 > 2개

                        // 현재 이미지 저장
                        // i+1 -> 4~6 : 뮤직, 7~9 : 헤어
                        if ((4 <= (i + 1)) && ((i + 1) <= 6))
                        {
                            // 앨범이미지
                            if((i+1) == 4)
                                GameObject.Find("AlbumImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("MusicImage/Music1");
                            else if ((i + 1) == 5)
                                GameObject.Find("AlbumImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("MusicImage/Music2");
                            else if ((i + 1) == 6)
                                GameObject.Find("AlbumImage").GetComponent<Image>().sprite = Resources.Load<Sprite>("MusicImage/Music3");
                        }
                        else if ((7 <= (i + 1)) && ((i + 1) <= 9))
                        {
                            // 어떤 사진 선택했는지
                            if ((i + 1) == 7)
                            {
                                hairstyle1 = true;  //머리 선택
                                hairstyle2 = false; hairstyle3 = false;
                            }
                            else if ((i + 1) == 8)
                            {
                                hairstyle2 = true;  //머리선택
                                hairstyle1 = false; hairstyle3 = false;
                            }
                            else if ((i + 1) == 9)
                            {
                                hairstyle3 = true;  //머리선택
                                hairstyle1 = false; hairstyle2 = false;
                            }
                                
                        }
                        
                        PageChange((i + 1));    //해당 페이지로 전환해주는 함수
                    }
                }
            }
        }

        Invoke("UnClickLineColorChage", 3f);    //색 해제하는 함수

    }

    //선택 해제 색 변환 함수
    public void UnClickLineColorChage()
    {
        for (int i = 0; i < contentNum; i++)
        {
            if (TouchSystem.touchName == "ClickView" + (i + 1))
            {
                if (checkLineImage.name == objName)
                {
                    checkLineImage.color = new Color32(255, 255, 255, 255);
                    TouchSystem.touchNum = 0;   //색 변환 시 선택 해제가 되었다는 뜻으로 터치 카운트를 0으로 해줌
                }
            }
        }
    }
    

    //선택한 컨텍츠의 화면으로 이동 함수
    void PageChange(int num)
    {
        GameObject page = GameObject.Find("SystemPanel");
        var pageover = page.GetComponent<PageOver>();

        if (num <= 3) //배
        {
            pageMove = true;    //페이지 이동

            if (num == 1)
            {
                //생성 위치에 자식이 없으면 생성
                if (spawnPos.transform.childCount == 0)
                    copyObj = Instantiate(Resources.Load("Prefab/Frigate"), spawnPos.transform) as GameObject;
                else
                {
                    //자식이 있으면 지우고 다시 생성
                    Destroy(spawnPos.transform.GetChild(0).gameObject);
                    copyObj = Instantiate(Resources.Load("Prefab/Frigate"), spawnPos.transform) as GameObject;
                }
            }
            else if (num == 2)
            {
                if (spawnPos.transform.childCount == 0)
                    copyObj = Instantiate(Resources.Load("Prefab/Cruiser"), spawnPos.transform) as GameObject;
                else
                {
                    Destroy(spawnPos.transform.GetChild(0).gameObject);
                    copyObj = Instantiate(Resources.Load("Prefab/Cruiser"), spawnPos.transform) as GameObject;
                }
            }
            else if (num == 3)
            {
                if (spawnPos.transform.childCount == 0)
                    copyObj = Instantiate(Resources.Load("Prefab/Corvette"), spawnPos.transform) as GameObject;
                else
                {
                    Destroy(spawnPos.transform.GetChild(0).gameObject);
                    copyObj = Instantiate(Resources.Load("Prefab/Corvette"), spawnPos.transform) as GameObject;
                }
            }
            pageover.Page_Change(1);
        }
        else if (num >= 4 && num <= 6) //음악
        {
            pageMove = true;    //페이지 전환

            //음악 페이지 몇번째 선택했는지 알려줌
            if (num == 4)
            {
                music1 = true;
            }
            else if (num == 5)
            {
                music2 = true;
            }
            else if (num == 6)
            {
                music3 = true;
            }

            pageover.Page_Change(2);
        }
        else if (num >= 7 && num <= 9) //머리
        {
            pageMove = true;
            pageover.Page_Change(3);
        }
    }
}
