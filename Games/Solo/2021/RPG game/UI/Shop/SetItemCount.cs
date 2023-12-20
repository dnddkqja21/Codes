using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItemCount : MonoBehaviour
{
    bool activated = false; 

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void OnSetCount()
    {
        activated = !activated;
        if (activated)
            gameObject.SetActive(true);

        else
            gameObject.SetActive(false);
    }
}
