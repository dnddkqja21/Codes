using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : MonoBehaviour
{
    public ItemDataBase table;
    [SerializeField]
    InventoryUI inventory;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            inventory.AddSlotItem(table.item[0], 1);
        }
    }
}
