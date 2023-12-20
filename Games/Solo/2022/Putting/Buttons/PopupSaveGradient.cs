using UnityEngine;

public class PopupSaveGradient : MonoBehaviour
{
    public void OnSaveGradientPopup()
    {
        PopupManager.Instance.saveGradientPopup.SetActive(true);
    }
}
