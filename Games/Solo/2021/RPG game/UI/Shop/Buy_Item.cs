using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buy_Item : MonoBehaviour
{
    Player_PF player;

    ActionController bgm;

    public InventoryUI inven;    

    public Item item;

    public int price;

    public int itemCount = 0;

    public TextMeshProUGUI msg;

    public TextMeshProUGUI count;

    public GameObject setCount;

    void Start()
    {
        player = FindObjectOfType<Player_PF>();

        bgm = FindObjectOfType<ActionController>();
    }

    
    void Update()
    {    
        if (itemCount <= 0)
        {
            itemCount = 0;
            //count.text = itemCount.ToString();
        }
    }

    public void BuyItem()
    {
        if (itemCount == 0)
            return;

        if(player.gold >= price * itemCount)
        {
            inven.AddSlotItem(item, itemCount);
            player.gold -= price * itemCount;
            bgm.PlayClips(9);
            msg.gameObject.SetActive(true);
            msg.text = "[" + item.itemName + "]" + "을(를) " + itemCount.ToString()  + "개 구매하였습니다.";
            Invoke("SetFalse", 1f);
            itemCount = 0;
            count.text = itemCount.ToString();
            setCount.SetActive(false);
        }
        else
        {
            msg.gameObject.SetActive(true);
            msg.text = "골드가 부족합니다.";
            Invoke("SetFalse", 1f);
            Debug.Log("돈 부족합니다.");
            itemCount = 0;
            count.text = itemCount.ToString();
            setCount.SetActive(false);
            //돈 부족 띄우기.
        }
    }

    void SetFalse()
    {
        msg.gameObject.SetActive(false);
    }
}
