using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorPopup : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Hide", 2f);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
