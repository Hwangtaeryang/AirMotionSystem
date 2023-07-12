using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairChangeCtrl : MonoBehaviour
{
    Image hairImage;    //전체 사진

    //머리 색상 이미지
    public Image o_Background;
    public Image g_Background;
    public Image p_Background;
    public Image r_Background;
    public Image b_Background;

    void Start()
    {
        hairImage = this.gameObject.GetComponent<Image>();
    }

    
    void Update()
    {
        HairColorChange();
    }

    void HairColorChange()
    {
        //머리스타일 1 선택 시
        if (ContentObjCtrl.hairstyle1)
        {
            //이미지에 머리스타일1 이미지 넣어줌
            hairImage.sprite = Resources.Load<Sprite>("HairImage/HairStyle1");
            o_Background.gameObject.SetActive(true);    //처음 보일 머리 활성
            //Debug.Log(o_Background.transform.position);

            //이미지 위치 세팅
            o_Background.transform.position = new Vector3(-123f, 2.7f, 100);
            g_Background.transform.position = new Vector3(-123f, 2.7f, 100);
            p_Background.transform.position = new Vector3(-123f, 2.7f, 100);
            r_Background.transform.position = new Vector3(-123f, 2.7f, 100);
            b_Background.transform.position = new Vector3(-123f, 2.7f, 100);

            //크기 세팅
            o_Background.transform.localScale = new Vector3(4.6f, 5f, 1);
            g_Background.transform.localScale = new Vector3(4.6f, 5f, 11);
            p_Background.transform.localScale = new Vector3(4.6f, 5f, 1);
            r_Background.transform.localScale = new Vector3(4.6f, 5f, 1);
            b_Background.transform.localScale = new Vector3(4.6f, 5f, 1);

            //해당 머리 세팅
            o_Background.sprite = Resources.Load<Sprite>("HairImage/Hair1");
            g_Background.sprite = Resources.Load<Sprite>("HairImage/Hair1");
            p_Background.sprite = Resources.Load<Sprite>("HairImage/Hair1");
            r_Background.sprite = Resources.Load<Sprite>("HairImage/Hair1");
            b_Background.sprite = Resources.Load<Sprite>("HairImage/Hair1");
        }
        else if (ContentObjCtrl.hairstyle2)
        {
            hairImage.sprite = Resources.Load<Sprite>("HairImage/HairStyle2");
            o_Background.gameObject.SetActive(true);
            //Debug.Log(o_Background.transform.position);

            o_Background.transform.position = new Vector3(-124.3f, 5.9f, 100);
            g_Background.transform.position = new Vector3(-124.3f, 5.9f, 100);
            p_Background.transform.position = new Vector3(-124.3f, 5.9f, 100);
            r_Background.transform.position = new Vector3(-124.3f, 5.9f, 100);
            b_Background.transform.position = new Vector3(-124.3f, 5.9f, 100);

            o_Background.transform.localScale = new Vector3(5.7f, 5.9f, 1);
            g_Background.transform.localScale = new Vector3(5.7f, 5.9f, 1);
            p_Background.transform.localScale = new Vector3(5.7f, 5.9f, 1);
            r_Background.transform.localScale = new Vector3(5.7f, 5.9f, 1);
            b_Background.transform.localScale = new Vector3(5.7f, 5.9f, 1);

            o_Background.sprite = Resources.Load<Sprite>("HairImage/Hair2");
            g_Background.sprite = Resources.Load<Sprite>("HairImage/Hair2");
            p_Background.sprite = Resources.Load<Sprite>("HairImage/Hair2");
            r_Background.sprite = Resources.Load<Sprite>("HairImage/Hair2");
            b_Background.sprite = Resources.Load<Sprite>("HairImage/Hair2");
        }
        else if (ContentObjCtrl.hairstyle3)
        {
            hairImage.sprite = Resources.Load<Sprite>("HairImage/HairStyle3");
            o_Background.gameObject.SetActive(true);

            o_Background.transform.position = new Vector3(-124.3f, 5.9f, 100);
            g_Background.transform.position = new Vector3(-124.3f, 5.9f, 100);
            p_Background.transform.position = new Vector3(-124.3f, 5.9f, 100);
            r_Background.transform.position = new Vector3(-124.3f, 5.9f, 100);
            b_Background.transform.position = new Vector3(-124.3f, 5.9f, 100);

            o_Background.transform.localScale = new Vector3(5.7f, 5.9f, 1);
            g_Background.transform.localScale = new Vector3(5.7f, 5.9f, 1);
            p_Background.transform.localScale = new Vector3(5.7f, 5.9f, 1);
            r_Background.transform.localScale = new Vector3(5.7f, 5.9f, 1);
            b_Background.transform.localScale = new Vector3(5.7f, 5.9f, 1);

            o_Background.sprite = Resources.Load<Sprite>("HairImage/Hair3");
            g_Background.sprite = Resources.Load<Sprite>("HairImage/Hair3");
            p_Background.sprite = Resources.Load<Sprite>("HairImage/Hair3");
            r_Background.sprite = Resources.Load<Sprite>("HairImage/Hair3");
            b_Background.sprite = Resources.Load<Sprite>("HairImage/Hair3");
        }
        else
        {
            hairImage.sprite = Resources.Load<Sprite>("");
            //나갔을때 활성화되어 있는 머리 전부 비활성화
            o_Background.gameObject.SetActive(false);
            g_Background.gameObject.SetActive(false);
            p_Background.gameObject.SetActive(false);
            r_Background.gameObject.SetActive(false);
            b_Background.gameObject.SetActive(false);
        }
    }
}
