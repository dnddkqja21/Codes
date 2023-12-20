using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonLeft : MonoBehaviour
{
    public Buy_Item buyItem;   

    public void OnCountLeft()
    {
        if (buyItem.itemCount == 0)
            return;

        buyItem.itemCount--;
        buyItem.count.text = buyItem.itemCount.ToString();        
    }
}
