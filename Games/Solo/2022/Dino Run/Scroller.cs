using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    [Header("�ڽ� ī��Ʈ")]
    public int count;
    [Header("���ǵ� ����Ʈ")]
    public float speedRate;

    void Start()
    {
        // �ڽ� ī��Ʈ
        count = transform.childCount;
    }
        
    void Update()
    {
        if(!GameManager.isLive) { return; }

        float totalSpeed = GameManager.globalSpeed * speedRate * Time.deltaTime * -1f;
        transform.Translate(totalSpeed, 0, 0);        
    }
}
