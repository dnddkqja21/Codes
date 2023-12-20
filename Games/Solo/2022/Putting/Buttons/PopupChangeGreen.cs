using UnityEngine;

public class PopupChangeGreen : MonoBehaviour
{
    public void OnChangeGreenPopup()
    {
        PopupManager.Instance.changeGreenPopup.SetActive(true);
    }
}
