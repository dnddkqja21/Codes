using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersMagic : MonoBehaviour
{
    public int damage;

    private void Start()
    {
        Delete(5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            Destroy(gameObject, 1f);
        }
    }

    public void Delete(float _time)
    {
        Destroy(gameObject, _time);
    }
}
