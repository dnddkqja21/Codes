using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Threading.Tasks;

public class BackendManager : MonoBehaviour
{    
    void Start()
    {
        var backend = Backend.Initialize(true);
        if (backend.IsSuccess())
        {
            Debug.Log("초기화 성공 " + backend);
        }
        else
        {
            Debug.Log("초기화 실패 " + backend);
        }

        UIManagerInit.Instance.buttonSignUp.onClick.AddListener(() =>
        {
            TEST();
        });
    }

    async void TEST()
    {
        await Task.Run(() =>
        {
            Login.Instance.SignUp(UIManagerInit.Instance.inputIDSignUp.text, UIManagerInit.Instance.inputPWSignUp.text);
        });
    }
}
