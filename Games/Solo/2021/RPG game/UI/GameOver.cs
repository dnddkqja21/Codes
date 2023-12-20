using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Image backGround;
    Color originColor;


    void Start()
    {        
        originColor = backGround.color;
        originColor.a = 0;
        InvokeRepeating("PlusAlpha", 0.1f, 0.05f);
    }

    void Update()
    {

    }

    public void PlusAlpha()
    {

        originColor.a += Time.deltaTime;
        backGround.color = originColor;
        if (originColor.a >= 1f)
        {
            CancelInvoke("PlusAlpha");
        }
    }
}
