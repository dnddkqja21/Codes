using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public Color highRate;
    public Color midRate;
    public Color lowRate;
    public Image fill;

    Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        // 남은 시간
        float remain = GameManager.maxTime - GameManager.playTime;

        // 남은 시간을 총 시간으로 나눔
        float rate = remain / GameManager.maxTime;
        slider.value = rate;

        if(rate > 0.7)
        {
            fill.color = highRate;
        }
        else if(rate > 0.3f)
        {
            fill.color = midRate;
        }
        else
        {
            fill.color = lowRate;
        }
    }
}
