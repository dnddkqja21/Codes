using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버블챗 불가능 지역 설정
/// </summary>

public class NoneBubbleZone : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("BubbleCollider"))
        {
            other.GetComponentInParent<PlayerAttributes>().bubbleAble = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BubbleCollider"))
        {
            other.GetComponentInParent<PlayerAttributes>().bubbleAble = true;
        }
    }    
}
