using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EachMotor : MonoBehaviour
{
    [Header("���� ��ȣ")]
    public MotorNumber motorNumber;
    [Header("��ǲ �ʵ�")]
    public GameObject inputField;
    [Header("���� ��ȣ �ؽ�Ʈ")]
    public TextMeshProUGUI motorNo;
    bool isShow = false;
    [Header("��������")]
    public PuttingGreenProtocol protocol;

    void Start()
    {
        
    }

    
    void Update()
    {
        //// �Է� ����
        //if(inputField.text != "")
        //{
        //    if(int.Parse( inputField.text) < 2)
        //    {
        //        inputField.text = "2";
        //    }
        //    else if(int.Parse(inputField.text) > 120)
        //    {
        //        inputField.text = "120";
        //    }
        //}        
    }
    
    public void ShowInput()
    {
        isShow = !isShow;        
        
        inputField.gameObject.SetActive(isShow);
        motorNo.text = (int)motorNumber + " �� ����";
        protocol.motorNumber = (int)motorNumber;
    }

    
}
