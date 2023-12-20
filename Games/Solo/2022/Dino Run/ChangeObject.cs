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
            // �������� �ε����� ��ġ�ϸ� �Ѷ�.
            transform.GetChild(i).gameObject.SetActive(random == i);
        }
    }
}
