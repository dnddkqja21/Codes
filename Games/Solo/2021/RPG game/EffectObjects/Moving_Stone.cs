using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Stone : MonoBehaviour
{
    
    float speed = 1.5f;

    float maxHeight = 15f;

    float minHeight = 7f;
    void Start()
    {
        InvokeRepeating("StoneUp", 0.5f, 0.1f);
    }

    void Update()
    {

        
        
    }

    public void StoneUp()
    {
        Vector3 tmp = transform.position;
        tmp.y += Time.deltaTime * speed;
        transform.position = tmp;

        if (transform.position.y >= maxHeight)
        {
            CancelInvoke("StoneUp");
            InvokeRepeating("StoneDown", 0.5f, 0.1f);
        }
    }

    public void StoneDown()
    {
        Vector3 tmp = transform.position;
        tmp.y -= Time.deltaTime * speed;
        transform.position = tmp;

        if (transform.position.y <= minHeight)
        {
            CancelInvoke("StoneDown");
            InvokeRepeating("StoneUp", 0.5f, 0.1f);
        }
    }
}
