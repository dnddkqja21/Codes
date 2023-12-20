using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Middle_Button : MonoBehaviour
{
    public Image Low;
    public Image Middle;
    public Image High;

    void Start()
    {

    }


    void Update()
    {

    }
    public void SwitchOn()
    {
        Low.gameObject.SetActive(false);
        Middle.gameObject.SetActive(true);
        High.gameObject.SetActive(false);
    }
}
