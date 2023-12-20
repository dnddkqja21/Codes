using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class : LastPlayer
/// Desc  : 최근 플레이어 출력 
/// Date  : 2022-08-23
/// Autor : Kang Cheol Woong

public class LastPlayer : MonoBehaviour
{
    public UILabel lastPlayerName;
    public UILabel lastPlayerPoint;
	
	void Start ()
    {
        PrintLastPlayer();

        // find (조건에 해당하는 것 리턴) ex)스코어가 999인 것을 찾아라
        //Record str = HomerunDerbyResultManager.instance.recordList.Find(x=>x.bestScore =="999");
    }

    public void PrintLastPlayer()
    {
        // 이전 작동 방식(최근 기록이 따로 없을 시)
        #region
        // 매니저에서 마지막에 최근 플레이어를 리스트에 추가하였으므로 소팅없이 가장 마지막 인덱스의 원소가 최근 플레이어가 된다.
        // 카운트는 원소의 갯수이고 인덱스는 0부터 시작하므로 -1 해준다.
        //Record last = HomerunDerbyResultManager.instance.lastRecord[HomerunDerbyResultManager.instance.lastRecord.Count - 1];

        // 최근 기록 또한 정렬 후 출력한다. 오름 차순으로 소팅 후 마지막 인덱스 검색
        //Record last = HomerunDerbyResultManager.instance.lastRecord[HomerunDerbyResultManager.instance.lastRecord.Count - 1];
        #endregion
        if(HomerunDerbyResultManager.Instance.LastPlayerData == null)
        {
            Debug.Log("최근 기록이 없습니다.");
            return;
        }
        LastPlayerRecord last = HomerunDerbyResultManager.Instance.LastPlayerData;
        lastPlayerName.text = last.playerName;
        lastPlayerPoint.text = last.homerunCount;
    }    
}
