using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning_Line : MonoBehaviour
{
    public Vector3 targetPos;

    void Start()
    {        
        Destroy(gameObject, 1.5f);        
    }

    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos + new Vector3(0, 1f, 0), Time.deltaTime * 2.5f);
    }
}
