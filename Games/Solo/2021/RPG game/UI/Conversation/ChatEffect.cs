using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatEffect : MonoBehaviour
{
    public GameObject endButton;

    string message;

    public int speed;

    float interval;

    TextMeshProUGUI tmpro;

    int index;

    public bool isChatting;

    AudioSource sound;

    void Start()
    {
        tmpro = GetComponent<TextMeshProUGUI>();
        sound = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        
    }

    public void SetMessage(string _msg)
    {
        if(isChatting)
        {
            tmpro.text = message;

            CancelInvoke();
            
            EffectExit();
        }
        else
        {
            message = _msg;
            EffectEnter();
        }
    }

    void EffectEnter()
    {
        tmpro.text = "";
        index = 0;
        endButton.SetActive(false);

        isChatting = true;

        interval = 1.0f / speed;

        Invoke("EffectStay", interval);
    }

    void EffectStay()
    {
        if(tmpro.text == message)
        {
            EffectExit();
            return;
        }

        tmpro.text += message[index];
        
        if(message[index] != ' ')
        {
            sound.Play();
        }

        index++;

        Invoke("EffectStay", interval);
    }

    void EffectExit()
    {
        endButton.SetActive(true);
        isChatting = false;
    }
}
