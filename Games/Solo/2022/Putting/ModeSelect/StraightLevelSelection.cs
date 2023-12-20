using UnityEngine;
using TMPro;

public class StraightLevelSelection : MonoBehaviour
{
    [Header("직선 훈련 단계")]
    public StraightLevel straightLevel;
    [Header("직선 단계 텍스트")]
    public TextMeshProUGUI levelInfo;

    private void Start()
    {
        levelInfo.text = EnumToData.Instance.StraightTitleToKor((int)straightLevel);
    }
    // 직선 훈련 단계
    public void OnStraightLevelSelection()
    {
        GameOption.Instance.straightLevel = (int)straightLevel;
    }
}
