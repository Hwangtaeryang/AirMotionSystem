using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HairContentText : MonoBehaviour
{
    TextMeshProUGUI contentText;

    void Start()
    {
        contentText = this.gameObject.GetComponent<TextMeshProUGUI>();
    }

    
    void Update()
    {
        //Debug.Log(ContentObjCtrl.hairstyle1);
        if(ContentObjCtrl.hairstyle1)
        {
            contentText.text = "깔끔하고 단정한 헤어스타일을 원하는 남성분에게 추천드립니다." + "\n" +
                "너무 긴 기장의 머리에서 다듬는 것보다, 짧은 기장에서 더 모양이" + "\n" + 
                "예쁘게 나오는 헤어컷입니다. 다운펌과 불륨펌을 함께 진행하시면,"+"\n" +
                "드라마 속 남자 주인공 머리를 만들 수 있습니다." + "\n" +
                "밝은 갈색으로 염색 시 트랜디하고 세련된 인상을 줄 수 있습니다." + "\n" +
                "헤어 컬러 추천은 '브라운'입니다." + "\n\n" +
                "드라마 연애의 발견 / 서강준 스타일" + "\n" +
                "영화 어제보다 오늘 더 / 현빈 스타일" + "\n" +
                "CF티르티르 로션광고 / 박서준 스타일" + "\n";
        }
        else if (ContentObjCtrl.hairstyle2)
        {
            contentText.text = "예쁘게 나오는 헤어컷입니다. 다운펌과 불륨펌을 함께 진행하시면," + "\n" +
                "드라마 속 남자 주인공 머리를 만들 수 있습니다." + "\n" +
                "밝은 갈색으로 염색 시 트랜디하고 세련된 인상을 줄 수 있습니다." + "\n" +
                "헤어 컬러 추천은 '브라운'입니다." + "\n\n" +
                "드라마 연애의 발견 / 서강준 스타일" + "\n" +
                "영화 어제보다 오늘 더 / 현빈 스타일" + "\n" +
                "CF티르티르 로션광고 / 박서준 스타일" + "\n";
        }
        else if (ContentObjCtrl.hairstyle3)
        {
            contentText.text = "예쁘게 나오는 헤어컷입니다. 다운펌과 불륨펌을 함께 진행하시면," + "\n" +
                "드라마 속 남자 주인공 머리를 만들 수 있습니다." + "\n" +
                "밝은 갈색으로 염색 시 트랜디하고 세련된 인상을 줄 수 있습니다." + "\n" +
                "헤어 컬러 추천은 '브라운'입니다." + "\n\n" +
                "드라마 연애의 발견 / 서강준 스타일" + "\n" +
                "영화 어제보다 오늘 더 / 현빈 스타일" + "\n" +
                "CF티르티르 로션광고 / 박서준 스타일" + "\n";
        }
    }
}
