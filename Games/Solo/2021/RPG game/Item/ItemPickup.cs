using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    private void Update()
    {
        if(item.itemType == Item.ItemType.Equipment)
        {
            transform.Rotate(Vector3.up * 20f * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        if(item.itemType != Item.ItemType.Equipment)
        {
            if (item.itemName == "µÂ∑°∞Ô ¿Ãª°")
                return;

            Invoke("Pool", 5f);
        }
    }

    void Pool()
    {
        ObjectPool_PF.objectPoolInstance.AddPoolObject(gameObject);
    }
}
