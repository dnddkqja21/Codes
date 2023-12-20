using UnityEngine;

public class TranningGreenButtonInteraction : MonoBehaviour
{
    public void ToGreen()
    {
        if(GameOption.Instance.isTranning)
        {
            KioskPanelManager.Instance.greenSetting.SetActive(true);
            KioskPanelManager.Instance.tranningDisplay.SetActive(false);
        }
        else
        {
            KioskPanelManager.Instance.greenSetting.SetActive(true);
            KioskPanelManager.Instance.tranningSetting.SetActive(false);
        }
    }

    public void ToTranning()
    {
        if(GameOption.Instance.isTranning)
        {
            KioskPanelManager.Instance.tranningDisplay.SetActive(true);
            KioskPanelManager.Instance.greenSetting.SetActive(false);
        }
        else
        {
            KioskPanelManager.Instance.tranningSetting.SetActive(true);
            KioskPanelManager.Instance.greenSetting.SetActive(false);
        }
    }
}
