using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

/// <summary>
/// 화상 챗을 위한 카메라와 마이크 퍼미션
/// </summary>

public class PermissionManager : MonoBehaviour
{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_EDITOR_OSX
    
#elif UNITY_ANDROID
    static string[] AndroidPermissions = new string[] 
    {
        Permission.Camera,
        Permission.Microphone
    };

    void Start()
    {
        CheckPermission();
    }
    void CheckPermission()
    {
        foreach (string permission in AndroidPermissions)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermissions(AndroidPermissions);
            }
        }
    }
#endif
}
