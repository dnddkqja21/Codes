using UnityEngine;

/// <summary>
/// Class : CommingSoon
/// Desc  : 시연 버전에서 다루지않을 모드에 대한 락 처리
/// Date  : 2022-08-18
/// Autor : Kang Cheol Woong

public class CommingSoon : MonoBehaviour
{
    public BoxCollider[] boxes;

    // 2022-08-29 각 모드 별 따로 커밍순 띄움
    public UISprite[] commingSoon;  

    void Update()
    {   
        // 2022-08-19 랭킹 버튼 누를 시 ai와 hd가 비활성화되기 때문에 "준비 중입니다."가 출력됨. 비활성화 말고 다른 조건 하나를 더 추가해야 함.
        // xml에서 파일 읽었을 때만 준비 중 띄움.
        if (SelectModeManager.Instance.isXmlOn)
        {
            ShowCommingSoon();
        }
    }    

    private void ShowCommingSoon()
    {    
        for (int i = 0; i < boxes.Length; i++)
        {
            if(!boxes[i].enabled)
            {
                commingSoon[i].enabled = true;
            }
        }
    }
}
