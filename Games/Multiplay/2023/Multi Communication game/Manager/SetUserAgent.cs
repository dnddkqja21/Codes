using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuplex.WebView;

/// <summary>
/// 유저 에이전트
/// </summary>

public class SetUserAgent : MonoBehaviour
{
    bool isSetting = false;

    async void Awake()
    {
        // 웹뷰 세팅 시 브라우저 파괴
#if UNITY_STANDALONE || UNITY_EDITOR
        await StandaloneWebView.TerminateBrowserProcess();
#endif
        //Web.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");
        //Web.SetUserAgent("Mozilla/5.0 (X11; Linux i586; rv:31.0) Gecko/20100101 Firefox/31.0");
        Web.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36");

        //string setUserAgent = Util.LoadData(AESUtil.SetUserAgent);
        //if(!setUserAgent.Equals("1"))
        //{
        //    Web.SetUserAgent("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");
        //    isSetting = true;

        //    string booleanData = isSetting == true ? "1" : "0";
        //    Util.SaveData(AESUtil.SetUserAgent, booleanData);
        //}
    }
}
