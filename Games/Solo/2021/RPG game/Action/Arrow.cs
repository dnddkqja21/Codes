using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        Invoke("PoolObject", 1.3f);
        //Invoke("Shoot", 0.2f);
    }

    void Shoot()
    {
        Rigidbody arrow = GetComponent<Rigidbody>();
        arrow.AddForce(new Vector3(0, 0,transform.position.y) * 50f, ForceMode.Impulse);
    }

    void PoolObject()
    {        
        ObjectPool_PF.objectPoolInstance.AddPoolObject(gameObject);
    }   
}
