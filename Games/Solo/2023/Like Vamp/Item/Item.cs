using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image iconImage;
    Text levelText;
    Text nameText;
    Text descText;

    void Awake()
    {
        iconImage = GetComponentsInChildren<Image>()[1];
        iconImage.sprite = itemData.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        levelText = texts[0];
        nameText = texts[1];
        descText = texts[2];

        nameText.text = itemData.itemName;
    }

    void OnEnable()
    {
        levelText.text = "Lv." + (level + 1);

        switch (itemData.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                descText.text = string.Format(itemData.itemDescription, itemData.damages[level] * 100, itemData.counts[level]);
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                descText.text = string.Format(itemData.itemDescription, itemData.damages[level] * 100);
                break;

            default:
                descText.text = string.Format(itemData.itemDescription);
                break;
        }
    }
        

    public void OnClick()
    {
        switch (itemData.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if(level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(itemData);
                }
                else
                {
                    float nextDamage = itemData.baseDamage;
                    int nextCount = 0;

                    // 백분율을 기존 데미지에 곱한 것을 누적
                    nextDamage += itemData.baseDamage * itemData.damages[level];
                    nextCount += itemData.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;

            case ItemData.ItemType.Glove: 
            case ItemData.ItemType.Shoe: 
                if(level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(itemData);
                }
                else
                {
                    float nextRate = itemData.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;

            case ItemData.ItemType.Heal:
                GameManager.Instance.health = GameManager.Instance.maxHealth;
                break;
        }

        if(level == itemData.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
