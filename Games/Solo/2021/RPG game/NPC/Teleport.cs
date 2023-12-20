using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Light lighting;

    public GameObject button;    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            lighting.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (lighting.range <= 15f)
            {
                lighting.range += 0.5f * Time.deltaTime;
                button.SetActive(true);
            }
        }        
    }


    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            lighting.range = 9f;
            lighting.enabled = false;
            button.SetActive(false);
        }
    }
}
