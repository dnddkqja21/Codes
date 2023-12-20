using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArrow : MonoBehaviour
{
    public int damage;


    private void OnEnable()
    {
        Invoke("PoolObject", 1.3f);
    }
    

    void PoolObject()
    {        
        ObjectPool_PF.objectPoolInstance.AddPoolObject(gameObject);
    }   
}
