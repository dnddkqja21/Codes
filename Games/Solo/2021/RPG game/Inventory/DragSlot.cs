using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;

    public ItemSlotUI dragSlot;

    public Image itemImage;

    private void Start()
    {
        instance = this;    // 나 자신을 인스턴스로 함.
    }

    public void DragSetImage(Image _itemImage)
    {
        itemImage.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _a)
    {
        Color color = itemImage.color;
        color.a = _a;
        itemImage.color = color;
    }
}
