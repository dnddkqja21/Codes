using UnityEngine;

public class TranningModeSelection : MonoBehaviour
{
    [Header("�Ʒ� ���")]
    public TranningMode tranningMode;    

    // Ʈ���̴� ���
    public void OnTranningModeSelection()
    {        
        GameOption.Instance.tranningMode = (int)tranningMode;
        KioskPanelManager.Instance.tranningTitle.text = EnumToData.Instance.TranningModeToKor(GameOption.Instance.tranningMode) + " ����";
    }    
}
