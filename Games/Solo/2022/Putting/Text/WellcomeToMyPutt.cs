using UnityEngine;
using TMPro;

public class WellcomeToMyPutt : MonoBehaviour
{
    [Header("ȯ�� �ؽ�Ʈ")]
    public TextMeshProUGUI welcomeTitleText;

    private void Start()
    {
        if(GameOption.Instance.playerList.Count != 0)
        {
            welcomeTitleText.text = "[" + GameOption.Instance.playerList[0].nickName + "]�� MyPutt�� ���� ���� ȯ���մϴ�.";            
        }
        else
        {
            welcomeTitleText.text = "�������� �α��� ������ ��ġ�� �ʾҽ��ϴ�.";
        }
    }
}
