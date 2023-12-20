using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Status : MonoBehaviour
{
    public Player_PF player;

    public TextMeshProUGUI attack;

    public TextMeshProUGUI defense;

    public TextMeshProUGUI health;

    public TextMeshProUGUI mana;

    public TextMeshProUGUI gold;

    void Start()
    {
        
    }

    
    void Update()
    {
        SetAttack();
        SetDefense();
        SetHealth();
        SetMana();
        SetGold();
    }

    void SetAttack()
    {
       attack.text = player.attack.ToString();
    }

    void SetDefense()
    {
        defense.text = player.defense.ToString();
    }

    void SetHealth()
    {
        health.text = player.maxHP.ToString();
    }

    void SetMana()
    {
        mana.text = player.maxMP.ToString();
    }

    void SetGold()
    {
        gold.text = player.gold.ToString();
    }   
}
