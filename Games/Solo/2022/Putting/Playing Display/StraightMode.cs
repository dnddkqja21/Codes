using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StraightMode : MonoBehaviour
{
    [Header("가이드 라인")]
    public RectTransform[] guideLines;
    [Header("홀컵")]
    public GameObject[] holecups;

    float maxWidth = 700;
    float width = 0;
    float height = 0;

    int guideLineIndex = 0;

    void Start()
    {
        string[] temp = EnumToData.Instance.StraightRule(GameOption.Instance.straightLevel).Split(",");
        width = int.Parse(temp[0]);
        height = int.Parse(temp[1]);

        // 가져온 데이터에서 2배의 스케일 적용해야 함
        width *= 2;
        height *= 2;

        // 직선 훈련 단계에 따른 가이드 라인 세팅
        for (int i = 0; i < guideLines.Length; i++)
        {            
            guideLines[i].sizeDelta = new Vector2(maxWidth, height);
            guideLines[i].GetComponent<Image>().fillAmount = width / maxWidth;
        }
        
        InitRound();
    }
   

    public void InitRound()
    {        
        // 1 ~ 7 라운드(3번 치면 다른 라인)
        // 3 x 3라인(9라운드) 사용했다면 볼 정리 팝업 (9의 배수 라운드)
        if(GameOption.Instance.straightLevel != (int)StraightLevel.EIGHT)
        {
            // 모두 비활성화
            for (int i = 0; i < guideLines.Length; i++)
            {
                guideLines[i].gameObject.SetActive(false);                
            }  

            // 진행 횟수가 0이 아니고, 3의 배수면 인덱스 증가
            if (GameOption.Instance.progressCount  % 3 == 0 && GameOption.Instance.progressCount != 0)
            {
                guideLineIndex++;
                //Debug.Log("가이드 라인 인덱스 : "+guideLineIndex);
            }

            // 9의 배수 라운드면 인덱스 초기화, 공 정리 팝업 띄움
            if(GameOption.Instance.progressCount % 9 == 0 && GameOption.Instance.progressCount != 0)
            {
                guideLineIndex = 0;
                PopupManager.Instance.cleanBallPopup.SetActive(true);
            }
            guideLines[guideLineIndex].gameObject.SetActive(true);
            
        }
        else
        {
            // 8 라운드(랜덤 홀컵 지정)
            for (int i = 0; i < guideLines.Length; i++)
            {            
                guideLines[i].gameObject.SetActive(false);
                holecups[i].SetActive(false);            
            }

            int random = Random.Range(0, 3);
            guideLines[random].gameObject.SetActive(true);
            holecups[random].SetActive(true);
            
            if (GameOption.Instance.progressCount % 9 == 0 && GameOption.Instance.progressCount != 0)
            {                
                PopupManager.Instance.cleanBallPopup.SetActive(true);
            }
        }
    }
}
