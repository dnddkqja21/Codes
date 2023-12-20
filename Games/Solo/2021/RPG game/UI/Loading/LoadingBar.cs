using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingBar : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public Image fillImage;

    
    private float loadingTime = 3;
    private float curTime;
    private float startTime;
    private bool isEnd = true;

    void Start()
    {
        Reset_Loading();
    }

    void Update()
    {
        if (isEnd)
            return;
        Check_Loading();

        if(gameObject.activeSelf == true)
        {
            Invoke("TurnOff", 3.01f);
        }
    }

    void Check_Loading()
    {
        curTime = Time.time - startTime;
        if (curTime < loadingTime)
        {
            Set_FillAmount(curTime / loadingTime);
        }
        else if (!isEnd)
        {
            End_Loading();
            Reset_Loading();
            //TurnOff();
            //Invoke("Reset_Loading", 0.2f);
            //Invoke("TurnOff", 0.5f);
        }
    }

    void End_Loading()
    {
        Set_FillAmount(1);
        isEnd = true;
    }

    void Reset_Loading()
    {
        curTime = loadingTime;        
        startTime = Time.time;
        
        Set_FillAmount(0);
        isEnd = false;
    }

    void Set_FillAmount(float _value)
    {
        fillImage.fillAmount = _value;
        string txt = (_value.Equals(1) ? "Finished... " : "Loading... ") + (_value).ToString("P");
        loadingText.text = txt;
        //Debug.Log(txt);
    }

    void TurnOff()
    {
        gameObject.SetActive(false);
    }

}
