using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeObject : MonoBehaviour
{
    public GameObject[] objs;

    public void Change()
    {
        int random = Random.Range(0, objs.Length);

        for (int i = 0; i < objs.Length; i++)
        {
            // 랜덤값과 인덱스가 일치하면 켜라.
            transform.GetChild(i).gameObject.SetActive(random == i);
        }
    }
}
