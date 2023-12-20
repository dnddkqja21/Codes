using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EachMotor : MonoBehaviour
{
    [Header("모터 번호")]
    public MotorNumber motorNumber;
    [Header("인풋 필드")]
    public GameObject inputField;
    [Header("모터 번호 텍스트")]
    public TextMeshProUGUI motorNo;
    bool isShow = false;
    [Header("프로토콜")]
    public PuttingGreenProtocol protocol;

    void Start()
    {
        
    }

    
    void Update()
    {
        //// 입력 제한
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
        motorNo.text = (int)motorNumber + " 번 모터";
        protocol.motorNumber = (int)motorNumber;
    }

    
}
