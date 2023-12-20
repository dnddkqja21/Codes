using UnityEngine;
using TMPro;

public class WellcomeToMyPutt : MonoBehaviour
{
    [Header("환영 텍스트")]
    public TextMeshProUGUI welcomeTitleText;

    private void Start()
    {
        if(GameOption.Instance.playerList.Count != 0)
        {
            welcomeTitleText.text = "[" + GameOption.Instance.playerList[0].nickName + "]님 MyPutt에 오신 것을 환영합니다.";            
        }
        else
        {
            welcomeTitleText.text = "정상적인 로그인 과정을 거치지 않았습니다.";
        }
    }
}
