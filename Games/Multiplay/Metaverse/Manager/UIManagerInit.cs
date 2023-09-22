using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerInit : MonoBehaviour
{
    static UIManagerInit instance = null;
    public static UIManagerInit Instance { get { return instance; } }

    [Header("�г�")]
    [SerializeField]
    Animator panelLogin;
    [SerializeField]
    Animator panelSignUp;
    [Header("��ư")]
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
    [Header("�ƹ�Ÿ")]
    [SerializeField]
    GameObject avatarCamera;
    [SerializeField]
    GameObject[] avatars;
    int currentIndex = 0;
    [Header("�α��� ��ǲ")]
    [SerializeField]
    TMP_InputField inputIDLogin;
    [SerializeField]
    TMP_InputField inputPWLogin;
    [Header("ȸ�� ���� ��ǲ")]    
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
        //    // ���� ���� �� �޽����� ���ö���¡���� ó��
        //    PopupManager.Instance.ShowOneButtnPopup("ȯ���մϴ�.", GoToLoginPanel);
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

        // ��� ������Ʈ ��Ȱ��ȭ �� �� ���� �ε����� ������Ʈ�� Ȱ��ȭ
        foreach (GameObject obj in avatars)
        {
            obj.SetActive(false);
        }
        if (currentIndex >= 0 && currentIndex < avatars.Length)
        {
            avatars[currentIndex].SetActive(true);
        }

        // ī�޶� ��Ȱ��ȭ �ؾ� ������Ʈ ��ħ���� ������ �ȴ�.
        avatarCamera.SetActive(true);
    }

    // �Ķ���� : ����� ����
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
