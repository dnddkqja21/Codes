using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Setting_Button : MonoBehaviour
{
    public Image SettingWindow;

    ActionController player;

    private void Start()
    {
        player = FindObjectOfType<ActionController>();
    }

    public void WindowOn()
    {
        player.PlayClips(2);
        SettingWindow.gameObject.SetActive(true);
    }
}
