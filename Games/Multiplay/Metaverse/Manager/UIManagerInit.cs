using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerInit : MonoBehaviour
{
    [SerializeField]
    Animator loginPanel;
    [SerializeField]
    Animator signUpPanel;
    [SerializeField]
    Button loginButton;
    [SerializeField]
    Button trySignUpButton;
    [SerializeField]
    Button cancelButton;
    [SerializeField]
    Button signUpButton;

    void Start()
    {
        trySignUpButton.onClick.AddListener(() => 
        {
            StartCoroutine(ShowPanel(loginPanel, signUpPanel));
        });

        cancelButton.onClick.AddListener(() =>
        {
            StartCoroutine(ShowPanel(signUpPanel, loginPanel));
        });

        signUpButton.onClick.AddListener(() =>
        {
            PopupManager.Instance.ShowOneButtnPopup("ȯ���մϴ�.", GoToLoginPanel);
        });
    }

    // �Ķ���� : ����� ����
    IEnumerator ShowPanel(Animator firstPanel, Animator secondPanel)
    {
        firstPanel.SetBool("isShow", false);
        yield return new WaitForSeconds(0.5f);
        secondPanel.SetBool("isShow", true);
    }

    void GoToLoginPanel()
    {
        StartCoroutine(ShowPanel(signUpPanel, loginPanel));
    }
}
