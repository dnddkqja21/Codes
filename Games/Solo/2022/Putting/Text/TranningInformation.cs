using UnityEngine;
using TMPro;

public class TranningInformation : MonoBehaviour
{
    [Header("�Ʒ� ����")]
    public TextMeshProUGUI title;
    [Header("�Ʒ� ����")]
    public TextMeshProUGUI content;

    private void OnEnable()
    {
        var option = GameOption.Instance;
        var data = EnumToData.Instance;

        switch(option.tranningMode)
        {
            case (int)TranningMode.STRAIGHT:
                // �Ʒ� ����
                title.text = data.StraightTitleToKor(option.straightLevel);
                // �Ʒ� ����
                content.text = data.StraightInfoToKor(option.straightLevel);
                break;

            case (int)TranningMode.DISTANCE:
                title.text = data.TranningModeToKor(option.tranningMode) + " [���̵� : " + data.DistanceLevelToKor(option.distanceLevel) + "]";
                content.text = "�Ʒ� ��� : " + data.DistanceMethodToKor(option.distanceMethod) + " [" + data.DistanceInfoToKor(option.distanceMethod) + "]";
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
                content.text    = "�� ���� : " + "[" + data.ActualCupToKor(option.actualCupPoint) + "]"
                                + "     " + "���� ���� : " + "[" + data.ActualStartToKor(option.actualStartPoint) + "]"
                                + "     " + "���� : " + "[" + data.ActualGradientToKor(option.actualGradient) + "]";
                break;
        }
    }
}
