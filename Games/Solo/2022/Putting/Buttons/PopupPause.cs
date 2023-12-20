using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPause : MonoBehaviour
{
    public void OnPopupPause()
    {
        PopupManager.Instance.pausePopup.SetActive(true);
    }
}
