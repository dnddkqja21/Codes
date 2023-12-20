using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFireball : MonoBehaviour
{
    float speed = 1f;
    void Start()
    {
        
    }

    void Update()
    {
        if(transform.localScale.x <= 3f)
        {
            float tmpX = transform.localScale.x;
            tmpX += Time.deltaTime * speed;

            float tmpY = transform.localScale.x;
            tmpY += Time.deltaTime * speed;

            float tmpZ = transform.localScale.x;
            tmpZ += Time.deltaTime * speed;
            transform.localScale = new Vector3(tmpX, tmpY, tmpZ);
        }
    }
}
