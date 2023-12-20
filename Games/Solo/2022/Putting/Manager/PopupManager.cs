using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [Header("��� �˾�")]
    public GameObject recordPopup;    
    [Header("���� �˾�")]
    public GameObject exitPopup;        
    [Header("���� ���� �˾�")]
    public GameObject saveGradientPopup;
    [Header("���� ���� �޽���")]
    public GameObject saveGradientMessage;
    [Header("���� �ε� �˾�")]
    public GameObject loadGradientPopup;
    [Header("�׸� ���� ��� �˾�")]
    public GameObject changeGreenPopup;
    [Header("���� �ʱ�ȭ �˾�")]
    public GameObject resetGradientPopup;
    [Header("�Ͻ� ���� �˾�")]
    public GameObject pausePopup;
    [Header("�Ʒ� ��� �˾�")]
    public GameObject tranningResultPopup;
    [Header("�� ���� �˾�")]
    public GameObject cleanBallPopup;
    [Header("��� ȭ�� �˾�")]
    public GameObject standbyPopup;
    [Header("���̵� �ƿ�")]
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
        // ��� �α��� �� ��� �˾� Ȱ��ȭ
        if (!GameOption.Instance.playerList[0].isGuest)
        {            
            recordPopup.SetActive(true);            
        }

        fadeOut.SetActive(true);
    }
}
