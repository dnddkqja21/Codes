using UnityEngine;
using TMPro;

public class GetKeyInteraction : MonoBehaviour
{   
    [Header("인풋 필드")]
    public TMP_InputField[] inputFields;
    [Header("인풋 필드 전환 키")]
    public KeyCode tab = KeyCode.Tab;
    [Header("엔터 키")]
    public KeyCode enter = KeyCode.Return;

    // 탭키 누를 시 이동할 인덱스
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
