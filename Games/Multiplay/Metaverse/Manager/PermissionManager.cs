using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

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
