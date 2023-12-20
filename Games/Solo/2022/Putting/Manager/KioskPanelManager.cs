using UnityEngine;
using TMPro;

public class KioskPanelManager : MonoBehaviour
{
    [Header("��� �˾� ��ư")]
    public GameObject recordPopupButton;
    [Header("���� �޴�")]
    public GameObject mainMenu;
    [Header("Ʈ���̴� ����")]
    public GameObject tranningSetting;
    [Header("�Ʒ� �� ȭ��")]
    public GameObject tranningDisplay;
    [Header("�Ʒ� ���� �ؽ�Ʈ")]
    public TextMeshProUGUI tranningTitle;
    [Header("�� ��� �� ���÷���")]
    public GameObject[] tranningModeDisplays;
    [Header("�׸� ����")]
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
        // ��� �α��� �� ��� ��ư Ȱ��ȭ
        if (!GameOption.Instance.playerList[0].isGuest)
        {            
            recordPopupButton.SetActive(true);
        }
    }
}
