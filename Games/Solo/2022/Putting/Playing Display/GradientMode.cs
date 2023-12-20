using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GradientMode : MonoBehaviour
{
    [Header("타이틀 텍스트")]
    public TextMeshProUGUI title;    

    private void Start()
    {
        title.text = EnumToData.Instance.GradientEachInfoToKor(GameOption.Instance.gradientCondition) + " " + GameOption.Instance.GradientLevel.ToString() + "레벨";
    }

    public void InitRound()
    {
        // 현재 레벨 별 훈련 카운트를 기록, 성공 여부도 기록
        switch(GameOption.Instance.GradientLevel)
        {
            case (int)GradientLevel.ONE:
                GameOption.Instance.tranningCountForL[(int)GradientLevel.ONE -1]++;
                if(GameOption.Instance.isCountUp)
                {
                    GameOption.Instance.successCountForL[(int)GradientLevel.ONE - 1]++;
                }
                
                break;

            case (int)GradientLevel.TWO:
                GameOption.Instance.tranningCountForL[(int)GradientLevel.TWO - 1]++;
                if (GameOption.Instance.isCountUp)
                {
                    GameOption.Instance.successCountForL[(int)GradientLevel.TWO - 1]++;
                }
                break;

            case (int)GradientLevel.THREE:
                GameOption.Instance.tranningCountForL[(int)GradientLevel.THREE - 1]++;
                if (GameOption.Instance.isCountUp)
                {
                    GameOption.Instance.successCountForL[(int)GradientLevel.THREE - 1]++;
                }
                break;

            case (int)GradientLevel.FOUR:
                GameOption.Instance.tranningCountForL[(int)GradientLevel.FOUR - 1]++;
                if (GameOption.Instance.isCountUp)
                {
                    GameOption.Instance.successCountForL[(int)GradientLevel.FOUR - 1]++;
                }
                break;

            case (int)GradientLevel.FIVE:
                GameOption.Instance.tranningCountForL[(int)GradientLevel.FIVE - 1]++;
                if (GameOption.Instance.isCountUp)
                {
                    GameOption.Instance.successCountForL[(int)GradientLevel.FIVE - 1]++;
                }
                break;

        }
        GameOption.Instance.isCountUp = false;
    }

    public void OnLevelUp()
    {
        GameOption.Instance.GradientLevel++;        
        title.text = EnumToData.Instance.GradientEachInfoToKor(GameOption.Instance.gradientCondition) + " " + GameOption.Instance.GradientLevel.ToString() + "레벨";
    }
    public void OnLevelDown()
    {
        GameOption.Instance.GradientLevel--;
        title.text = EnumToData.Instance.GradientEachInfoToKor(GameOption.Instance.gradientCondition) + " " + GameOption.Instance.GradientLevel.ToString() + "레벨";
    }
}
