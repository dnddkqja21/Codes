using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    // 총 시간 (60초 안에 가장 높은 점수 내야 함)
    public static float maxTime = 60f;
    public static float playTime;
    public static float comboTime = 1f;
    public static float failTime;
    public static bool isGameOver = true;

    public static float score;
    public static int hit;
    public static int combo;

    public GameObject uiInfo;
    public GameObject comboTimer;
    public GameObject uiSelect;
    public GameObject uiGameOver;
    public GameObject uiGameStart;
    public GameObject bestRecord;

    public ParticleSystem successEffect;
    public Animator scoreAni;
    public Animator comboAni;

    void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        // 타겟 프레임레이트를 고정
        Application.targetFrameRate = 60;

        instance = this;
        
        InitGame();
    }

    void Update()
    {
        if(isGameOver)
        {
            return;
        }
        GameTimer();
    }

    void InitGame()
    {
        playTime = 0;        
        isGameOver = true;
        score = 0;
        hit = 0;
        combo = 0;

        // 점수 저장, 처음 실행했다면 0점
        if(!PlayerPrefs.HasKey("Score"))
        {
            PlayerPrefs.SetFloat("Score", 0f);
        }
    }

    void GameTimer()
    {
        playTime += Time.deltaTime;

        if(playTime > maxTime)
        {
            GameOver();
        }
    }

    public void GameStart()
    {
        isGameOver = false;
        uiInfo.SetActive(true);
        uiSelect.SetActive(true);
        uiGameStart.SetActive(false);

        // 정적 메모리에 태운 코루틴은 찾아가서 호출해야 한다.
        instance.StartCoroutine(instance.ComboTime());
        SoundManager.PlaySound("Start");
        SoundManager.BGMStart();
    }

    void GameOver()
    {
        isGameOver = true;
        uiInfo.SetActive(false);
        uiSelect.SetActive(false);
        uiGameOver.SetActive(true);

        // 베스트 기록 비교하여 저장
        float bestScore = PlayerPrefs.GetFloat("Score");
        if(score > bestScore)
        {
            PlayerPrefs.SetFloat("Score", score);
            bestRecord.SetActive(true);
        }
        SoundManager.PlaySound("Over");
        SoundManager.BGMStop();
    }

    public void Retry()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    public static void success()
    {
        // 1회 성공했을 시 활성화시킴
        if(!instance.comboTimer.activeSelf)
        {
            instance.comboTimer.SetActive(true);    
        }

        hit++;
        combo++;
        score += 1 + (combo * 0.1f);
        failTime = 0;
        instance.successEffect.Play();
        instance.scoreAni.SetTrigger("Hit");
        SoundManager.PlaySound("Hit");
        instance.comboAni.SetTrigger("Combo");
    }

    IEnumerator ComboTime()
    {
        while(!isGameOver)
        {
            yield return null;
            failTime += Time.deltaTime;

            if(failTime > comboTime)
            {
                combo = 0;
            }
        }
    }

    public static void failure()
    {
        // 실패 시 플레이 타임 더해줌
        playTime += 5f;
        combo = 0;

        SoundManager.PlaySound("Fail");
    }
}
