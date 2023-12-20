using UnityEngine;

/// <summary>
/// Class : SelectAreaButtons
/// Desc  : 랭킹에 사용되는 기록 데이터
/// Date  : 2022-08-24
/// Autor : Kang Cheol Woong

public class SelectAreaButtons : MonoBehaviour
{        
    //  매장 지역 이름
    public UILabel[] branchName;
    public Transform[] rank;
    
    float offset = 17f;

    // 한 번만 포지션 변경을 해야 하므로 불변수 추가
    public bool isGlobal;

    public SelectDifficultly selectDifficultly;

    public void OnClickGlobal()
    {
        if(!isGlobal)
        {
            for (int i = 0; i < branchName.Length; i++)
            {
                branchName[i].enabled = true;
                

                var temp = rank[i].localPosition;
                temp.y -= offset;
                rank[i].localPosition = temp;
                isGlobal = true;
            }
            // 2022-09-06 한번만 출력하기 위한 변수의 값 수정
            selectDifficultly.isPrinted = false;
        }
    }

    public void OnClickLocal()
    {
        if(isGlobal)
        {
            for (int i = 0; i < branchName.Length; i++)
            {
                branchName[i].enabled = false;

                var temp = rank[i].localPosition;
                temp.y += offset;
                rank[i].localPosition = temp;
                isGlobal = false;
            }
            selectDifficultly.isPrinted = false;
        }
    }
}
