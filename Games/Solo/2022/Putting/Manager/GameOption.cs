using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MyPuttUser
{
    public string id;
    public string password;
    public string nickName;
    public bool isGuest = false;
}

public class GameOption
{
    private static GameOption instance = null;
    public static GameOption Instance
    {
        set { instance = value; }
        get
        {
            if (instance == null)
            {
                //Debug.Log("���� �ɼ� : null");
                instance = new GameOption();
            }
            return instance;
        }
    }

    // ������
    public string id = "kcw0987";
    public string password = "1111";
    public string nickName = "Master";

    // �÷��̾�
    public MyPuttUser player;
    public List<MyPuttUser> playerList;

    // ���
    public List<ResultRecord> recordList;

    // ���� �ð� �⺻ 60�� = 60 * 60
    public float chargedTime = 60 * 60; // test�� 6�� ����

    // enums
    // ���õ� ���� ���
    public int selectedMode = 0;

    // ���õ� Ʈ���̴� ���
    public int tranningMode = 1;

    // ���� ��� �ܰ�
    public int straightLevel = 1;

    // �Ÿ� ���� ���̵�
    public int distanceLevel = 1;

    // �Ÿ� ���� ���
    public int distanceMethod = 1;

    // ���� ���� ȯ��
    public int gradientCondition = 1;

    // ���� ���� ����
    private int gradientLevel = 1;
    int gLMin = 1;
    int gLMax = 5;
    public int GradientLevel
    {
        set
        {
            value = Mathf.Clamp(value, gLMin, gLMax);
            gradientLevel = value;
        }
        get
        {
            return gradientLevel;
        }
    }

    // ���� ���� �� ����
    public int actualCupPoint = 1;

    // ���� ���� ���� ����
    public int actualStartPoint = 1;

    // ���� ���� ��� ����
    public int actualGradient = 0;

    // ���� Ƚ�� (�ּ� 5, �ִ� 90)
    private int tranningCount = 10;
    const int tCMax = 90;
    const int tCMin = 5;
    public int TranningCount
    {
        set
        {
            value = Mathf.Clamp(value, tCMin, tCMax);
            tranningCount = value;
        }
        get { return tranningCount; }
    }

    // ���� Ƚ��
    public int progressCount = 0;

    // �Ʒ� ���� Ƚ��
    public int successCount = 0;

    // �Ʒ� ī��Ʈ ������ �ʿ��� ����
    public bool isCountUp = false;

    // �Ÿ�(����)�� ���� �Ʒ� ���  
    public int[] tranningCountForM = { 0, 0, 0, 0 };
    public int[] successCountForM = { 0, 0, 0, 0 };

    // ����(����)�� ���� �Ʒ� ���
    public int[] tranningCountForL = { 0, 0, 0, 0, 0 };
    public int[] successCountForL = { 0, 0, 0, 0, 0 };

    // ���� �Ʒ� ���� - �׸� ���� 
    public int[] gradients = { 0, 0, 0, 0 };    

    // ���� ����
    public int[] originGradients = { 0, 0, 0, 0 };

    // ��ȣ�ۿ� ��ư
    public bool isPressed = false;

    // �ҷ����� ��ư
    public bool isLoaded = false;

    // �Ʒ� ��
    public bool isTranning = false;

    

    // ������
    private GameOption()
    {
        if (playerList == null)
        {
            playerList = new List<MyPuttUser>();
        }
        if(recordList == null)
        {
            recordList = new List<ResultRecord>();
        }
    }

    // �Ҹ���
    ~GameOption()
    {
        ClearUser();
    }

    public void ClearUser()
    {
        if (playerList != null)
        {
            playerList.Clear();
        }
        if(recordList != null)
        {
            recordList.Clear();
        }
    }

    // �ʱ�ȭ
    public void ResetSettings()
    {
        selectedMode = 0;
        tranningMode = 1;
        straightLevel = 1;
        distanceLevel = 1;
        distanceMethod = 1;
        gradientCondition = 1;
        gradientLevel = 1;
        actualCupPoint = 1;
        actualStartPoint = 1;
        actualGradient = 0;
        TranningCount = 10;
        progressCount = 0;
        successCount = 0;
        for (int i = 0; i < tranningCountForM.Length; i++)
        {
            tranningCountForM[i] = 0;
            successCountForM[i] = 0;
        }
        for (int i = 0; i < tranningCountForL.Length; i++)
        {
            tranningCountForL[i] = 0;
            successCountForL[i] = 0;
        }
        for (int i = 0; i < gradients.Length; i++)
        {
            gradients[i] = 0;
            originGradients[i] = 0;
        }
        isPressed = false;
        isLoaded = false;
        isTranning = false;
    }
}
