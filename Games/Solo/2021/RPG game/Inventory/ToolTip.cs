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
    
    // 아이템 정보와 포지션을 받음
    public void ItemTooltip(Item _item, Vector3 _pos)
    {
        tooltip.SetActive(true);

        // 툴팁이 아이템의 좌상단에서 생성되게끔 포지션을 계산함.
        _pos += new Vector3(-tooltip.GetComponent<RectTransform>().rect.width * 0.5f,
            tooltip.GetComponent<RectTransform>().rect.height * 0.5f, 0);

        tooltip.transform.position = _pos;

        itemName.text = _item.itemName;
        desc.text = _item.itemDesc;
        
        if(_item.itemType == Item.ItemType.Equipment)
        {
            type.text = "-장비 아이템-";
        }
        else if (_item.itemType == Item.ItemType.Quest)
        {
            type.text = "-퀘스트 아이템-";
        }
        else
        {
            type.text = "-우클릭하여 사용-";
        }
    }

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
