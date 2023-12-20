using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicArrowGuide : MonoBehaviour
{
    public int damage;

    public Rigidbody rigid;

    Transform target = null;

    public LayerMask layer;

    void Start()
    {
        
    }

    
    void Update()
    {
        SearchTarget();
        Attack();

    }

    void Attack()
    {
        if(target != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, dir, 0.3f);
        }
    }
    void SearchTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, 50f, layer);

        if(targets.Length > 0)
        {
            target = targets[Random.Range(0, targets.Length)].transform;
        }
    }
}
