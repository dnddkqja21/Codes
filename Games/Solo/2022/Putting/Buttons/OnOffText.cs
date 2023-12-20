using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnOffText : MonoBehaviour
{
    [Header("변경할 텍스트")]
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
