using UnityEngine;

public class PopupResetGradient : MonoBehaviour
{
    public void OnResetGradientPopup()
    {
        PopupManager.Instance.resetGradientPopup.SetActive(true);
    }
}
