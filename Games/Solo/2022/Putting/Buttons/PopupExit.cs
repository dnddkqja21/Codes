using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupExit : MonoBehaviour
{
    public void OnExitPopup()
    {
        PopupManager.Instance.exitPopup.SetActive(true);
    }
}
