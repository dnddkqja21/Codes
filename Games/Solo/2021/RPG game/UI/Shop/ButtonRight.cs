using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRight : MonoBehaviour
{
    public Buy_Item buyItem;

    public void OnCountRight()
    {
        buyItem.itemCount++;
        buyItem.count.text = buyItem.itemCount.ToString();
    }
}
