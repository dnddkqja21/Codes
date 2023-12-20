using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [Header("기록 팝업")]
    public GameObject recordPopup;    
    [Header("종료 팝업")]
    public GameObject exitPopup;        
    [Header("기울기 저장 팝업")]
    public GameObject saveGradientPopup;
    [Header("기울기 저장 메시지")]
    public GameObject saveGradientMessage;
    [Header("기울기 로드 팝업")]
    public GameObject loadGradientPopup;
    [Header("그린 변경 경고 팝업")]
    public GameObject changeGreenPopup;
    [Header("기울기 초기화 팝업")]
    public GameObject resetGradientPopup;
    [Header("일시 정지 팝업")]
    public GameObject pausePopup;
    [Header("훈련 결과 팝업")]
    public GameObject tranningResultPopup;
    [Header("볼 정리 팝업")]
    public GameObject cleanBallPopup;
    [Header("대기 화면 팝업")]
    public GameObject standbyPopup;
    [Header("페이드 아웃")]
    public GameObject fadeOut;

    private static PopupManager instance = null;
    public static PopupManager Instance { get { return instance; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (GameOption.Instance.playerList.Count == 0)
        {
            return;
        }
        // 멤버 로그인 시 기록 팝업 활성화
        if (!GameOption.Instance.playerList[0].isGuest)
        {            
            recordPopup.SetActive(true);            
        }

        fadeOut.SetActive(true);
    }
}
