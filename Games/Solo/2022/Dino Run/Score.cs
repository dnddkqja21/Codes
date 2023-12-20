using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [Header("스코어 구분")]
    public bool isHighScore;

    float highScore;
    Text uiText;

    void Start()
    {
        uiText= GetComponent<Text>();

        if(isHighScore)
        {
            // 플레이어 프립스에서 스코어를 가져와 스트링을 변환, 포맷 설정(소숫점 자르기)
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
