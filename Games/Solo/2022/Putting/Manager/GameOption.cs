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
                //Debug.Log("게임 옵션 : null");
                instance = new GameOption();
            }
            return instance;
        }
    }

    // 마스터
    public string id = "kcw0987";
    public string password = "1111";
    public string nickName = "Master";

    // 플레이어
    public MyPuttUser player;
    public List<MyPuttUser> playerList;

    // 기록
    public List<ResultRecord> recordList;

    // 충전 시간 기본 60분 = 60 * 60
    public float chargedTime = 60 * 60; // test용 6분 세팅

    // enums
    // 선택된 메인 모드
    public int selectedMode = 0;

    // 선택된 트레이닝 모드
    public int tranningMode = 1;

    // 직선 모드 단계
    public int straightLevel = 1;

    // 거리 연습 난이도
    public int distanceLevel = 1;

    // 거리 연습 방식
    public int distanceMethod = 1;

    // 기울기 연습 환경
    public int gradientCondition = 1;

    // 기울기 연습 레벨
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

    // 실전 연습 컵 지정
    public int actualCupPoint = 1;

    // 실전 연습 시작 지점
    public int actualStartPoint = 1;

    // 실전 연습 경사 설정
    public int actualGradient = 0;

    // 연습 횟수 (최소 5, 최대 90)
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

    // 진행 횟수
    public int progressCount = 0;

    // 훈련 성공 횟수
    public int successCount = 0;

    // 훈련 카운트 산정에 필요한 변수
    public bool isCountUp = false;

    // 거리(미터)에 따른 훈련 결과  
    public int[] tranningCountForM = { 0, 0, 0, 0 };
    public int[] successCountForM = { 0, 0, 0, 0 };

    // 기울기(레벨)에 따른 훈련 결과
    public int[] tranningCountForL = { 0, 0, 0, 0, 0 };
    public int[] successCountForL = { 0, 0, 0, 0, 0 };

    // 퍼팅 훈련 설정 - 그린 기울기 
    public int[] gradients = { 0, 0, 0, 0 };    

    // 기존 기울기
    public int[] originGradients = { 0, 0, 0, 0 };

    // 상호작용 버튼
    public bool isPressed = false;

    // 불러오기 버튼
    public bool isLoaded = false;

    // 훈련 중
    public bool isTranning = false;

    

    // 생성자
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

    // 소멸자
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

    // 초기화
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
