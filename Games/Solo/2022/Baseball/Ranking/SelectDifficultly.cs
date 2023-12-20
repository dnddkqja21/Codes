using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class : SelectDifficultly
/// Desc  : 각 난이도 버튼 별 기록 출력
/// Date  : 2022-08-24
/// Autor : Kang Cheol Woong

public class SelectDifficultly : MonoBehaviour
{
    HomerunDerbyResultManager manager;

    [Header("매장 이름")]
    public UILabel[] branchTitle;
    [Header("플레이어 이름")]
    public UILabel[] playerTitle;
    [Header("플레이어 점수")]
    public UILabel[] playerScore;
    [Header("글로벌 버튼 확인")]
    public SelectAreaButtons global;

    int rankingBoardIndex = 2;

    // 초기값으로 주니어를 주어서 루키데이터를 출력함
    int difficultlyNum = (int)GAME_LEVEL.JUNIOR;

    public bool isPrinted;

    private void Start()
    {
        // 현재 문제 : 매니저가 널임        
        // 스타트가 돌지 않았으니깐 스크립트 또는 오브젝트의 비활성화 의심해볼 것.
        manager = HomerunDerbyResultManager.Instance;
    }

    // 2022-09-06 버튼 누를 시 난이도 별 넘버를 부여
    public void OnClickRookie()
    {
        difficultlyNum = (int)GAME_LEVEL.JUNIOR;
        isPrinted = false;
    }
    public void OnClickAmateur()
    {
        difficultlyNum = (int)GAME_LEVEL.AMATEUR;
        isPrinted = false;
    }
    public void OnClickMinor()
    {
        difficultlyNum = (int)GAME_LEVEL.SEMI_PRO;
        isPrinted = false;
    }
    public void OnClickMajor()
    {
        difficultlyNum = (int)GAME_LEVEL.PRO;
        isPrinted = false;
    }

    void Update()
    {
        ShowRanking();
    }

    private void ShowRanking()
    {
        if (!isPrinted)
        {
            switch (difficultlyNum)
            {
                case -1:
                    ClearText();
                    break;
                case 0:
                    SetRookie();
                    break;
                case 1:
                    SetAmateur();
                    break;
                case 2:
                    SetMinor();
                    break;
                case 3:
                    SetMajor();
                    break;
            }
        }
    }

    public void SetRookie()
    {
        // 각 난이도 별 버튼 클릭 시 초기화 
        ClearText();

        // 텍스트 출력 제어
        SetText(manager.globalRookieData, manager.rookieData);

        // 2022-09-06 한번만 출력되도록 하기위한 처리
        isPrinted = true;
    }

    public void SetAmateur()
    {
        ClearText();
        SetText(manager.globalAmateurData, manager.amateurData);
        isPrinted = true;
    }

    public void SetMinor()
    {
        ClearText();
        SetText(manager.globalMinorData, manager.minorData);
        isPrinted = true;
    }

    public void SetMajor()
    {
        ClearText();
        SetText(manager.globalMajorData, manager.majorData);
        isPrinted = true;
    }

    // 각 난이도 별 버튼 클릭 시 텍스트 초기화 
    public void ClearText()
    {
        for (int i = 0; i < branchTitle.Length; i++)
        {
            branchTitle[i].text = "";
            playerTitle[i].text = "";
            playerScore[i].text = "";
        }
    }

    // 2022-08-31 전국, 매장 기록 누를 시 텍스트 출력
    public void SetText(List<Record> globalRecords, List<Record> records)
    {
        // 글로벌일 경우 글로벌데이터 가져옴
        if (global.isGlobal)
        {
            if (globalRecords.Count == 0)
            {
                Debug.Log("데이터 없음");
                return;
            }

            for (int i = 0; i < globalRecords.Count; i++)
            {
                if (i > rankingBoardIndex)
                {
                    // i가 랭킹보드에 표시될 수보다 높아지면 리턴
                    return;
                }

                branchTitle[i].text = globalRecords[i].shopName;
                playerTitle[i].text = globalRecords[i].playerName;
                playerScore[i].text = globalRecords[i].homerunCount;
            }
        }
        // 매장 기록 가져옴
        else
        {
            if (records.Count == 0)
            {
                Debug.Log("데이터 없음");
                return;
            }
            for (int i = 0; i < records.Count; i++)
            {
                if (i > rankingBoardIndex)
                {
                    return;
                }

                branchTitle[i].text = records[i].shopName;
                playerTitle[i].text = records[i].playerName;
                playerScore[i].text = records[i].homerunCount;
            }
        }
    }
}
