using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuplex.WebView;

/// <summary>
/// ���� ����
/// </summary>

public class WebviewManager : MonoBehaviour
{
    public CanvasWebViewPrefab webview;

    async void Awake()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_EDITOR_OSX
        await StandaloneWebView.TerminateBrowserProcess();
#endif
        Web.SetCameraAndMicrophoneEnabled(true);
    }

    void Start()
    {
        
    }

    // ���� ���� (������� ����)
    async public void LoadUrl(bool isVisibleWebview, string url)
    {
        await webview.WaitUntilInitialized();
        webview.WebView.LoadUrl(url);

        await webview.WebView.WaitForNextPageLoadToFinish();
        webview.Visible = isVisibleWebview;
    }
}
