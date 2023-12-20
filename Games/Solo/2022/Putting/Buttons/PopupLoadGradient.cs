using UnityEngine;

public class PopupLoadGradient : MonoBehaviour
{
    public void OnLoadGradientPopup()
    {
        PopupManager.Instance.loadGradientPopup.SetActive(true);
        LoadRecord.Instance.Load();
    }
}
