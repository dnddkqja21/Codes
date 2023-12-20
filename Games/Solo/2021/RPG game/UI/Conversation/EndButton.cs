using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndButton : MonoBehaviour
{
    public GameObject panel;

    public void ExitPanel()
    {
        panel.SetActive(false);
    }
}
