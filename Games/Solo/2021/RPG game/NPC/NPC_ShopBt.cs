using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_ShopBt : MonoBehaviour
{
    public GameObject button;

    Vector3 offSet = new Vector3(0, 1f, 0);  

    void Start()
    {
        //Transform parents = GameObject.Find("Canvas").transform;
        button.SetActive(false);        
    }

    
    void Update()
    {
        button.transform.position = Camera.main.WorldToScreenPoint(transform.position + offSet);

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if(hit.transform.name == "연금술사 알랭")
                {
                    button.SetActive(true);
                }
                else
                {
                    button.SetActive(false);
                }
            }
        }

    }
}
