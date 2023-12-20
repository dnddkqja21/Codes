using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BubblePopup : MonoBehaviour
{
    [SerializeField]
    GameObject bubbleAblePanel;
    [SerializeField]
    TextMeshProUGUI bubbleAble;
    [SerializeField]
    Animator ani;

    bool isBubbleAble = true;

    public void OnBubbleAble()
    {
        isBubbleAble = !isBubbleAble;
        // 오브젝트 활성화
        bubbleAblePanel.SetActive(true);
        // 텍스트 변경
        bubbleAble.text = isBubbleAble ? "Bubble On" : "Bubble Off";
        // 애니메이션 실행
        ani.SetTrigger("DoShow");
        // 비활성
        Invoke("EnactiveOBJ", 1.5f);
    }

    void EnactiveOBJ()
    {
        bubbleAblePanel.SetActive(false);
    }


}
