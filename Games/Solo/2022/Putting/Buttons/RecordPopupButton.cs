using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordPopupButton : MonoBehaviour
{
    public void OnRecordPopup()
    {
        PopupManager.Instance.recordPopup.SetActive(true);
    }
}
