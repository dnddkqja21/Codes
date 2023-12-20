using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DistanceMode : MonoBehaviour
{
    [Header("���̵� ����")]
    public RectTransform guideLine;
    [Header("����Ʈ �����̴�")]
    public RectTransform setPoint;
    [Header("��ǥ �Ÿ�")]
    public RectTransform targetDistance;

    // ���̵��� ���� ����Ʈ
    int easyMin = 2;
    int easyMax = 13;
    int normalMin = 4;
    int normalMax = 15;
    int hardMin = 6;
    int hardMax = 16;

    // ���̵� ���� �ִ� ����
    float maxWidth = 700;

    // ���̵� �� ����Ʈ ����
    public List<float> points = new List<float>();

    // ����Ʈ �ε���
    int pointIndex = 0;

    // ���̵� �� �ݰ� (������ �� �ٽ� ����)
    float successRadius = 0;

    // �����̴�, ��ǥ�Ÿ� �� ��ġ
    Vector3 originPosSlider = Vector3.zero;
    Vector3 originPosTarget = Vector3.zero;

    // �̵��� ��ġ
    Vector3 moveToPosSlider = Vector3.zero;
    Vector3 moveToPosTarget = Vector3.zero;

    // ���� ��� ���� ������ y��
    float offsetYFixed = 135;

    // ���� ��� ����Ʈ Ŭ����
    bool isClear = false;

    // ���̵� ���� ����ġ
    Vector3 originPosGuideLine = Vector3.zero;

    // ���̵� ���� �̵� ��ġ
    Vector3 moveToPosGuideLine = Vector3.zero;

    // ����, ��ȯ ��� ������ y��
    float offsetYOther = 131;

    // ��ġ ����
    bool isChanged = false;

    void Start()
    {
        originPosSlider = setPoint.transform.position;
        originPosTarget = targetDistance.transform.position;
        originPosGuideLine = guideLine.transform.position;

        moveToPosSlider = originPosSlider + new Vector3(0, offsetYFixed, 0);
        moveToPosTarget = originPosTarget - new Vector3(0, offsetYFixed, 0);
        moveToPosGuideLine = originPosGuideLine + new Vector3(0, offsetYOther, 0);              

        SetLevel();

        InitRound();
    }

    public void InitRound()
    {
        // ���� ���� 
        // ��ǥ �Ÿ�
        targetDistance.gameObject.SetActive(true);

        // 6���� ���� �� ����
        if (GameOption.Instance.progressCount % 6 == 0 && GameOption.Instance.progressCount != 0)
        {
            PopupManager.Instance.cleanBallPopup.SetActive(true);
        }

        // ��� �� �۵�
        switch (GameOption.Instance.distanceMethod)
        {
            case (int)DistanceMethod.FIXED:
                setPoint.gameObject.SetActive(true);

                // ���� ��忡���� SetLevel();�� ��Ģ�� ������� �����Ƿ� �ѹ��� Ŭ���� �Ѵ�.
                if (isClear == false)
                {
                    isClear = true;
                    points.Clear();
                }
                
                break;

            case (int)DistanceMethod.ROTATION:
                guideLine.gameObject.SetActive(true);

                guideLine.GetComponent<Image>().fillAmount = (points[pointIndex] * 2) / maxWidth;
                targetDistance.GetComponentInChildren<TextMeshProUGUI>().text = "��ǥ �Ÿ� : " + points[pointIndex].ToString() + "cm";
                pointIndex++;
                if(pointIndex > points.Count -1)
                {
                    pointIndex = 0;
                }   
                break;

            case (int)DistanceMethod.RANDOM:
                guideLine.gameObject.SetActive(true);

                pointIndex = Random.Range(0, points.Count);

                guideLine.GetComponent<Image>().fillAmount = (points[pointIndex] * 2) / maxWidth;
                targetDistance.GetComponentInChildren<TextMeshProUGUI>().text = "��ǥ �Ÿ� : " + points[pointIndex].ToString() + "cm";

                break;
        } 

        // ���� ��� �ƴϰ�, ���� Ƚ���� 0�� �ƴϰ�, 3�� ����� ���̵� ������ ��ġ �ٲ�
        if (GameOption.Instance.progressCount % 3 == 0 && GameOption.Instance.progressCount != 0 && GameOption.Instance.distanceMethod != (int)DistanceMethod.FIXED)
        {     
            isChanged = !isChanged;
            if (isChanged)
            {
                guideLine.transform.position = moveToPosGuideLine;
                targetDistance.transform.position = moveToPosTarget;
            }
            else
            {
                guideLine.transform.position = originPosGuideLine;
                targetDistance.transform.position = originPosTarget;
            }
        }
        // ���� ����� ���� ����Ʈ �����̴� ��ġ ����
        else if(GameOption.Instance.progressCount % 3 == 0 && GameOption.Instance.progressCount != 0 && GameOption.Instance.distanceMethod == (int)DistanceMethod.FIXED)
        {
            isChanged = !isChanged;
            if (isChanged)
            {
                setPoint.transform.position = moveToPosSlider;
                targetDistance.transform.position = moveToPosTarget;
            }
            else
            {
                setPoint.transform.position = originPosSlider;
                targetDistance.transform.position = originPosTarget;
            }
        }
    }

    // ���̵� ����
    public void SetLevel()
    {
        switch(GameOption.Instance.distanceLevel)
        {
            case (int)DistanceLevel.EASY:
                // 2 ~ 13 ����Ʈ
                for (int i = easyMin; i <= easyMax; i++)
                {
                   points.Add(EnumToData.Instance.DistanceRule(i));
                }
                successRadius = 20;
                break;

            case (int)DistanceLevel.NORMAL:
                // 4 ~ 15 ����Ʈ
                for (int i = normalMin; i <= normalMax; i++)
                {
                    points.Add(EnumToData.Instance.DistanceRule(i));
                }
                successRadius = 15;
                break;

            case (int)DistanceLevel.HARD:
                // 6 ~ 16 ����Ʈ
                for (int i = hardMin; i <= hardMax; i++)
                {
                    points.Add(EnumToData.Instance.DistanceRule(i));
                }
                successRadius = 10;
                break;
        }
    }

    
    public void AddPointEachRound()
    {
        // ���� ����� ���� ���� �ٸ���.
        if (GameOption.Instance.distanceMethod == (int)DistanceMethod.FIXED)
        {
            // �� ���帶�� ���� �Ÿ��� ����Ʈ�� �߰�
            string stringToFloat = targetDistance.GetComponentInChildren<TextMeshProUGUI>().text;
            // ��ǥ �Ÿ� : 000cm (7���� ���� �� cm�� ��������)                      
            points.Add(float.Parse(stringToFloat.Substring(7).Replace("cm", "")));
        }        

        if (points[pointIndex] >= 0f && points[pointIndex] < 100f)
        {
            GameOption.Instance.tranningCountForM[0]++;
            if (GameOption.Instance.isCountUp)
            {
                GameOption.Instance.successCountForM[0]++;
            }
        }
        else if (points[pointIndex] >= 100f && points[pointIndex] < 200f)
        {
            GameOption.Instance.tranningCountForM[1]++;
            if (GameOption.Instance.isCountUp)
            {
                GameOption.Instance.successCountForM[1]++;
            }
        }
        else if (points[pointIndex] >= 200f && points[pointIndex] < 300f)
        {
            GameOption.Instance.tranningCountForM[2]++;
            if (GameOption.Instance.isCountUp)
            {
                GameOption.Instance.successCountForM[2]++;
            }
        }
        else if (points[pointIndex] >= 300f && points[pointIndex] < 400f)
        {
            GameOption.Instance.tranningCountForM[3]++;
            if (GameOption.Instance.isCountUp)
            {
                GameOption.Instance.successCountForM[3]++;
            }
        }
        pointIndex++;
        //Debug.Log(pointIndex + "/ " + point[pointIndex]);
        if (pointIndex > points.Count - 1)
        {
            pointIndex = 0;
        }
        
        GameOption.Instance.isCountUp = false;
    }
}
