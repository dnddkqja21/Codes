using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breath : MonoBehaviour
{
    
    void Start()
    {
        Delete();
    }

    
    void Update()
    {
        
    }

    void Delete()
    {
        Destroy(gameObject, 1f);
    }
}
