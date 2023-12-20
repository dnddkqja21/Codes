using UnityEngine;

public class CancelGradientButton : MonoBehaviour
{
    public void CancelButton()
    {
        var manager = SaveRecord.Instance;
        manager.inputField.text = "";
        manager.caution.enabled = false;

        PopupManager.Instance.saveGradientPopup.SetActive(false);
    }
}
