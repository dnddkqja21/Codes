using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    public ItemSlotUI equipSlot;

    Player_PF player;       

    public GameObject basic;

    public GameObject wood;

    public GameObject leather;

    public GameObject bone;

    public GameObject plate;

    List<Item> equipedItemList;

    bool isEquiped;

    void Start()
    {
        player = FindObjectOfType<Player_PF>();
        equipedItemList = new List<Item>();
    }

    
    void Update()
    {
        if(equipSlot.item != null && isEquiped == false)
        {
            switch(equipSlot.item.itemName)
            {
                case "³ª¹« °©¿Ê":
                    basic.SetActive(false);
                    wood.SetActive(true);
                    leather.SetActive(false);
                    bone.SetActive(false);
                    plate.SetActive(false);
                    break;

                case "°¡Á× °©¿Ê":
                    basic.SetActive(false);
                    wood.SetActive(false);
                    leather.SetActive(true);
                    bone.SetActive(false);
                    plate.SetActive(false);
                    break;

                case "»À °©¿Ê":
                    basic.SetActive(false);
                    wood.SetActive(false);
                    leather.SetActive(false);
                    bone.SetActive(true);
                    plate.SetActive(false);
                    break;

                case "ÇÃ·¹ÀÌÆ® ¸ÞÀÏ":
                    basic.SetActive(false);
                    wood.SetActive(false);
                    leather.SetActive(false);
                    bone.SetActive(false);
                    plate.SetActive(true);
                    break;

            }
            player.defense +=
            equipSlot.item.value;
            isEquiped = true;
            equipedItemList.Add(equipSlot.item);
        }

        if (equipSlot.item == null && isEquiped == true)
        {
            basic.SetActive(true);
            wood.SetActive(false);
            leather.SetActive(false);
            bone.SetActive(false);
            plate.SetActive(false);
            player.defense -= equipedItemList[0].value;
            equipedItemList.Clear();
            isEquiped = false;
        }
    }
}
