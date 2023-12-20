using UnityEngine;
using TMPro;

public class TranningInformation : MonoBehaviour
{
    [Header("훈련 제목")]
    public TextMeshProUGUI title;
    [Header("훈련 내용")]
    public TextMeshProUGUI content;

    private void OnEnable()
    {
        var option = GameOption.Instance;
        var data = EnumToData.Instance;

        switch(option.tranningMode)
        {
            case (int)TranningMode.STRAIGHT:
                // 훈련 제목
                title.text = data.StraightTitleToKor(option.straightLevel);
                // 훈련 내용
                content.text = data.StraightInfoToKor(option.straightLevel);
                break;

            case (int)TranningMode.DISTANCE:
                title.text = data.TranningModeToKor(option.tranningMode) + " [난이도 : " + data.DistanceLevelToKor(option.distanceLevel) + "]";
                content.text = "훈련 방식 : " + data.DistanceMethodToKor(option.distanceMethod) + " [" + data.DistanceInfoToKor(option.distanceMethod) + "]";
                break;

            case (int)TranningMode.GRADIENT:
                string temp = data.GradientTitleToKor(option.gradientCondition);
                string tempTitle = temp.Substring(0, 3);
                string tempContent = temp.Substring(6);

                title.text = data.TranningModeToKor(option.tranningMode) + " [" + tempTitle + "]";
                content.text = tempContent;
                break;

            case (int)TranningMode.ACTUAL:
                title.text = data.TranningModeToKor(option.tranningMode);
                content.text    = "컵 지정 : " + "[" + data.ActualCupToKor(option.actualCupPoint) + "]"
                                + "     " + "시작 지점 : " + "[" + data.ActualStartToKor(option.actualStartPoint) + "]"
                                + "     " + "기울기 : " + "[" + data.ActualGradientToKor(option.actualGradient) + "]";
                break;
        }
    }
}
