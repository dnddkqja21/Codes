using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class LoadingManager : MonoBehaviour
{
    [SerializeField]
    GameObject schLogoPanel;
    [SerializeField]
    GameObject appCheckPanel;    
    CheckPermission permissionHandler;

    void Start()
    {
        Debug.Log("gusanaglkyo releaseMode = " + BuildConfig.isRelease);
        DebugCustom.Log("릴리즈 모드 : " + BuildConfig.isRelease);
        Invoke("InActiveLogo", 2f);
    }

    void InActiveLogo()
    {
        // 테스트 서버일 시 바로 통과
        if(!BuildConfig.isRelease)
        {
            schLogoPanel.SetActive(false);
            Invoke("InActiveLoading", 2f);
            return;
        }
        schLogoPanel.SetActive(false);
        appCheckPanel.SetActive(true);

        permissionHandler = GetComponent<CheckPermission>();
        CheckPermissionsAndDoSomething();


        //// 앱 버전 체크
        Dictionary<string, object> requestData = new Dictionary<string, object>();

        StartCoroutine(HttpNetwork.Instance.PostSendNetwork(requestData, URL_CONFIG.APP_CHECK, (data) =>
        {
            if (data != null)
            {
                Dictionary<string, object> obj = (Dictionary<string, object>)data;
                string appver = (string)obj["appVer"]; 
                // 0 : 검수 완료, 1 : 검수 중
                string confirm = (string)obj["isConfirm"];                

                // 에디터에서만 사용가능함. 앱 빌드 불가
                //string appVersion = PlayerSettings.bundleVersion;
                string appVersion = Application.version;

                if (appver.Equals(appVersion) || confirm.Equals("1"))
                {
                    Invoke("InActiveLoading", 2f);
                }
                else
                {
                    Locale curLang = LocalizationSettings.SelectedLocale;
                    string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "앱출시", curLang);
                    PopupManager.Instance.ShowOneButtnPopup(text, OpenStore);                    
                }
            }
        }));
    }

    void CheckPermissionsAndDoSomething()
    {
        permissionHandler.CheckPermissions();
    }

    void OpenStore()
    {
#if UNITY_STANDALONE
        Application.OpenURL(URL_CONFIG.Download_PC);
#elif UNITY_ANDROID              
        Application.OpenURL(URL_CONFIG.Download_AOS);
#elif UNITY_IOS
        Application.OpenURL(URL_CONFIG.Download_IOS);
#endif     
        Application.Quit();
    }

    void InActiveLoading()
    {
        appCheckPanel.SetActive(false);
    }
}
