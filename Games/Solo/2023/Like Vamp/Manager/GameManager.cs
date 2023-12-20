using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    [Header("�÷��̾�")]
    public PlayerMove player;
    [Header("Ǯ�� �Ŵ���")]
    public PoolingManager poolingManager;
    [Header("������")]
    public LevelUp levelUp;
    [Header("���� ���� �ð�")]
    public float gameTime;
    [Header("�ִ� �ð�")]
    public float maxGameTime = 5 * 10f;

    [Header("�÷��̾� ����")]
    public int playerID;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 7, 9, 150, 210, 280, 360, 450, 600 };


    public bool isLive;

    public GameResult gameOverUI;
    public GameObject enemyCleaner;
    public Transform joystickUI;


    void Awake()
    {
        if (instance == null)
            instance = this;

        Application.targetFrameRate = 60;
    }

    public void GameStart(int id)
    {
        playerID = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        levelUp.Select(playerID % 2);
        Resume();

        AudioManager.instance.PlaySFX(AudioManager.SFX.Select);
        AudioManager.instance.PlayBGM(true);
    }

    public void GameReStart()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        gameOverUI.gameObject.SetActive(true);
        gameOverUI.Lose();
        Stop();

        AudioManager.instance.PlaySFX(AudioManager.SFX.Lose);
        AudioManager.instance.PlayBGM(false);
    }

    public void GameWin()
    {
        StartCoroutine(GameWinRoutine());
    }

    IEnumerator GameWinRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        gameOverUI.gameObject.SetActive(true);
        gameOverUI.Win();
        Stop();

        AudioManager.instance.PlaySFX(AudioManager.SFX.Win);
        AudioManager.instance.PlayBGM(false);
    }

    void Update()
    {
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        if(gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameWin();
        }
    }

    public void GetExp()
    {
        // ���� ���� ����ġ �ο��ϴ� ���� ����
        if (!isLive)
            return;

        exp++;

        // max���� �̻���ʹ� �ְ� ����ġ �䱸���� �״�� ��� ���
        if(exp == nextExp[Mathf.Min(level, nextExp.Length -1)])
        {
            level++;
            exp = 0;
            levelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        // 0���� �ϸ� ����
        Time.timeScale = 0;

        joystickUI.localScale = Vector3.zero;
    }

    public void Resume() 
    {
        isLive = true;
        // 2~3��� ��� ���� ����
        Time.timeScale = 1;

        joystickUI.localScale = Vector3.one;
    }
}
