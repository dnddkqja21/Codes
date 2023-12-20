using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Text;
using TMPro;

public enum MotorNumber
{
    ZERO,
    ONE,
    TWO,
    THREE
}

// 기본 구조 STX + Command[2Byte] + Data[NByte] + ETX
public class PuttingGreenProtocol : MonoBehaviour
{
    [Header("높이 텍스트")]
    public TextMeshProUGUI[] heightTexts;
    [Header("인풋 필드")]
    public TextMeshProUGUI inputField;
    [Header("모든 높이 변경 인풋 필드")]
    public TextMeshProUGUI inputFieldAll;
    [Header("에러 팝업")]
    public GameObject errorInput;

    public SerialPort serialPort;
    int baudRate = 115200;
    int dataBit = 8;

    //string portName = "COM3";
    byte motor = 0x53;

    public string config;

    public readonly byte STX = 0x02;
    public readonly byte ETX = 0x03;

    Thread thread;

    // 하드웨어에서 최대 16바이트를 전송해준다.   
    byte[] buffer = new byte[1024];

    public string motorHeight = string.Empty;
    public int motorNumber = 0;

    // 퍼블릭 조심하자...인스펙터 무조건 확인하기!
    public byte[] motorNo =
    {
        0x30,   // 0번 모터
        0x31,   // 1번 모터
        0x32,   // 2번 모터
        0x33,   // 3번 모터
        0x42    // 모든 모터
    };
    // 퍼블릭 조심하자...인스펙터 무조건 확인하기!
    public byte[] greenHeight =
    {
        0x30,   // 0
        0x31,   // 1
        0x32,   // 2
        0x33,   // 3
        0x34,   // 4
        0x35,   // 5
        0x36,   // 6
        0x37,   // 7
        0x38,   // 8
        0x39    // 9
    };


    private void Start()
    {
        config = Application.dataPath + "/StreamingAssets/config.txt";
        ThreadStart ts = new ThreadStart(Init);
        thread = new Thread(ts);
        thread.Start();


        // 시리얼 통신과 연결되기 전에 입력을 하지 않기 위해 1초 쉼
        Thread.Sleep(1500);

        // 시작 시 005로 세팅
        ResetAllHeight();
    }

    private void Update()
    {
        //ResetConnect();
    }

    void Init()
    {
        int tryNum = 0;

        while (tryNum < 10)
        {
            try
            {
                serialPort = new SerialPort(File.ReadAllText(config), baudRate, Parity.None, dataBit, StopBits.One);
                //serialPort = new SerialPort("COM2", baudRate, Parity.None, dataBit, StopBits.One);
                //serialPort.DtrEnable = true;
                //serialPort.WriteTimeout = 1500;
                serialPort.Open();

                Debug.Log("디바이스 연결");
                break;
            }
            catch
            {
                Thread.Sleep(2000);
                Debug.Log("연결 실패");
                tryNum++;
            }
        }

        if (tryNum >= 10)
            return;

        while (!serialPort.IsOpen)
        {
            Thread.Sleep(1000);
        }



        while (true)
        {
            Array.Clear(buffer, 0x0, buffer.Length);
            // 응답이 오면 앞에 3바이트 읽어서 각각의 모터인지 모든 모터인지 판단
            serialPort.Read(buffer, 0, buffer.Length);
            Receive();
            Thread.Sleep(500);
        }
    }

    void Receive()
    {
        // 첫번 째 버퍼가 널이거나 두번 째 버퍼가 R(결과)가 아니라면 리턴
        //Debug.LogError(Encoding.Default.GetString(buffer));
        
        if (buffer[0] != 0x52)
            return;
        
        // 받은 두번째 버퍼가 모든 모터라면
        if (buffer[1] == motorNo[4])
        {
            byte[] temp = new byte[3];
            for (int i = 0; i < 3; i++)
            {
                temp[i] = buffer[i + 2];
            }
            string answer = Encoding.Default.GetString(temp);
            Debug.LogError("모든 모터의 높이 : "  + answer);
        }
        else
        {
            /* for문에서 사용할 때
            int index = 0;
            serialPort.Read(height, offsett, index+= 4)
            serialPort.Read(height, index,  4);
            index += 4; 
            */

            byte[] temp = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                temp[i] = buffer[i + 1];
            }
            string answer = Encoding.Default.GetString(temp);
            
            Debug.LogError(answer.Substring(0, 1) + "번 모터의 높이 : " + answer.Substring(1,3));
        }
    }

    void Send(byte motorNo, byte greenHeight, byte greenHeight2, byte greenHeight3)
    {
        if (!serialPort.IsOpen)
            return;
        byte CS = (byte)(STX + motor + motorNo + greenHeight + greenHeight2 + greenHeight3 + ETX);
        var message = new byte[8] { STX, motor, motorNo, greenHeight, greenHeight2, greenHeight3, ETX, CS };
        serialPort.Write(message, 0, 8);
    }

    void SendAll(   byte greenHeight1, byte greenHeight2, byte greenHeight3,
                    byte greenHeight4, byte greenHeight5, byte greenHeight6,
                    byte greenHeight7, byte greenHeight8, byte greenHeight9,
                    byte greenHeight10, byte greenHeight11, byte greenHeight12)
    {
        if (!serialPort.IsOpen)
            return;
        byte CS = (byte)(STX + motor + motorNo[4] + greenHeight1 + greenHeight2 + greenHeight3 + 
                                                    greenHeight4 + greenHeight5 + greenHeight6 +
                                                    greenHeight7 + greenHeight8 + greenHeight9 +
                                                    greenHeight10 + greenHeight11 + greenHeight12 + ETX);

        var message = new byte[17] { STX, motor, motorNo[4], greenHeight1, greenHeight2, greenHeight3,
                                                            greenHeight4, greenHeight5, greenHeight6,
                                                            greenHeight7, greenHeight8, greenHeight9,
                                                            greenHeight10, greenHeight11, greenHeight12, ETX, CS };
        serialPort.Write(message, 0, 17);
    }
       


    private void ResetConnect()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (serialPort.IsOpen)
                serialPort.Close();

            if (thread.IsAlive)
                thread.Abort();
            //thread.
            //thread.Start();
        }
    }


    void ReadyToSerialPort()
    {

    }

    //public byte SetCommand(byte motorNo, byte position)
    //{
    //    byte command = (byte)(STX + motorNo + position + ETX);
    //    return command;
    //}

    //public string[] StringToHex(string strData)
    //{
    //    string[] resultHex = { "", "", ""};
    //    byte[] arr_byteStr = Encoding.Default.GetBytes(strData);                

    //    for (int i = 0; i < arr_byteStr.Length; i++)
    //    {
    //        resultHex[i] = string.Format("{0:X2}", arr_byteStr[i]);
    //    }

    //    return resultHex;
    //}

    public int[] StringToInt(string num)
    {        
        int[] result = { 0, 0, 0 };
        
        // 인풋 필드의 길이는 마지막 공백까지 합하여 +1이 된다. 포문에서 -1처리 해야 함

        if(num.Length >= 4)
        {
            for (int i = 0; i < num.Length -1; i++)
            {
                //Debug.Log(num[i].ToString());
                result[i] = int.Parse(num[i].ToString());
            }        
        }

        return result;
    }

    public void ChangeHeight()
    {
        
        int[] heightIndex = StringToInt(motorHeight);

        //Debug.Log(heightIndex);

        foreach (var item in heightIndex)
        {
            try
            {
                //Debug.Log(item);
               // Debug.Log(greenHeight[item].ToString("X2"));
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        Send(motorNo[motorNumber], greenHeight[heightIndex[0]], greenHeight[heightIndex[1]], greenHeight[heightIndex[2]]);
    }

    public void ChangeHeightAll()
    {
        motorHeight = inputFieldAll.text;

        #region 잘못된 값 입력 시 팝업 띄움 
        //if (int.Parse(inputFieldAll.text.Substring(0, inputFieldAll.text.Length - 1)) < 5 || int.Parse(inputFieldAll.text.Substring(0, inputFieldAll.text.Length - 1)) > 120)
        //{
        //    Debug.Log("잘못된 수치 입력 : 범위 005~120");

        //    errorInput.SetActive(true);
        //    return;
        //}
        #endregion

        if (int.Parse(inputFieldAll.text.Substring(0, inputFieldAll.text.Length - 1)) < 5)
        {
            motorHeight = "005" + "\0";
        }
        else if (int.Parse(inputFieldAll.text.Substring(0, inputFieldAll.text.Length - 1)) > 120)
        {
            motorHeight = "120" + "\0";
        }

        PadLeftZero();

        int[] heightIndex = StringToInt(motorHeight);

        SendAll(greenHeight[heightIndex[0]], greenHeight[heightIndex[1]], greenHeight[heightIndex[2]],
                greenHeight[heightIndex[0]], greenHeight[heightIndex[1]], greenHeight[heightIndex[2]],
                greenHeight[heightIndex[0]], greenHeight[heightIndex[1]], greenHeight[heightIndex[2]],
                greenHeight[heightIndex[0]], greenHeight[heightIndex[1]], greenHeight[heightIndex[2]]);

        for (int i = 0; i < heightTexts.Length; i++)
        {
            heightTexts[i].text = motorHeight;
        }
    }

    public void EachMotorChangeText()
    {
        #region 범위 밖의 값 입력 시 에러 메세지 띄우는 코드
        //if (int.Parse(inputField.text.Substring(0, inputField.text.Length -1)) < 5 || int.Parse(inputField.text.Substring(0, inputField.text.Length -1)) > 120)
        //{
        //    Debug.Log("잘못된 수치 입력 : 범위 005~120");
        //    errorInput.SetActive(true);
        //    return;
        //}
        #endregion

        motorHeight = inputField.text;
        //Debug.Log(motorHeight.Length);

        if(int.Parse(inputField.text.Substring(0, inputField.text.Length -1)) < 5)
        {
            motorHeight = "005" + "\0";
        }
        else if(int.Parse(inputField.text.Substring(0, inputField.text.Length -1)) > 120)
        {
            motorHeight = "120" + "\0";
        }

        PadLeftZero();

        switch (motorNumber)
        {
            case (int)MotorNumber.ZERO:
                heightTexts[(int)MotorNumber.ZERO].text = motorHeight;
                break;

            case (int)MotorNumber.ONE:
                heightTexts[(int)MotorNumber.ONE].text = motorHeight;
                break;

            case (int)MotorNumber.TWO:
                heightTexts[(int)MotorNumber.TWO].text = motorHeight;
                break;

            case (int)MotorNumber.THREE:
                heightTexts[(int)MotorNumber.THREE].text = motorHeight;
                break;

        }        
    }

    public void ResetAndClose()
    {
        inputField.text = "";        
    }

    public void ResetAllHeight()
    {
        SendAll(greenHeight[0], greenHeight[0], greenHeight[5],
                greenHeight[0], greenHeight[0], greenHeight[5],
                greenHeight[0], greenHeight[0], greenHeight[5],
                greenHeight[0], greenHeight[0], greenHeight[5]);

        for (int i = 0; i < heightTexts.Length; i++)
        {
            heightTexts[i].text = "005";
        }
    }

    public void Exit()
    {
        ResetAllHeight();
        Application.Quit();
    }

    public void PadLeftZero()
    {
        if(motorHeight.Length < 4)
        {
            Debug.Log("입력 값 : " + motorHeight);
            Debug.Log("길이가 4보다 작다");
            string temp = motorHeight.PadLeft(4, '0');
            motorHeight = temp;
            Debug.Log("수정된 값 : " + motorHeight);
        }
    }
}
