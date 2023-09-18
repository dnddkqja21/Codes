using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��� �������� �������� ���Ǵ� �˾�â UI����
/// </summary>

public class PopupManager : MonoBehaviour
{
    static PopupManager instance = null;
    public static PopupManager Instance { get { return instance; } }

    [SerializeField]
    GameObject oneButtonPopup;
    [SerializeField]
    GameObject twoButtonPopup;

    TextMeshProUGUI warningText;
    Button okBtn;
    Button[] buttons;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ShowOneButtnPopup(string msg, Action onCloseCallback = null)
    {
        GameObject temp = Instantiate(oneButtonPopup, GameObject.Find("Canvas").transform);
        warningText = temp.GetComponentInChildren<TextMeshProUGUI>();
        temp.SetActive(true);
        warningText.text = msg;

        okBtn = temp.GetComponentInChildren<Button>();
        okBtn.onClick.AddListener(() =>
        {
            Destroy(temp);

            if (onCloseCallback != null)
            {
                onCloseCallback.Invoke();
            }
        });
    }

    // �Ķ���� (�޽���, ��ҹ�ư, Ȯ�ι�ư)
    public void ShowTwoButtnPopup(string msg, Action onCloseCallback = null, Action onCloseCallbackSecond = null)
    {
        GameObject temp = Instantiate(twoButtonPopup, GameObject.Find("Canvas").transform);
        warningText = temp.GetComponentInChildren<TextMeshProUGUI>();
        temp.SetActive(true);
        warningText.text = msg;

        buttons = temp.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() =>
        {
            Destroy(temp);

            if (onCloseCallback != null)
            {
                onCloseCallback.Invoke();
            }
        });
        buttons[1].onClick.AddListener(() =>
        {
            Destroy(temp);

            if (onCloseCallbackSecond != null)
            {
                onCloseCallbackSecond.Invoke();
            }
        });
    }
}
