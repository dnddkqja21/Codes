using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class breathWarning : MonoBehaviour
{
    public Image warningImage;

    public Transform warning;

    public Transform warningPos;

    float speed = 0.2f;

    Vector3 offSet = new Vector3(0, 0, 0);

    void Start()
    {
        
    }

    
    void Update()
    {
        ActiveWarning();
        SetWarningPos();
        ReturnZero();
    }

    void SetWarningPos()
    {
        Vector3 tmp = Camera.main.WorldToScreenPoint(warningPos.position + offSet);
        warning.transform.position = tmp;
    }

    void ActiveWarning()
    {
        if(warningImage.enabled == true)
        {
            if (warningImage.fillAmount <= 0.3f)
            {
                warningImage.fillAmount += Time.deltaTime * speed;
            }
        }
    }

    void ReturnZero()
    {
        if(warningImage.fillAmount >= 0.3f)
        {
            warningImage.fillAmount = 0;
            warningImage.enabled = false;
        }
    }
}
