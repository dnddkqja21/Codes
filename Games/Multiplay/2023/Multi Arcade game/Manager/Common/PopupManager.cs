using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField]
    GameObject tutorialPopupPrefab;

    const float Origin_Position = 2000f;
    const float Origin_Width = 700;
    const float Setting_Width = 1050f;
    const float Fade_Speed = 0.7f;

    GameObject oneButtonPopup;
    GameObject twoButtonPopup;
    GameObject untouchable;
    RectTransform oneButtonRectTransform;
    RectTransform twoButtonRectTransform;
    Animator animatorOneButton;
    Animator animatorTwoButton;
    TextMeshProUGUI popupText;
    Button oneButton;
    Button[] twoButtons;
    TextMeshProUGUI confirmText;
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

    // DontDestroyOnLoad 오브젝트의 경우 씬 체인지 이후에 Start함수가 호출되지 않기 때문에 씬 매니저를 이용하여 직접 초기화 해야 한다.
    void OnEnable()
    {
        // 씬 전환 이벤트에 이벤트 핸들러 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬 전환 시 호출되는 이벤트 핸들러
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Init)
        {
            canvas = UIManagerInit.Instance.canvas;
        }
        else if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.World)
        {
            canvas = UIManagerWorld.Instance.canvas;
        }
        untouchable = canvas.Find("Untouchable").gameObject;
        untouchable.SetActive(false);
    }

    // 파라미터 (팝업 사이즈업 유무, 메시지, 콜백 1, 2) 
    public void ShowOneButtnPopup(bool isSizeUp, string msg, Action confirmOne = null, Action confirmTwo = null)
    {
        SoundManager.Instance.PlaySFX(SFX.Panel);
        untouchable.SetActive(true);

        if (oneButtonPopup == null)
        {
            oneButtonPopup = Instantiate(oneButtonPopupPrefab, canvas);
            oneButtonPopup.transform.position = new Vector3(Origin_Position, 0, 0);
            oneButtonRectTransform = oneButtonPopup.GetComponent<RectTransform>();
            animatorOneButton = oneButtonPopup.GetComponent<Animator>();
            oneButton = oneButtonPopup.GetComponentInChildren<Button>();
        }
        else
        {
            //oneButtonPopup.SetActive(true);
            // 재활용되는 팝업의 사이즈 초기화 
            Vector2 width = oneButtonRectTransform.sizeDelta;
            width.x = Origin_Width;
            oneButtonRectTransform.sizeDelta = width;
        }

        // 팝업 사이즈 조절
        if (isSizeUp)
        {
            Vector2 width = oneButtonRectTransform.sizeDelta;
            width.x = Setting_Width;
            oneButtonRectTransform.sizeDelta = width;
        }
        animatorOneButton.SetBool("isShow", true);
        StartCoroutine(OnFadeIn(oneButtonPopup));

        popupText = oneButtonPopup.GetComponentInChildren<TextMeshProUGUI>();
        popupText.text = msg;

        // 이전 콜백 지우기
        oneButton.onClick.RemoveAllListeners();

        oneButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySFX(SFX.Panel);
            untouchable.SetActive(false);

            animatorOneButton.SetBool("isShow", false);
            StopAllCoroutines();
            StartCoroutine(OnFadeOut(oneButtonPopup));

            if (confirmOne != null)
            {
                confirmOne.Invoke();
            }
            if (confirmTwo != null)
            {
                confirmTwo.Invoke();
            }
        });
    }

    // 파라미터 (사이즈업 유무, 메시지, 확인 버튼의 콜백 1, 2, 취소 버튼 콜백)
    public void ShowTwoButtnPopup(bool isSizeUp, string msg, Action confirmOne = null, Action confirmTwo = null, Action cancel = null)
    {
        SoundManager.Instance.PlaySFX(SFX.Panel);
        untouchable.SetActive(true);

        if (twoButtonPopup == null)
        {
            twoButtonPopup = Instantiate(twoButtonPopupPrefab, canvas);
            twoButtonPopup.transform.position = new Vector3(Origin_Position, 0, 0);
            twoButtonRectTransform = twoButtonPopup.GetComponent<RectTransform>();
            animatorTwoButton = twoButtonPopup.GetComponent<Animator>();
            twoButtons = twoButtonPopup.GetComponentsInChildren<Button>();
            confirmText = twoButtons[1].GetComponentInChildren<TextMeshProUGUI>();
        }
        else
        {
            //twoButtonPopup.SetActive(true);

            Vector2 width = twoButtonRectTransform.sizeDelta;
            width.x = Origin_Width;
            twoButtonRectTransform.sizeDelta = width;
        }
        // 팝업 사이즈 조절
        if (isSizeUp)
        {
            Vector2 width = twoButtonRectTransform.sizeDelta;
            width.x = Setting_Width;
            twoButtonRectTransform.sizeDelta = width;
        }
        twoButtons[1].interactable = false;
        StartCoroutine(ConfirmButton());        

        animatorTwoButton.SetBool("isShow", true);
        StartCoroutine(OnFadeIn(twoButtonPopup));

        popupText = twoButtonPopup.GetComponentInChildren<TextMeshProUGUI>();
        popupText.text = msg;

        foreach (var item in twoButtons)
        {
            item.onClick.RemoveAllListeners();
        }

        twoButtons[0].onClick.AddListener(() =>
        {
            ClosePopup();
            if (cancel != null)
            {
                cancel.Invoke();
            }
        });
        twoButtons[1].onClick.AddListener(() =>
        {
            ClosePopup();

            if (confirmOne != null)
            {
                confirmOne.Invoke();
            }
            if (confirmTwo != null)
            {
                confirmTwo.Invoke();
            }
        });
    }

    IEnumerator ConfirmButton()
    {
        yield return new WaitForSeconds(0.5f);
        confirmText.text = "3";

        yield return new WaitForSeconds(0.7f);
        confirmText.text = "2";

        yield return new WaitForSeconds(0.7f);
        confirmText.text = "1";

        yield return new WaitForSeconds(0.7f);
        string text = LocalizationManager.Instance.LocaleTable("확인");
        confirmText.text = text;
        twoButtons[1].interactable = true;
    }

    public void ClosePopup()
    {
        SoundManager.Instance.PlaySFX(SFX.Panel);
        animatorTwoButton.SetBool("isShow", false);
        untouchable.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(OnFadeOut(twoButtonPopup));
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
            temp.a -= Time.deltaTime * Fade_Speed;
            background.color = temp;
            yield return null;
        }

        temp.a = 0;
        background.color = temp;

        //if(popup != null)
        //{
        //    popup.SetActive(false);
        //}
    }

    IEnumerator OnFadeIn(GameObject popup)
    {
        background = popup.GetComponent<Image>();
        Color temp = background.color;

        yield return new WaitForSeconds(0.1f);

        while (background.color.a < 1)
        {
            temp.a += Time.deltaTime * Fade_Speed;
            background.color = temp;
            yield return null;
        }

        temp.a = 1;
        background.color = temp;
    }
}
