using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SliderPosition : MonoBehaviour, IPointerUpHandler, IDragHandler
{
    [Header("핸들바")]
    public Slider slider;
    [Header("목표 거리")]
    public TextMeshProUGUI targetDistance;

    void Start()
    {
        slider.value = 0;
    }
    

    public void OnPointerUp(PointerEventData eventData)
    {
        slider.value = Mathf.Round(slider.value);
        //point.text = slider.value.ToString();
        targetDistance.text = "목표 거리 : " + EnumToData.Instance.DistanceRule(slider.value).ToString() + "cm";
    }

    public void OnDrag(PointerEventData eventData)
    {        
        //point.text = string.Format("{0:F1}", (slider.value));
    }
}
