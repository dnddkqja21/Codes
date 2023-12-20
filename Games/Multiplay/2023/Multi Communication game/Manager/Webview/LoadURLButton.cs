using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuplex.WebView;

/// <summary>
/// URL 로드 버튼
/// </summary>

public class LoadURLButton : MonoBehaviour
{
    [SerializeField]
    CanvasWebViewPrefab canvasWebViewPrefab;    

    public async void OnLoadURL(string url)
    {
        await canvasWebViewPrefab.WaitUntilInitialized();
        canvasWebViewPrefab.WebView.LoadUrl(url);
    }
}
