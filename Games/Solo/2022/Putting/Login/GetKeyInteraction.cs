using UnityEngine;
using TMPro;

public class GetKeyInteraction : MonoBehaviour
{   
    [Header("��ǲ �ʵ�")]
    public TMP_InputField[] inputFields;
    [Header("��ǲ �ʵ� ��ȯ Ű")]
    public KeyCode tab = KeyCode.Tab;
    [Header("���� Ű")]
    public KeyCode enter = KeyCode.Return;

    // ��Ű ���� �� �̵��� �ε���
    int nextIndex;

    LoginUI login;

    private void Start()
    {
        login = GetComponent<LoginUI>();
    }

    void Update()
    {
        GetCurrentFocused();
        EnterToLogin();
    }

    void GetCurrentFocused()
    {
        if (Input.GetKeyDown(tab))
        {
            for (int i = 0; i < inputFields.Length; i++)
            {                
                if(!inputFields[i].isFocused)
                {
                    nextIndex = i;
                }            
            }
            inputFields[nextIndex].Select();
        }
    }

    void EnterToLogin()
    {
        if(Input.GetKeyDown(enter))
        {
            login.OnEnterMember();
        }
    }

    public void IDInputSelect()
    {
        inputFields[0].Select();
    }
}
