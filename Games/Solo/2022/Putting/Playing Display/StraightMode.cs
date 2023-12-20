using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StraightMode : MonoBehaviour
{
    [Header("���̵� ����")]
    public RectTransform[] guideLines;
    [Header("Ȧ��")]
    public GameObject[] holecups;

    float maxWidth = 700;
    float width = 0;
    float height = 0;

    int guideLineIndex = 0;

    void Start()
    {
        string[] temp = EnumToData.Instance.StraightRule(GameOption.Instance.straightLevel).Split(",");
        width = int.Parse(temp[0]);
        height = int.Parse(temp[1]);

        // ������ �����Ϳ��� 2���� ������ �����ؾ� ��
        width *= 2;
        height *= 2;

        // ���� �Ʒ� �ܰ迡 ���� ���̵� ���� ����
        for (int i = 0; i < guideLines.Length; i++)
        {            
            guideLines[i].sizeDelta = new Vector2(maxWidth, height);
            guideLines[i].GetComponent<Image>().fillAmount = width / maxWidth;
        }
        
        InitRound();
    }
   

    public void InitRound()
    {        
        // 1 ~ 7 ����(3�� ġ�� �ٸ� ����)
        // 3 x 3����(9����) ����ߴٸ� �� ���� �˾� (9�� ��� ����)
        if(GameOption.Instance.straightLevel != (int)StraightLevel.EIGHT)
        {
            // ��� ��Ȱ��ȭ
            for (int i = 0; i < guideLines.Length; i++)
            {
                guideLines[i].gameObject.SetActive(false);                
            }  

            // ���� Ƚ���� 0�� �ƴϰ�, 3�� ����� �ε��� ����
            if (GameOption.Instance.progressCount  % 3 == 0 && GameOption.Instance.progressCount != 0)
            {
                guideLineIndex++;
                //Debug.Log("���̵� ���� �ε��� : "+guideLineIndex);
            }

            // 9�� ��� ����� �ε��� �ʱ�ȭ, �� ���� �˾� ���
            if(GameOption.Instance.progressCount % 9 == 0 && GameOption.Instance.progressCount != 0)
            {
                guideLineIndex = 0;
                PopupManager.Instance.cleanBallPopup.SetActive(true);
            }
            guideLines[guideLineIndex].gameObject.SetActive(true);
            
        }
        else
        {
            // 8 ����(���� Ȧ�� ����)
            for (int i = 0; i < guideLines.Length; i++)
            {            
                guideLines[i].gameObject.SetActive(false);
                holecups[i].SetActive(false);            
            }

            int random = Random.Range(0, 3);
            guideLines[random].gameObject.SetActive(true);
            holecups[random].SetActive(true);
            
            if (GameOption.Instance.progressCount % 9 == 0 && GameOption.Instance.progressCount != 0)
            {                
                PopupManager.Instance.cleanBallPopup.SetActive(true);
            }
        }
    }
}
