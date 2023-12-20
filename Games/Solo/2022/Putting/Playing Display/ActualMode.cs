using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualMode : MonoBehaviour
{
    [Header("���� ��� ���� ������Ʈ")]
    public GameObject fixedStartPoint;
    [Header("���� ��� �� ������Ʈ")]
    public GameObject fixedCupPoint;
    [Header("���� ��� ���� ������Ʈ")]
    public GameObject randomStartPoint;
    [Header("���� ��� �� ������Ʈ")]
    public GameObject randomCupPoint;
    [Header("���� ��� ���õ� ���� ����")]
    public GameObject[] selectedStartPoints;
    [Header("���� ��� ���õ� Ȧ ��")]
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
