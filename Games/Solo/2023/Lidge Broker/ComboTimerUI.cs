using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTimerUI : MonoBehaviour
{
    Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        float remain = GameManager.comboTime - GameManager.failTime;
        float rate = remain / GameManager.comboTime;
        slider.value = rate;
    }
}
