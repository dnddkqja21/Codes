using UnityEngine;
using TMPro;

public class SeeMore : MonoBehaviour
{
    [Header("일반 / 상세 보기 텍스트")]
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
            seeMoreText.text = "일반 보기";
            RecordManager.Instance.seeMoreModePopups[RecordManager.Instance.infoPopupMode -1].SetActive(true);
        }
        else
        {
            seeMoreText.text = "상세 보기";
            RecordManager.Instance.normalModePopups[RecordManager.Instance.infoPopupMode -1].SetActive(true);
        }

        RecordManager.Instance.isSeeMore = isSeeMore;
    }
}
