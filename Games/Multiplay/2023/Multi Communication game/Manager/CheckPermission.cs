using UnityEngine.Android;
using UnityEngine;

/// <summary>
/// 안드로이드 퍼미션
/// </summary>

public class CheckPermission : MonoBehaviour
{
    public delegate void PermissionCheckCallback(bool hasCameraPermission, bool hasMicPermission);
    
    static string[] AndroidPermissions_taget32 = new string[] {
                Permission.ExternalStorageRead,
                Permission.ExternalStorageWrite,
                Permission.Camera,
                Permission.Microphone,
                "android.permission.NOTIFICATION"
            };

    static string[] AndroidPermissions_taget33 = new string[] {
                Permission.ExternalStorageRead,
                Permission.ExternalStorageWrite,
                Permission.Camera,
                Permission.Microphone,
                "android.permission.POST_NOTIFICATIONS"
            };

    public bool CheckPermissions()
    {
#if UNITY_EDITOR_WIN
        
#elif UNITY_ANDROID
        Debug.Log("gusanaglkyo check Permission");
        return CheckAndroidPermissions();
        // CheckAndroidPermissions2();
#elif UNITY_IOS
        CheckIOSPermissions();
#else
        // Handle PC (Windows) or other platforms here if needed.
#endif
        return true;
    }

   
    private bool CheckAndroidPermissions()
    {
        string androidInfo = SystemInfo.operatingSystem;
        Debug.Log("gusanaglkyo androidInfo: " + androidInfo);
        int apiLevel = int.Parse(androidInfo.Substring(androidInfo.IndexOf("-") + 1, 2));
        Debug.Log("gusanaglkyo apiLevel: " + apiLevel);
        string[] permissions = apiLevel >= 33 ? AndroidPermissions_taget33 : AndroidPermissions_taget32;

        foreach (string permission in permissions)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Debug.Log("gusanaglkyo check permission = " + permission + "false");
                Permission.RequestUserPermissions(permissions);
                return false;
            }
        }
        return true;
    }

    private void CheckIOSPermissions()
    {

    }
}