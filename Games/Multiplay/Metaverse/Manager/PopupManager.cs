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
    GameObject oneButtonPopupPrefab;
    [SerializeField]
    GameObject twoButtonPopupPrefab;

    GameObject oneButtonPopup;
    GameObject twoButtonPopup;
    Animator animatorOne;
    Animator animatorTwo;
    TextMeshProUGUI warningText;    
    Button[] buttons;
    Image background;
    Transform canvas;

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

    void Start()
    {
        canvas = GameObject.Find("Canvas").transform;
    }

    public void ShowOneButtnPopup(string msg, Action onCloseCallback = null)
    {
        if (oneButtonPopup == null)
        {            
            oneButtonPopup = Instantiate(oneButtonPopupPrefab, canvas);
            oneButtonPopup.transform.position = new Vector3 (2000f, 0, 0);
            animatorOne = oneButtonPopup.GetComponent<Animator>();            
        }
        else
        {
            oneButtonPopup.SetActive(true);
        }
        animatorOne.SetBool("isShow", true);
        StartCoroutine(OnFadeIn(oneButtonPopup));

        warningText = oneButtonPopup.GetComponentInChildren<TextMeshProUGUI>();
        warningText.text = msg;

        buttons = oneButtonPopup.GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() =>
        {
            animatorOne.SetBool("isShow", false);
            StartCoroutine(OnFadeOut(oneButtonPopup));

            if (onCloseCallback != null)
            {
                onCloseCallback.Invoke();
            }
        });
    }

    // �Ķ���� (�޽���, ��ҹ�ư, Ȯ�ι�ư)
    public void ShowTwoButtnPopup(string msg, Action onCloseCallback = null, Action onCloseCallbackSecond = null)
    {
        if (twoButtonPopup == null)
        {
            twoButtonPopup = Instantiate(twoButtonPopupPrefab, canvas);
            oneButtonPopup.transform.position = new Vector3(2000f, 0, 0);
            animatorTwo = twoButtonPopup.GetComponent<Animator>();
        }
        else
        {
            twoButtonPopup.SetActive(true);
        }
        animatorTwo.SetBool("isShow", true);
        StartCoroutine(OnFadeIn(twoButtonPopup));

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
            temp.a -= Time.deltaTime * 0.7f;
            background.color = temp;
            yield return null;
        }

        temp.a = 0;
        background.color = temp;

        popup.SetActive(false);
    }

    IEnumerator OnFadeIn(GameObject popup)
    {
        background = popup.GetComponent<Image>();
        Color temp = background.color;

        yield return new WaitForSeconds(0.1f);

        while (background.color.a < 1)
        {
            temp.a += Time.deltaTime * 0.7f;
            background.color = temp;
            yield return null;
        }

        temp.a = 1;
        background.color = temp;
    }
}
