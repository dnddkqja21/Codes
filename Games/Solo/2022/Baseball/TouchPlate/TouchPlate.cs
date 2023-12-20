using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPlate : MonoBehaviour {

    public GameObject obj_01;
    public GameObject obj_02;

    bool bPlay = false;
    float ToggleTime = 0.6f;
    float CheckTime = 0f;


    void OnEnable()
    {
        if (!bPlay)
        {
            obj_01.SetActive(true);
            obj_02.SetActive(false);
            CheckTime = 0f;
            bPlay = true;
        }
    }

    void OnDisable()
    {
        bPlay = false;
    }

    void Update()
    {
        if (bPlay)
        {
            CheckTime += Time.deltaTime;

            if (CheckTime >= ToggleTime)
            {
                CheckTime = 0f;

                if (obj_01.activeSelf)
                {
                    obj_01.SetActive(false);
                    obj_02.SetActive(true);
                }
                else
                {
                    obj_01.SetActive(true);
                    obj_02.SetActive(false);
                }
            }
        }
    }
}
