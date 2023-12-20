using UnityEngine;

public class TranningModeSelection : MonoBehaviour
{
    [Header("훈련 모드")]
    public TranningMode tranningMode;    

    // 트레이닝 모드
    public void OnTranningModeSelection()
    {        
        GameOption.Instance.tranningMode = (int)tranningMode;
        KioskPanelManager.Instance.tranningTitle.text = EnumToData.Instance.TranningModeToKor(GameOption.Instance.tranningMode) + " 설정";
    }    
}
