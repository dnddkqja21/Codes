using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualMode : MonoBehaviour
{
    [Header("고정 모드 시작 오브젝트")]
    public GameObject fixedStartPoint;
    [Header("고정 모드 컵 오브젝트")]
    public GameObject fixedCupPoint;
    [Header("랜덤 모드 시작 오브젝트")]
    public GameObject randomStartPoint;
    [Header("랜덤 모드 컵 오브젝트")]
    public GameObject randomCupPoint;
    [Header("랜덤 모드 선택된 시작 지점")]
    public GameObject[] selectedStartPoints;
    [Header("랜덤 모드 선택된 홀 컵")]
    public GameObject[] selectedCupPoints;


    void Start()
    {
        SetStartAndCupPoint();
        InitRound();
    }

    
    void Update()
    {
        
    }

    public void InitRound()
    {
        for (int i = 0; i < selectedStartPoints.Length; i++)
        {
            selectedStartPoints[i].SetActive(false);
        }
        for (int i = 0; i < selectedCupPoints.Length; i++)
        {
            selectedCupPoints[i].SetActive(false);
        }

        var option = GameOption.Instance;

        switch (option.actualStartPoint)
        {
            case (int)ActualStartPoint.FIXED:
                
                break;
            case (int)ActualStartPoint.RANDOM:
                int random = Random.Range(0, selectedStartPoints.Length);
                selectedStartPoints[random].SetActive(true);
                break;
        }
        switch (option.actualCupPoint)
        {
            case (int)ActualCupPoint.FIXED:
                
                break;
            case (int)ActualCupPoint.RANDOM:
                int random = Random.Range(0, selectedCupPoints.Length);
                selectedCupPoints[random].SetActive(true);
                break;
        }
    }

    public void SetStartAndCupPoint()
    {
        var option = GameOption.Instance;

        switch (option.actualStartPoint)
        {
            case (int)ActualStartPoint.FIXED:
                fixedStartPoint.SetActive(true);
                break;
            case (int)ActualStartPoint.RANDOM:
                randomStartPoint.SetActive(true);
                break;
        }
        switch (option.actualCupPoint)
        {
            case (int)ActualCupPoint.FIXED:
                fixedCupPoint.SetActive(true);
                break;
            case (int)ActualCupPoint.RANDOM:
                randomCupPoint.SetActive(true);
                break;
        }
    }
}
