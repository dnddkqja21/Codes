using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [Header("���ھ� ����")]
    public bool isHighScore;

    float highScore;
    Text uiText;

    void Start()
    {
        uiText= GetComponent<Text>();

        if(isHighScore)
        {
            // �÷��̾� ���������� ���ھ ������ ��Ʈ���� ��ȯ, ���� ����(�Ҽ��� �ڸ���)
            highScore = PlayerPrefs.GetFloat("Score");
            uiText.text = highScore.ToString("F0");
        }
    }

    void LateUpdate()
    {
        if(!GameManager.isLive) { return; }

        if(isHighScore && GameManager.score < highScore) { return; }

        uiText.text = GameManager.score.ToString("F0");
    }
}
