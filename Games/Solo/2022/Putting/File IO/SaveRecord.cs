using UnityEngine;
using TMPro;
using System.IO;

public class SaveRecord : MonoBehaviour
{
    private static SaveRecord instance = null;
    public static SaveRecord Instance { get { return instance; } }

    [Header("인풋필드")]
    public TMP_InputField inputField;
    [Header("경고 메세지")]
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

        // 파일명 공백인지 검사
        if(isinSpecialChar == "")
        {
            caution.gameObject.SetActive(true);
            caution.enabled = true;
            caution.text = "파일 이름을 입력하세요.";
            return;
        }

        // 파일명에 특수문자 있는지 여부 검사
        char[] specialCharacters = { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

        for (int i = 0; i < specialCharacters.Length; i++)
        {
            foreach(char item in isinSpecialChar)
            {
                if(item.Equals(specialCharacters[i]))
                {
                    caution.gameObject.SetActive(true);
                    caution.enabled = true;
                    caution.text = "파일 이름에는 특수문자를 사용할 수 없습니다.";
                    return;
                }
            }
        }       

        // 동일한 이름의 파일 있는지 여부 검사
        string sameNameFile = path + fileName;
        FileInfo isFileExist = new FileInfo(sameNameFile);

        if(isFileExist.Exists)
        {
            caution.gameObject.SetActive(true);
            caution.enabled = true;
            caution.text = "동일한 이름의 파일명이 있습니다.";
            return;
        }

        // 인풋 필드 초기화
        inputField.text = "";
        // 경고 메시지 비활성
        caution.enabled = false;

        // 동일한 파일 없으면 저장
        GradientRecord gradientRecord = new GradientRecord();
        
        gradientRecord.gradients = GameOption.Instance.gradients;

        XmlSerializerManager<GradientRecord>.Save(path + fileName, gradientRecord);

        PopupManager.Instance.saveGradientPopup.SetActive(false);
        PopupManager.Instance.saveGradientMessage.SetActive(true);
    }    
}
