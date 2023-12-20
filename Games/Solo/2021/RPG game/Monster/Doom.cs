using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doom : MonoBehaviour
{
    float speed = 3f;

    void Start()
    {
        
    }

    
    void Update()
    {
        float tmp = transform.localScale.x;
        float tmp2 = transform.localScale.z;
        tmp += Time.deltaTime * speed;
        tmp2 += Time.deltaTime * speed;

        transform.localScale = new Vector3(tmp, 0.1f, tmp2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MonstersAttack")
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
