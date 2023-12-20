using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateResult : MonoBehaviour
{
    [Header("훈련 결과 프리펩")]
    public GameObject[] resultPrefabs;

    // 직선 훈련 
    public List<ResultRecord> straightRecord = new List<ResultRecord>();
    List<StraightResult> straightResults =  new List<StraightResult>();

    // 거리 훈련
    public List<ResultRecord> distanceRecord = new List<ResultRecord>();
    List<DistanceResult> distanceResults = new List<DistanceResult>();

    // 기울기 훈련
    public List<ResultRecord> gradientRecord = new List<ResultRecord>();
    List<GradientResult> gradientResults = new List<GradientResult>();

    // 실전 훈련
    public List<ResultRecord> actualRecord = new List<ResultRecord>();
    List<ActualResult> actualResults = new List<ActualResult>();

    GridLayoutGroup layout;
    // 모드 별 레이아웃 사이즈
    float standardWidth = 872;
    float straightModeHeight = 120;
    float distanceModeHeight = 200;
    float gradientModeHeight = 220;
    float actualModeWidth = 434;
    float actualModeHeight = 170;

    void Start()
    {
        // 자유 훈련이면 리턴
        if(GameOption.Instance.selectedMode == (int)GameMode.FREE)
        {
            return;
        }

        layout = GetComponent<GridLayoutGroup>();        

        switch (GameOption.Instance.tranningMode)
        {
            case (int)TranningMode.STRAIGHT:
                // 레이아웃 사이즈 변경
                layout.cellSize = new Vector2(standardWidth, straightModeHeight);

                // 직선 훈련만 가져옴
                for (int i = 0; i < GameOption.Instance.recordList.Count; i++)
                {
                    if (GameOption.Instance.recordList[i].tranningMode == (int)TranningMode.STRAIGHT)
                    {
                        straightRecord.Add(GameOption.Instance.recordList[i]);
                    }
                }
                // 최근 기록이 위에 뜨도록 함
                straightRecord.Reverse();

                for (int i = 0; i < straightRecord.Count; i++)
                {
                    var straightResult = Instantiate(resultPrefabs[(int)TranningMode.STRAIGHT - 1], gameObject.transform);
                    straightResults.Add(straightResult.GetComponentInChildren<StraightResult>());
                    
                    straightResults[i].level.text = straightRecord[i].level + "단계";
                    straightResults[i].distance.text = straightRecord[i].distance + "cm";
                    if(straightRecord[i].distance == "cup to hole")
                    {
                        straightResults[i].distance.text = "Cup to Hole";
                    }
                    straightResults[i].height.text = straightRecord[i].height + "cm";
                    straightResults[i].tranningCount.text = straightRecord[i].tranningCount + "개";
                    straightResults[i].successCount.text = straightRecord[i].successCount + "개";
                    straightResults[i].successRate.text = straightRecord[i].successRate + "%";
                }
                break;

            case (int)TranningMode.DISTANCE:
                layout.cellSize = new Vector2(standardWidth, distanceModeHeight);

                for (int i = 0; i < GameOption.Instance.recordList.Count; i++)
                {
                    if (GameOption.Instance.recordList[i].tranningMode == (int)TranningMode.DISTANCE)
                    {
                        distanceRecord.Add(GameOption.Instance.recordList[i]);
                    }
                }
                
                distanceRecord.Reverse();

                for (int i = 0; i < distanceRecord.Count; i++)
                {
                    var distanceResult = Instantiate(resultPrefabs[(int)TranningMode.DISTANCE - 1], gameObject.transform);
                    distanceResults.Add(distanceResult.GetComponentInChildren<DistanceResult>());
                    
                    distanceResults[i].level.text = distanceRecord[i].level;
                    distanceResults[i].method.text = distanceRecord[i].distanceMethod;

                    for (int j = 0; j < distanceResults[i].tranningCountForM.Length; j++)
                    {
                        distanceResults[i].tranningCountForM[j].text = distanceRecord[i].tranningCountForM[j] + "개";
                    }

                    for (int j = 0; j < distanceResults[i].successCountForM.Length; j++)
                    {
                        distanceResults[i].successCountForM[j].text = distanceRecord[i].successCountForM[j] + "개";
                    }

                    for (int j = 0; j < distanceResults[i].successRateForM.Length; j++)
                    {
                        distanceResults[i].successRateForM[j].text = distanceRecord[i].successRateForM[j] + "%"; 
                    }
                }
                
                break;

            case (int)TranningMode.GRADIENT:
                layout.cellSize = new Vector2(standardWidth, gradientModeHeight);

                for (int i = 0; i < GameOption.Instance.recordList.Count; i++)
                {
                    if (GameOption.Instance.recordList[i].tranningMode == (int)TranningMode.GRADIENT)
                    {
                        gradientRecord.Add(GameOption.Instance.recordList[i]);
                    }
                }

                gradientRecord.Reverse();

                for (int i = 0; i < gradientRecord.Count; i++)
                {
                    var gradientResult = Instantiate(resultPrefabs[(int)TranningMode.GRADIENT - 1], gameObject.transform);
                    gradientResults.Add(gradientResult.GetComponentInChildren<GradientResult>());

                    gradientResults[i].condition.text = gradientRecord[i].condition;

                    for (int j = 0; j < gradientResults[i].tranningCountForL.Length; j++)
                    {
                        gradientResults[i].tranningCountForL[j].text = gradientRecord[i].tranningCountForL[j] + "개";
                    }
                    for (int j = 0; j < gradientResults[i].successCountForL.Length; j++)
                    {
                        gradientResults[i].successCountForL[j].text = gradientRecord[i].successCountForL[j] + "개";
                    }
                    for (int j = 0; j < gradientResults[i].successRateForL.Length; j++)
                    {
                        gradientResults[i].successRateForL[j].text = gradientRecord[i].successRateForL[j] + "%";
                    }
                }
                break;

            case (int)TranningMode.ACTUAL:
                layout.cellSize = new Vector2(actualModeWidth, actualModeHeight);                

                for (int i = 0; i < GameOption.Instance.recordList.Count; i++)
                {
                    if (GameOption.Instance.recordList[i].tranningMode == (int)TranningMode.ACTUAL)
                    {
                        actualRecord.Add(GameOption.Instance.recordList[i]);
                    }
                }

                actualRecord.Reverse();

                for (int i = 0; i < actualRecord.Count; i++)
                {
                    var actualResult = Instantiate(resultPrefabs[(int)TranningMode.ACTUAL - 1], gameObject.transform);
                    actualResults.Add(actualResult.GetComponentInChildren<ActualResult>());

                    actualResults[i].actualCupPoint.text = actualRecord[i].actualCupPoint;
                    actualResults[i].actualStartPoint.text = actualRecord[i].actualStartPoint;
                    actualResults[i].actualGradient.text = actualRecord[i].actualGradient;
                    actualResults[i].tranningCount.text = actualRecord[i].tranningCount + "개";
                    actualResults[i].successCount.text = actualRecord[i].successCount + "개";
                    actualResults[i].successRate.text = actualRecord[i].successRate + "%";
                }
                break;
        }
    }
}
