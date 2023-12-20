using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneWarning : MonoBehaviour
{
    float speed = 2f;

    Material mat;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    
    void Update()
    {
        if (transform.localScale.x <= 2.5f)
        {
            float tmp = transform.localScale.x;
            float tmp2 = transform.localScale.z;
            tmp += Time.deltaTime * speed;
            tmp2 += Time.deltaTime * speed;

            transform.localScale = new Vector3(tmp, 0.1f, tmp2);
        }

        if(mat.color.a <= 0.7f)
        {
            Color color = mat.color;
            color.a += Time.deltaTime * 1f;
            mat.color = color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MonstersAttack")
        {   // 파괴되기 전 크기와 알파값을 리셋
            Color color = mat.color;
            color.a = 0.2f;
            mat.color = color;

            transform.localScale = new Vector3(0.3f, 0.1f, 0.3f);

            Destroy(gameObject, 0.1f);
        }
    }
}
