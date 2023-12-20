using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    ActionController ac;

    private void Start()
    {
        ac = FindObjectOfType<ActionController>();
    }

    public void Attack()
    {
        ac.Attack();        
    }
}
