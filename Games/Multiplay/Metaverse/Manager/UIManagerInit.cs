using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerInit : MonoBehaviour
{
    static UIManagerInit instance = null;
    public static UIManagerInit Instance { get { return instance; } }

    [Header("패널")]
    [SerializeField]
    Animator panelLogin;
    [SerializeField]
    Animator panelSignUp;
    [Header("버튼")]
    [SerializeField]
    Button buttonLogin;
    [SerializeField]
    Button buttonTrySignUp;
    [SerializeField]
    Button buttonCancel;    
    public Button buttonSignUp;
    [SerializeField]
    Button buttonToRight;
    [SerializeField]
    Button buttonToLeft;
    [Header("아바타")]
    [SerializeField]
    GameObject avatarCamera;
    [SerializeField]
    GameObject[] avatars;
    int currentIndex = 0;
    [Header("로그인 인풋")]
    [SerializeField]
    TMP_InputField inputIDLogin;
    [SerializeField]
    TMP_InputField inputPWLogin;
    [Header("회원 가입 인풋")]    
    public TMP_InputField inputIDSignUp;    
    public TMP_InputField inputPWSignUp;

    void Awake()
    {
        if(instance == null) { instance = this; }
    }

    void Start()
    {
        buttonTrySignUp.onClick.AddListener(() => 
        {
            StartCoroutine(ShowPanel(panelLogin, panelSignUp));
        });

        buttonCancel.onClick.AddListener(() =>
        {
            StartCoroutine(ShowPanel(panelSignUp, panelLogin));
        });

        //buttonSignUp.onClick.AddListener(() =>
        //{
        //    // 차후 구현 시 메시지도 로컬라이징으로 처리
        //    PopupManager.Instance.ShowOneButtnPopup("환영합니다.", GoToLoginPanel);
        //});

        buttonToRight.onClick.AddListener(() =>
        {
            currentIndex++;
            if(currentIndex >= avatars.Length)
                currentIndex = 0;
            StartCoroutine(ToggleAvatars());
        });

        buttonToLeft.onClick.AddListener(() =>
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = avatars.Length -1;
            StartCoroutine(ToggleAvatars());
        });
    }

    IEnumerator ToggleAvatars()
    {
        avatarCamera.SetActive(false);
        yield return new WaitForSeconds(0.1f);

        // 모든 오브젝트 비활성화 한 뒤 현재 인덱스의 오브젝트만 활성화
        foreach (GameObject obj in avatars)
        {
            obj.SetActive(false);
        }
        if (currentIndex >= 0 && currentIndex < avatars.Length)
        {
            avatars[currentIndex].SetActive(true);
        }

        // 카메라를 재활성화 해야 오브젝트 겹침없이 렌더링 된다.
        avatarCamera.SetActive(true);
    }

    // 파라미터 : 사라질 순서
    IEnumerator ShowPanel(Animator firstPanel, Animator secondPanel)
    {
        firstPanel.SetBool("isShow", false);
        yield return new WaitForSeconds(0.5f);
        secondPanel.SetBool("isShow", true);
    }

    public void GoToLoginPanel()
    {
        StartCoroutine(ShowPanel(panelSignUp, panelLogin));
    }
}
