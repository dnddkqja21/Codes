using UnityEngine;
using TMPro;
using System.IO;

public class SaveRecord : MonoBehaviour
{
    private static SaveRecord instance = null;
    public static SaveRecord Instance { get { return instance; } }

    [Header("��ǲ�ʵ�")]
    public TMP_InputField inputField;
    [Header("��� �޼���")]
    public TextMeshProUGUI caution;    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public void Save()
    {
        string path = Application.dataPath + "/StreamingAssets/SaveGradient/";
        string fileName = inputField.text + ".xml";
        
        string isinSpecialChar = inputField.text;

        // ���ϸ� �������� �˻�
        if(isinSpecialChar == "")
        {
            caution.gameObject.SetActive(true);
            caution.enabled = true;
            caution.text = "���� �̸��� �Է��ϼ���.";
            return;
        }

        // ���ϸ� Ư������ �ִ��� ���� �˻�
        char[] specialCharacters = { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

        for (int i = 0; i < specialCharacters.Length; i++)
        {
            foreach(char item in isinSpecialChar)
            {
                if(item.Equals(specialCharacters[i]))
                {
                    caution.gameObject.SetActive(true);
                    caution.enabled = true;
                    caution.text = "���� �̸����� Ư�����ڸ� ����� �� �����ϴ�.";
                    return;
                }
            }
        }       

        // ������ �̸��� ���� �ִ��� ���� �˻�
        string sameNameFile = path + fileName;
        FileInfo isFileExist = new FileInfo(sameNameFile);

        if(isFileExist.Exists)
        {
            caution.gameObject.SetActive(true);
            caution.enabled = true;
            caution.text = "������ �̸��� ���ϸ��� �ֽ��ϴ�.";
            return;
        }

        // ��ǲ �ʵ� �ʱ�ȭ
        inputField.text = "";
        // ��� �޽��� ��Ȱ��
        caution.enabled = false;

        // ������ ���� ������ ����
        GradientRecord gradientRecord = new GradientRecord();
        
        gradientRecord.gradients = GameOption.Instance.gradients;

        XmlSerializerManager<GradientRecord>.Save(path + fileName, gradientRecord);

        PopupManager.Instance.saveGradientPopup.SetActive(false);
        PopupManager.Instance.saveGradientMessage.SetActive(true);
    }    
}
