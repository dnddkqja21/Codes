using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpAndDown : MonoBehaviour
{
    [SerializeField]
    Sprite[] arrows;
    [SerializeField]
    Animator ani;

    Image upAndDown;
    bool isUp;

    void Start()
    {
        upAndDown = GetComponent<Image>();
        isUp = GetComponent<Animator>().GetBool("IsUp");
    }

    public void OnUpAndDown()
    {
        isUp = !isUp;
        ani.SetBool("IsUp", isUp);

        if(arrows.Length != 0)
        {
            Invoke("ChangeSprite", 1.5f);
        }
    }

    void ChangeSprite()
    {   
        if(upAndDown != null)
        {
            upAndDown.sprite = isUp ? arrows[1] : arrows[0];
        }
    }
}
