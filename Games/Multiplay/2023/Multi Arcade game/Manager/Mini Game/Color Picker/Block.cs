using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Block : MonoBehaviour, IPointerDownHandler
{
    Image image;
    ColorPickerManager colorPickerManager;

    // 프로퍼티
    public Color color { set => image.color = value; get => image.color; }

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetUp(ColorPickerManager gameManager)
    {
        image = GetComponent<Image>();
        this.colorPickerManager = gameManager;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        colorPickerManager.CheckBlock(color);
    }
}
