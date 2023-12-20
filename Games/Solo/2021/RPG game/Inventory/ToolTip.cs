using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolTip : MonoBehaviour
{
    public GameObject tooltip;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI type;
    
    // ������ ������ �������� ����
    public void ItemTooltip(Item _item, Vector3 _pos)
    {
        tooltip.SetActive(true);

        // ������ �������� �»�ܿ��� �����ǰԲ� �������� �����.
        _pos += new Vector3(-tooltip.GetComponent<RectTransform>().rect.width * 0.5f,
            tooltip.GetComponent<RectTransform>().rect.height * 0.5f, 0);

        tooltip.transform.position = _pos;

        itemName.text = _item.itemName;
        desc.text = _item.itemDesc;
        
        if(_item.itemType == Item.ItemType.Equipment)
        {
            type.text = "-��� ������-";
        }
        else if (_item.itemType == Item.ItemType.Quest)
        {
            type.text = "-����Ʈ ������-";
        }
        else
        {
            type.text = "-��Ŭ���Ͽ� ���-";
        }
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
