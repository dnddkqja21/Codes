using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextToWebview : MonoBehaviour
{
    [SerializeField]
    TMP_InputField inputField;    

    void Start()
    {
#if UNITY_IOS || UNITY_ANDROID
        //ios,aos 는 한글 지원이 됨..
        //옵션을 넣으면 됨.
#else
        InitText();
#endif

#if UNITY_EDITOR_WIN
        InitText();
#endif
    }

    async void InitText()
    {
        await WebviewManager.Instance.webview.WaitUntilInitialized();
        inputField.onEndEdit.AddListener((text) =>
        {
            Debug.Log("string = " + text);

            // Check if the new value is different from the old value
            if (inputField.text != text)
            {
                // Update the input field's value without triggering another onEndEdit event
                inputField.text = text;

                // Update the WebView using the new value
                WebviewManager.Instance.webview.WebView.SendKey(text);
            }
        });

        var nativeKeyboardListener = Vuplex.WebView.Internal.NativeKeyboardListener.Instantiate();
        WebviewManager.Instance.webview.WebView.FocusedInputFieldChanged += (sender, eventArgs) =>
        {
            if ((eventArgs.Type == Vuplex.WebView.FocusedInputFieldType.Text))
            {
                string jsScript = "document.activeElement.value";
                WebviewManager.Instance.webview.WebView.ExecuteJavaScript(jsScript, (value) =>
                {
                    string text = value.ToString();
                    inputField.text = text;
                    inputField.ActivateInputField();
                });
            }
            else
            {
                inputField.DeactivateInputField();
                inputField.text = "";
            }
        };
        
    }
    
}
