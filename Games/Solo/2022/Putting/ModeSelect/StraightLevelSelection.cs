using UnityEngine;
using TMPro;

public class StraightLevelSelection : MonoBehaviour
{
    [Header("���� �Ʒ� �ܰ�")]
    public StraightLevel straightLevel;
    [Header("���� �ܰ� �ؽ�Ʈ")]
    public TextMeshProUGUI levelInfo;

    private void Start()
    {
        levelInfo.text = EnumToData.Instance.StraightTitleToKor((int)straightLevel);
    }
    // ���� �Ʒ� �ܰ�
    public void OnStraightLevelSelection()
    {
        GameOption.Instance.straightLevel = (int)straightLevel;
    }
}
