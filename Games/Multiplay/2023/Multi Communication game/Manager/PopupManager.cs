using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour  
{   
    static PopupManager instance = null;
    public static PopupManager Instance { get { return instance; } }

    [SerializeField]
    GameObject oneButtonPopup;
    [SerializeField]
    GameObject twoButtonPopup;
    [SerializeField]
    GameObject noButtonPopup;
    [SerializeField]
    GameObject inputPopup;
    TextMeshProUGUI warningText;
    Button okBtn;
    Button[] buttons;
    public string pass;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ShowOneButtnPopup(string msg , Action onCloseCallback = null)
    {
        GameObject temp = Instantiate(oneButtonPopup, GameObject.Find("Canvas").transform);
        warningText = temp.GetComponentInChildren<TextMeshProUGUI>();
        temp.SetActive(true);
        warningText.text = msg;

        okBtn = temp.GetComponentInChildren<Button>();
        okBtn.onClick.AddListener(() =>
        {
            DestroyPopup(temp);

            if (onCloseCallback != null)
            {
                onCloseCallback.Invoke();
            }
        });
    }

    // 파라미터 (메시지, 취소버튼, 확인버튼)
    public void ShowTwoButtnPopup(string msg, Action onCloseCallback = null, Action onCloseCallbackSecond = null)
    {
        GameObject temp = Instantiate(twoButtonPopup, GameObject.Find("Canvas").transform);
        warningText = temp.GetComponentInChildren<TextMeshProUGUI>();
        temp.SetActive(true);
        warningText.text = msg;

        buttons = temp.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() =>
        {
            DestroyPopup(temp);

            if (onCloseCallback != null)
            {
                onCloseCallback.Invoke();
            }
        });
        buttons[1].onClick.AddListener(() =>
        {
            DestroyPopup(temp);

            if (onCloseCallbackSecond != null)
            {
                onCloseCallbackSecond.Invoke();
            }
        });
    }

    public void ShowNoButtnPopup(string msg, Action onCloseCallback = null)
    {
        GameObject temp = Instantiate(noButtonPopup, GameObject.Find("Canvas").transform);
        warningText = temp.GetComponentInChildren<TextMeshProUGUI>();
        temp.SetActive(true);
        warningText.text = msg;   
        
        // 버튼 없애는 규칙?
    }

    // 파라미터 (메시지, 취소버튼, 확인버튼)
    public void ShowInputPopup(string msg, Action onCloseCallback = null, Action onCloseCallbackSecond = null)
    {
        GameObject temp = Instantiate(inputPopup, GameObject.Find("Canvas").transform);
        warningText = temp.GetComponentInChildren<TextMeshProUGUI>();
        temp.SetActive(true);
        warningText.text = msg;
        
        buttons = temp.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() =>
        {
            DestroyPopup(temp);

            if (onCloseCallback != null)
            {
                onCloseCallback.Invoke();
            }
        });
        buttons[1].onClick.AddListener(() =>
        {
            pass = temp.GetComponentInChildren<TMP_InputField>().text;
            DestroyPopup(temp);

            if (onCloseCallbackSecond != null)
            {
                onCloseCallbackSecond.Invoke();
            }
        });
    }

    void DestroyPopup(GameObject obj)
    {
        Destroy(obj.gameObject);
    }
}

