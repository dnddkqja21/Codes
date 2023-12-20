using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    [Header("자식 카운트")]
    public int count;
    [Header("스피드 레이트")]
    public float speedRate;

    void Start()
    {
        // 자식 카운트
        count = transform.childCount;
    }
        
    void Update()
    {
        if(!GameManager.isLive) { return; }

        float totalSpeed = GameManager.globalSpeed * speedRate * Time.deltaTime * -1f;
        transform.Translate(totalSpeed, 0, 0);        
    }
}
