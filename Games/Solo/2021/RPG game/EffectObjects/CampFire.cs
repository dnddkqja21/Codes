using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CampFire : MonoBehaviour
{
    public TextMeshPro text;

    float recoverySpeed = 7f;

    void Start()
    {
        
    }
        
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            text.gameObject.SetActive(true);
            Color color = text.color;
            color.a = 1;
            text.color = color;
        }
    }

    private void OnTriggerStay(Collider other)
    {        
        //Debug.Log("트리거 충돌 중");
        if (other.tag == "Player")
        {
            InvokeRepeating("MinusAlpha", 2f, 0.2f);

            Player_PF tmp = other.GetComponent<Player_PF>();

            if(tmp.curHP == tmp.maxHP)
            {
                return;
            }            

            tmp.curHP += Time.deltaTime * recoverySpeed;

            if(tmp.curHP > tmp.maxHP)
            {
                tmp.curHP = tmp.maxHP;
            }
        }        
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            text.gameObject.SetActive(false);
        }
    }
    
    void MinusAlpha()
    {
        Color color = text.color;
        color.a -= Time.deltaTime;
        text.color = color;

        if (text.color.a <= 0f)
        {
            CancelInvoke("MinusAlpha");            
        }
    }    
}
