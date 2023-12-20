using UnityEngine;

/// <summary>
/// Class : RankingBoard
/// Desc  : 랭킹 보드 출력 
/// Date  : 2022-08-19
/// Autor : Kang Cheol Woong

public class RankingBoard : MonoBehaviour {

    // 2022-08-24 패널 뒤의 오브젝트 상호작용 방식 변경 (boxCollider)의 활성화를 제어하는 방식 -> ui패널의 depth활용 방식으로 수정 

    public bool isShow = false;         

    public void ShowRankingBoard()
    {
        isShow = !isShow;
        gameObject.SetActive(isShow);
    }
}
