using UnityEngine;
using TMPro;

public class SeeMore : MonoBehaviour
{
    [Header("�Ϲ� / �� ���� �ؽ�Ʈ")]
    public TextMeshProUGUI seeMoreText;

    public void SeeMoreToNormal()
    {
        for (int i = 0; i < RecordManager.Instance.normalModePopups.Length; i++)
        {
            RecordManager.Instance.normalModePopups[i].SetActive(false);
            RecordManager.Instance.seeMoreModePopups[i].SetActive(false);
        }

        var isSeeMore = RecordManager.Instance.isSeeMore;
        isSeeMore = !isSeeMore;
        if(isSeeMore)
        {
            seeMoreText.text = "�Ϲ� ����";
            RecordManager.Instance.seeMoreModePopups[RecordManager.Instance.infoPopupMode -1].SetActive(true);
        }
        else
        {
            seeMoreText.text = "�� ����";
            RecordManager.Instance.normalModePopups[RecordManager.Instance.infoPopupMode -1].SetActive(true);
        }

        RecordManager.Instance.isSeeMore = isSeeMore;
    }
}
