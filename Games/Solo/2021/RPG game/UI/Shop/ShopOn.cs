using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOn : MonoBehaviour
{

    public GameObject shop;

    ActionController player;

    void Start()
    {
        player = FindObjectOfType<ActionController>();    
    }

    
    void Update()
    {
        
    }

    public void OnShop()
    {
        gameObject.SetActive(false);
        shop.SetActive(true);
        player.PlayClips(2);
    }
}
