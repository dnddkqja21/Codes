using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Crafting : MonoBehaviour
{
    public string matItemName;

    public string matItemName2;

    public int matItemCount;

    public int matItemCount2;

    public Item rewardItem;

    public TextMeshProUGUI log;

    public GameObject craftingBt;
    

    public void OnCrafting()
    {
        if (matItemName2 == "")  // ���2�� ���ٸ� (��ᰡ �Ѱ������)
        {
            if(InventoryUI.instance.FindItem(matItemName, matItemCount) != -1)
            {
                int tmp = InventoryUI.instance.FindItem(matItemName, matItemCount);
                InventoryUI.instance.slots[tmp].ClearSlot();
                
                InventoryUI.instance.AddSlotItem(rewardItem);

                CraftingEf.instance.EffectOn();

                log.gameObject.SetActive(true);
                log.text = "[" + rewardItem.itemName + "]" + "��(��) �����Ͽ����ϴ�.";
                Invoke("TurnOff", 2f);

                craftingBt.SetActive(false);
            }
            else
            {
                // ��ᰡ �����ϴ�.
                log.gameObject.SetActive(true);
                log.text = "��ᰡ �����մϴ�.";
                Invoke("TurnOff", 1f);

                craftingBt.SetActive(false);
            }
        }
        else
        {   // ��ᰡ �ΰ������
            if(InventoryUI.instance.FindItem(matItemName, matItemCount) != -1 && InventoryUI.instance.FindItem(matItemName2, matItemCount2) != -1)
            {
                int tmp = InventoryUI.instance.FindItem(matItemName, matItemCount);
                InventoryUI.instance.slots[tmp].ClearSlot();

                int tmp2 = InventoryUI.instance.FindItem(matItemName2, matItemCount2);
                InventoryUI.instance.slots[tmp2].ClearSlot();

                InventoryUI.instance.AddSlotItem(rewardItem);

                CraftingEf.instance.EffectOn();

                log.gameObject.SetActive(true);
                log.text = "[" + rewardItem.itemName + "]" + "��(��) �����Ͽ����ϴ�.";
                Invoke("TurnOff", 2f);

                craftingBt.SetActive(false);
            }
            else
            {
                log.gameObject.SetActive(true);
                log.text = "��ᰡ �����մϴ�.";
                Invoke("TurnOff", 1f);

                craftingBt.SetActive(false);
            }
        }
    }

    void TurnOff()
    {
        log.gameObject.SetActive(false);
    }
}
