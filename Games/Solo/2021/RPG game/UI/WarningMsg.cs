using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningMsg : MonoBehaviour
{

    ActionController player;

    void Start()
    {
        player = FindObjectOfType<ActionController>();
        Invoke("SoundOn", 4f);
        Invoke("TurnOff", 7f);
    }

    
    void Update()
    {
        
    }

    void TurnOff()
    {
        gameObject.SetActive(false);
    }

    void SoundOn()
    {
        player.PlayClips(22);
    }
}
