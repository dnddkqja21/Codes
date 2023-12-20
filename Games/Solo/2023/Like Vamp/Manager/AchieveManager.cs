using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacters;
    public GameObject[] unlockCharacters;
    public GameObject notice;

    // 2���� ����
    
    enum Achieve { UnlockPotato, UnlockBean }
    Achieve[] achieves;

    WaitForSecondsRealtime wait;

    void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5);

        // ó�� ���� �� �ʱ�ȭ, ���̵����͸� �־��� ������ �ι� ° ������ʹ� ������ 0���� ���� �ʵ��� ��.
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Start()
    {
        UnlockCharacter();
    }

    void LateUpdate()
    {
        foreach(Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);
        foreach(Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }        
    }

    void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacters.Length; i++)
        {
            string achieveName = achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;
            lockCharacters[i].SetActive(!isUnlock);
            unlockCharacters[i].SetActive(isUnlock);
        }
    }

    void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;

        switch(achieve)
        {
            case Achieve.UnlockPotato:
                isAchieve = GameManager.Instance.kill >= 10;
                break;
            case Achieve.UnlockBean:
                isAchieve = GameManager.Instance.gameTime == GameManager.Instance.maxGameTime;
                break;
        }

        if(isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0) 
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);

            for (int i = 0; i < notice.transform.childCount; i++)
            {
                bool isActive = i == (int)achieve;
                notice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        notice.SetActive(true);
        AudioManager.instance.PlaySFX(AudioManager.SFX.LevelUp);

        yield return wait;

        notice.SetActive(false);
    }
}
