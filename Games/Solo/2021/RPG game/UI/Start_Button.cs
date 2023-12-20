using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Start_Button : MonoBehaviour
{
    public Image startScene;
    public Image settingButton;
    public Image gameName;
    public GameObject UserInfo;
    public GameObject joystick;
    public GameObject loadingScene;
    public GameObject actionUI;

    Color originColor;
    Color originColor2;
    void Start()
    {
        originColor = startScene.color;
        originColor.a = 1f;

        originColor2 = gameName.color;
        originColor2.a = 1f;
    }

    void Update()
    {
        
    }

    public void OnStartButton()
    {
        InvokeRepeating("MinusAlpha", 1f, 0.01f);
        Invoke("StartGame", 4f);
        gameObject.SetActive(false);
        Invoke("LoadingScene", 0.7f);
    }

    public void MinusAlpha()
    {
        originColor.a -= Time.deltaTime;
        startScene.color = originColor;
        if (originColor.a <= 0)
        {
            CancelInvoke("MinusAlpha");
        }

        originColor2.a -= Time.deltaTime;
        gameName.color = originColor2;
        if (originColor2.a <= 0)
        {
            CancelInvoke("MinusAlpha");
        }
    }

    public void StartGame()
    {
        startScene.gameObject.SetActive(false);
        settingButton.gameObject.SetActive(true);
        UserInfo.SetActive(true);
        joystick.SetActive(true);
        actionUI.SetActive(true);
    }

    void LoadingScene()
    {
        loadingScene.SetActive(true);
    }
}
