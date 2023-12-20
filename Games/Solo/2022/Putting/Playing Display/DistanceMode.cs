using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DistanceMode : MonoBehaviour
{
    [Header("가이드 라인")]
    public RectTransform guideLine;
    [Header("포인트 슬라이더")]
    public RectTransform setPoint;
    [Header("목표 거리")]
    public RectTransform targetDistance;

    // 난이도에 따른 포인트
    int easyMin = 2;
    int easyMax = 13;
    int normalMin = 4;
    int normalMax = 15;
    int hardMin = 6;
    int hardMax = 16;

    // 가이드 라인 최대 길이
    float maxWidth = 700;

    // 난이도 별 포인트 저장
    public List<float> points = new List<float>();

    // 포인트 인덱스
    int pointIndex = 0;

    // 난이도 별 반경 (매핑할 때 다시 적용)
    float successRadius = 0;

    // 슬라이더, 목표거리 원 위치
    Vector3 originPosSlider = Vector3.zero;
    Vector3 originPosTarget = Vector3.zero;

    // 이동할 위치
    Vector3 moveToPosSlider = Vector3.zero;
    Vector3 moveToPosTarget = Vector3.zero;

    // 고정 모드 서로 변경할 y값
    float offsetYFixed = 135;

    // 고정 모드 포인트 클리어
    bool isClear = false;

    // 가이드 라인 원위치
    Vector3 originPosGuideLine = Vector3.zero;

    // 가이드 라인 이동 위치
    Vector3 moveToPosGuideLine = Vector3.zero;

    // 랜덤, 순환 모드 변경할 y값
    float offsetYOther = 131;

    // 위치 변경
    bool isChanged = false;

    void Start()
    {
        originPosSlider = setPoint.transform.position;
        originPosTarget = targetDistance.transform.position;
        originPosGuideLine = guideLine.transform.position;

        moveToPosSlider = originPosSlider + new Vector3(0, offsetYFixed, 0);
        moveToPosTarget = originPosTarget - new Vector3(0, offsetYFixed, 0);
        moveToPosGuideLine = originPosGuideLine + new Vector3(0, offsetYOther, 0);              

        SetLevel();

        InitRound();
    }

    public void InitRound()
    {
        // 공통 영역 
        // 목표 거리
        targetDistance.gameObject.SetActive(true);

        // 6라운드 마다 공 정리
        if (GameOption.Instance.progressCount % 6 == 0 && GameOption.Instance.progressCount != 0)
        {
            PopupManager.Instance.cleanBallPopup.SetActive(true);
        }

        // 모드 별 작동
        switch (GameOption.Instance.distanceMethod)
        {
            case (int)DistanceMethod.FIXED:
                setPoint.gameObject.SetActive(true);

                // 고정 모드에서는 SetLevel();의 규칙이 적용되지 않으므로 한번만 클리어 한다.
                if (isClear == false)
                {
                    isClear = true;
                    points.Clear();
                }
                
                break;

            case (int)DistanceMethod.ROTATION:
                guideLine.gameObject.SetActive(true);

                guideLine.GetComponent<Image>().fillAmount = (points[pointIndex] * 2) / maxWidth;
                targetDistance.GetComponentInChildren<TextMeshProUGUI>().text = "목표 거리 : " + points[pointIndex].ToString() + "cm";
                pointIndex++;
                if(pointIndex > points.Count -1)
                {
                    pointIndex = 0;
                }   
                break;

            case (int)DistanceMethod.RANDOM:
                guideLine.gameObject.SetActive(true);

                pointIndex = Random.Range(0, points.Count);

                guideLine.GetComponent<Image>().fillAmount = (points[pointIndex] * 2) / maxWidth;
                targetDistance.GetComponentInChildren<TextMeshProUGUI>().text = "목표 거리 : " + points[pointIndex].ToString() + "cm";

                break;
        } 

        // 고정 모드 아니고, 진행 횟수가 0이 아니고, 3의 배수면 가이드 라인의 위치 바꿈
        if (GameOption.Instance.progressCount % 3 == 0 && GameOption.Instance.progressCount != 0 && GameOption.Instance.distanceMethod != (int)DistanceMethod.FIXED)
        {     
            isChanged = !isChanged;
            if (isChanged)
            {
                guideLine.transform.position = moveToPosGuideLine;
                targetDistance.transform.position = moveToPosTarget;
            }
            else
            {
                guideLine.transform.position = originPosGuideLine;
                targetDistance.transform.position = originPosTarget;
            }
        }
        // 고정 모드일 때는 포인트 슬라이더 위치 변경
        else if(GameOption.Instance.progressCount % 3 == 0 && GameOption.Instance.progressCount != 0 && GameOption.Instance.distanceMethod == (int)DistanceMethod.FIXED)
        {
            isChanged = !isChanged;
            if (isChanged)
            {
                setPoint.transform.position = moveToPosSlider;
                targetDistance.transform.position = moveToPosTarget;
            }
            else
            {
                setPoint.transform.position = originPosSlider;
                targetDistance.transform.position = originPosTarget;
            }
        }
    }

    // 난이도 세팅
    public void SetLevel()
    {
        switch(GameOption.Instance.distanceLevel)
        {
            case (int)DistanceLevel.EASY:
                // 2 ~ 13 포인트
                for (int i = easyMin; i <= easyMax; i++)
                {
                   points.Add(EnumToData.Instance.DistanceRule(i));
                }
                successRadius = 20;
                break;

            case (int)DistanceLevel.NORMAL:
                // 4 ~ 15 포인트
                for (int i = normalMin; i <= normalMax; i++)
                {
                    points.Add(EnumToData.Instance.DistanceRule(i));
                }
                successRadius = 15;
                break;

            case (int)DistanceLevel.HARD:
                // 6 ~ 16 포인트
                for (int i = hardMin; i <= hardMax; i++)
                {
                    points.Add(EnumToData.Instance.DistanceRule(i));
                }
                successRadius = 10;
                break;
        }
    }

    
    public void AddPointEachRound()
    {
        // 고정 모드일 때만 룰이 다르다.
        if (GameOption.Instance.distanceMethod == (int)DistanceMethod.FIXED)
        {
            // 매 라운드마다 적힌 거리를 포인트에 추가
            string stringToFloat = targetDistance.GetComponentInChildren<TextMeshProUGUI>().text;
            // 목표 거리 : 000cm (7글자 삭제 후 cm를 공백으로)                      
            points.Add(float.Parse(stringToFloat.Substring(7).Replace("cm", "")));
        }        

        if (points[pointIndex] >= 0f && points[pointIndex] < 100f)
        {
            GameOption.Instance.tranningCountForM[0]++;
            if (GameOption.Instance.isCountUp)
            {
                GameOption.Instance.successCountForM[0]++;
            }
        }
        else if (points[pointIndex] >= 100f && points[pointIndex] < 200f)
        {
            GameOption.Instance.tranningCountForM[1]++;
            if (GameOption.Instance.isCountUp)
            {
                GameOption.Instance.successCountForM[1]++;
            }
        }
        else if (points[pointIndex] >= 200f && points[pointIndex] < 300f)
        {
            GameOption.Instance.tranningCountForM[2]++;
            if (GameOption.Instance.isCountUp)
            {
                GameOption.Instance.successCountForM[2]++;
            }
        }
        else if (points[pointIndex] >= 300f && points[pointIndex] < 400f)
        {
            GameOption.Instance.tranningCountForM[3]++;
            if (GameOption.Instance.isCountUp)
            {
                GameOption.Instance.successCountForM[3]++;
            }
        }
        pointIndex++;
        //Debug.Log(pointIndex + "/ " + point[pointIndex]);
        if (pointIndex > points.Count - 1)
        {
            pointIndex = 0;
        }
        
        GameOption.Instance.isCountUp = false;
    }
}
