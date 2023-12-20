using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const float ORIGIN_SPEED = 3f;

    // ���� ���ǵ�
    public static float globalSpeed;
    // ���ھ�
    public static float score;
    // ���� ��
    public static bool isLive;
    // 1%�� ���̵� ����
    float scoreRate = 0.01f;

    [Header("���� ���̵�")]
    public float gameLevel = 3f;
    [Header("���ӿ��� UI")]
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
        // ������ ���� ���� ���ǵ尡 ������ ���̵��� ������ �ְ� ��
        globalSpeed = ORIGIN_SPEED + score * scoreRate; 
    }

    public void GameOver()
    {
        isLive = false;
        gameOverUI.SetActive(true);

        // ���ӿ��� �� ���ھ� �����ͼ� ���� ���ھ�� ���ؼ� ���� ���� �ٽ� ����
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
