using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnOffText : MonoBehaviour
{
    [Header("������ �ؽ�Ʈ")]
    public TextMeshProUGUI onOffText;

    bool isOn = false;

    string originText = string.Empty;

    private void Start()
    {
        originText = onOffText.text.Substring(0, onOffText.text.Length - 2);
        //Debug.Log(originText);
    }

    public void TextSwitch()
    {
        isOn = !isOn;

        if(isOn)
        {
            onOffText.text = originText + "Off";
        }
        else
        {
            onOffText.text = originText + "On";
        }
    }
}
