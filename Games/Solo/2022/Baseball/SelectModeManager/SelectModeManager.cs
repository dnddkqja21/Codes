using UnityEngine;

/// <summary>
/// Class : SelectModeManager
/// Desc  : xml파일을 읽어서 실행될 게임 모드를 관리함.
/// Date  : 2022-08-19
/// Autor : Kang Cheol Woong

public class SelectModeManager : MonoBehaviour
{
    [Header("BoxCollider")]
    public BoxCollider[] boxes;

    RuleFile ruleFile = new RuleFile();

    string fileName = "SelectMode.xml";

    [Header("모드 선택 : Space")]
    public KeyCode key = KeyCode.Space;


    public SelectModeControl selectModeControl;

    private static SelectModeManager instance = null;

    public static SelectModeManager Instance { get { return instance; } }

    public bool isXmlOn = false;

    private void Start()
    {
        string path = Application.dataPath + "/StreamingAssets/";
        XmlSerializerManager<RuleFile>.Save(path + fileName, ruleFile);

        if (instance == null)
        {
            instance = this;
        }

        // 2022-08-29 [08.30 시연]버전에서만 스타트 시 읽기
        //LoadMode();
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            LoadMode();
        }
    }

    void LoadMode()
    {
        string path = Application.dataPath + "/StreamingAssets/";
        RuleFile temp = XmlSerializerManager<RuleFile>.Load(path + fileName);

        // 2022-08-19 커밍순과 랭킹의 두가지 기능 사이에서 발생하는 현상에 대한 해결책
        if (temp != null)
        {
            isXmlOn = true;
        }

        temp.SetMode();
        // HasFlag 이전 버전 이슈 --> HasEnum
        boxes[(int)SelectModeIndex.ONEOUT].enabled          = temp.mode.HasEnum(SelectModeControl.ONEOUT);
        boxes[(int)SelectModeIndex.FULLCOUNT].enabled       = temp.mode.HasEnum(SelectModeControl.FULLCOUNT);
        boxes[(int)SelectModeIndex.AI].enabled              = temp.mode.HasEnum(SelectModeControl.AI);
        boxes[(int)SelectModeIndex.HOMERUNDERBY].enabled    = temp.mode.HasEnum(SelectModeControl.HOMERUNDERBY);
    }
}
