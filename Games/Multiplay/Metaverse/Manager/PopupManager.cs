using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 어느 씬에서나 공통으로 사용되는 팝업창 UI관리
/// </summary>

public class PopupManager : MonoBehaviour
{
    static PopupManager instance = null;
    public static PopupManager Instance { get { return instance; } }

    [SerializeField]
    GameObject oneButtonPopupPrefab;
    [SerializeField]
    GameObject twoButtonPopupPrefab;

    GameObject oneButtonPopup;
    GameObject twoButtonPopup;
    TextMeshProUGUI warningText;    
    Button[] buttons;
    Image background;

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
        if (oneButtonPopup == null)
        {            
            oneButtonPopup = Instantiate(oneButtonPopupPrefab, GameObject.Find("Canvas").transform);
        }
        else
        {
            oneButtonPopup.SetActive(true);
            Color temp = background.color;
            temp.a = 1;
            background.color = temp;
        }

        warningText = oneButtonPopup.GetComponentInChildren<TextMeshProUGUI>();
        warningText.text = msg;

        buttons = oneButtonPopup.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() =>
        {
            StartCoroutine(OnFadeOut(oneButtonPopup));

            if (onCloseCallback != null)
            {
                onCloseCallback.Invoke();
            }
        });
    }

    // 파라미터 (메시지, 취소버튼, 확인버튼)
    public void ShowTwoButtnPopup(string msg, Action onCloseCallback = null, Action onCloseCallbackSecond = null)
    {
        if (twoButtonPopup == null)
        {
            twoButtonPopup = Instantiate(twoButtonPopupPrefab, GameObject.Find("Canvas").transform);
        }
        else
        {
            twoButtonPopup.SetActive(true);
        }
        
        warningText = twoButtonPopup.GetComponentInChildren<TextMeshProUGUI>();      
        warningText.text = msg;

        buttons = twoButtonPopup.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() =>
        {
            StartCoroutine(OnFadeOut(twoButtonPopup));

            if (onCloseCallback != null)
            {
                onCloseCallback.Invoke();
            }
        });
        buttons[1].onClick.AddListener(() =>
        {
            StartCoroutine(OnFadeOut(twoButtonPopup));

            if (onCloseCallbackSecond != null)
            {
                onCloseCallbackSecond.Invoke();
            }
        });
    }

    IEnumerator OnFadeOut(GameObject popup)
    {
        background = popup.GetComponent<Image>();
        Color temp = background.color;

        temp.a = 1;
        background.color = temp;

        yield return new WaitForSeconds(0.1f);


        while (background.color.a > 0)
        {
            temp.a -= Time.deltaTime * 1f;
            background.color = temp;
            yield return null;
        }

        temp.a = 0;
        background.color = temp;

        popup.SetActive(false);
    }
}
