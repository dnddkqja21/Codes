using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestTranning : MonoBehaviour
{
    [Header("진행 수")]
    public TextMeshProUGUI progressCount;
    [Header("성공 수")]
    public TextMeshProUGUI successCount;
    [Header("훈련 모드")]
    public GameObject[] tranningModes;

    public void OnTestTranning()
    {
        // 공통 영역
        GameOption.Instance.progressCount++;
        progressCount.text = GameOption.Instance.progressCount.ToString();

        int success = Random.Range(0, 2);
        if(success == 1)
        {
            GameOption.Instance.successCount++;
            GameOption.Instance.isCountUp = true;
            successCount.text = GameOption.Instance.successCount.ToString();
        }

        // 거리 훈련 이전 코드(캡슐화 고려, 거리 훈련으로 옮김)
        #region
        //if(GameOption.Instance.tranningMode == (int)TranningMode.DISTANCE)
        //{     
        //    // 거리 모드의 포인트를 가져옴
        //    var distanceMode = tranningModes[(int)TranningMode.DISTANCE - 1].GetComponent<DistanceMode>();
        //    var point = distanceMode.points;

        //    // 매 라운드마다 적힌 거리를 포인트에 추가
        //    string stringToFloat = distanceMode.targetDistance.GetComponentInChildren<TextMeshProUGUI>().text;
        //    // 목표 거리 : 000cm (7글자 삭제 후 cm를 공백으로)                      
        //    point.Add(float.Parse(stringToFloat.Substring(7).Replace("cm", "")));

        //    if(point[pointIndex] >= 0f && point[pointIndex] < 100f)
        //    {
        //        GameOption.Instance.tranningCountForM[0]++;
        //        if(GameOption.Instance.isCountUp)
        //        {
        //            GameOption.Instance.successCountForM[0]++;
        //        }                
        //    }
        //    else if(point[pointIndex] >= 100f && point[pointIndex] < 200f)
        //    {
        //        GameOption.Instance.tranningCountForM[1]++;
        //        if (GameOption.Instance.isCountUp)
        //        {
        //            GameOption.Instance.successCountForM[1]++;
        //        }                
        //    }
        //    else if(point[pointIndex] >= 200f && point[pointIndex] < 300f)
        //    {
        //        GameOption.Instance.tranningCountForM[2]++;
        //        if (GameOption.Instance.isCountUp)
        //        {
        //            GameOption.Instance.successCountForM[2]++;
        //        }
        //    }
        //    else if(point[pointIndex] >= 300f && point[pointIndex] < 400f)
        //    {
        //        GameOption.Instance.tranningCountForM[3]++;
        //        if (GameOption.Instance.isCountUp)
        //        {
        //            GameOption.Instance.successCountForM[3]++;
        //        }
        //    }
        //    pointIndex++;
        //    //Debug.Log(pointIndex + "/ " + point[pointIndex]);
        //    if(pointIndex > point.Count -1)
        //    {
        //        pointIndex = 0;
        //    }
        //}
        //GameOption.Instance.isCountUp = false;
        #endregion
        
        if(GameOption.Instance.selectedMode != (int)GameMode.FREE)
        {
            switch(GameOption.Instance.tranningMode)
            {
                case (int)TranningMode.STRAIGHT:                    
                    tranningModes[(int)TranningMode.STRAIGHT -1].GetComponent<StraightMode>().InitRound();
                    break;
                case (int)TranningMode.DISTANCE:
                    tranningModes[(int)TranningMode.DISTANCE - 1].GetComponent<DistanceMode>().AddPointEachRound();
                    tranningModes[(int)TranningMode.DISTANCE -1].GetComponent<DistanceMode>().InitRound();
                    break;
                case (int)TranningMode.GRADIENT:
                    tranningModes[(int)TranningMode.GRADIENT - 1].GetComponent<GradientMode>().InitRound();
                    break;
                case (int)TranningMode.ACTUAL:
                    tranningModes[(int)TranningMode.ACTUAL - 1].GetComponent<ActualMode>().InitRound();
                    break;
            }
        }


        // 훈련 마무리
        if (GameOption.Instance.progressCount == GameOption.Instance.TranningCount)
        {
            //Debug.Log("훈련 끝");
            // 자유 훈련이 아닐 때 기록을 저장
            if(GameOption.Instance.selectedMode != (int)GameMode.FREE)
            {
                ResultRecord result = new ResultRecord();
                GameOption.Instance.recordList.Add(result);
            }

            PopupManager.Instance.tranningResultPopup.SetActive(true);
            return;
        }
    }
}
