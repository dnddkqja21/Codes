using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase instance = null;

    public List<Item> item = new List<Item>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
}
