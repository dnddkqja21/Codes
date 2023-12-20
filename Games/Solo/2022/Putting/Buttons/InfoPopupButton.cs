using UnityEngine;

public class InfoPopupButton : MonoBehaviour
{
    [Header("�˾� ���")]
    public InfoPopupMode infoPopupMode;
    
    public void SetInfoMode()
    {
        RecordManager.Instance.infoPopupMode = (int)infoPopupMode;

        RecordManager.Instance.OnShowRecord();
    }
}
