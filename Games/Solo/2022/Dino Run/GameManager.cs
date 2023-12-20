using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const float ORIGIN_SPEED = 3f;

    // 전역 스피드
    public static float globalSpeed;
    // 스코어
    public static float score;
    // 게임 중
    public static bool isLive;
    // 1%씩 난이도 증가
    float scoreRate = 0.01f;

    [Header("게임 난이도")]
    public float gameLevel = 3f;
    [Header("게임오버 UI")]
    public GameObject gameOverUI;

    void Awake()
    {
        isLive= true;

        if(!PlayerPrefs.HasKey("Score"))
        {
            PlayerPrefs.SetFloat("Score", 0);
        }
    }

    void Update()
    {
        if(!isLive) { return; }

        score += Time.deltaTime * gameLevel;
        // 점수가 오를 수록 스피드가 빨라져 난이도에 영향을 주게 됨
        globalSpeed = ORIGIN_SPEED + score * scoreRate; 
    }

    public void GameOver()
    {
        isLive = false;
        gameOverUI.SetActive(true);

        // 게임오버 시 스코어 가져와서 현재 스코어와 비교해서 높은 것을 다시 저장
        float highScore = PlayerPrefs.GetFloat("Score");
        PlayerPrefs.SetFloat("Score", Mathf.Max(highScore, score));
    }

    public void Restart()
    {        
        SceneManager.LoadScene(0);
        score = 0;
        isLive = true;
    }
}
