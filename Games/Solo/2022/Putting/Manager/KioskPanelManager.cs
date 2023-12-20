using UnityEngine;
using TMPro;

public class KioskPanelManager : MonoBehaviour
{
    [Header("기록 팝업 버튼")]
    public GameObject recordPopupButton;
    [Header("메인 메뉴")]
    public GameObject mainMenu;
    [Header("트레이닝 세팅")]
    public GameObject tranningSetting;
    [Header("훈련 중 화면")]
    public GameObject tranningDisplay;
    [Header("훈련 설정 텍스트")]
    public TextMeshProUGUI tranningTitle;
    [Header("각 모드 별 디스플레이")]
    public GameObject[] tranningModeDisplays;
    [Header("그린 세팅")]
    public GameObject greenSetting;

    private static KioskPanelManager instance = null;
    public static KioskPanelManager Instance { get { return instance; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        mainMenu.SetActive(true);
        if (GameOption.Instance.playerList.Count == 0)
        {
            return;
        }
        // 멤버 로그인 시 기록 버튼 활성화
        if (!GameOption.Instance.playerList[0].isGuest)
        {            
            recordPopupButton.SetActive(true);
        }
    }
}
