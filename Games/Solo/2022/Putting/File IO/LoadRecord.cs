using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class LoadRecord : MonoBehaviour
{
    private static LoadRecord instance = null;
    public static LoadRecord Instance { get { return instance; } }

    [Header("기울기 프리펩")]
    public GameObject gradientPrefab;
    [Header("프리펩 생성될 부모 트랜스폼")]
    public Transform contents;

    // meta파일 제외한 파일의 이름 리스트
    List<string> fileNameList = new List<string>();

    // 기울기 리스트
    public List<GradientRecord> recordList = new List<GradientRecord>();

    // 이름, 기울기
    public Dictionary<string, GradientRecord> nameToGradient = new Dictionary<string, GradientRecord>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Load()
    {
        // 로드 시 리스트 클리어
        fileNameList.Clear();
        recordList.Clear();
        nameToGradient.Clear();

        GameOption.Instance.isLoaded = true;

        string path = Application.dataPath + "/StreamingAssets/SaveGradient/";

        // 경로 안의 파일
        DirectoryInfo di = new DirectoryInfo(path);

        if (!di.Exists)
        {
            Debug.Log("저장된 파일이 없습니다.");
            return;
        }
        FileInfo[] files = di.GetFiles();
        
        // 파일 이름 저장
        foreach (var item in files)
        {
            if (!item.Name.Contains("meta"))
            {                
                fileNameList.Add(item.Name);
            }
        }        

        // 0929 의문 사항 - 이름과 데이터를 일치시켜야 하므로 다른 컨테이너 써야 하나? 
        // 파일 저장
        for (int i = 0; i < fileNameList.Count; i++)
        {
            recordList.Add(XmlSerializerManager<GradientRecord>.Load(path + fileNameList[i]));
        }

        for (int i = 0; i < recordList.Count; i++)
        {
            nameToGradient.Add(fileNameList[i], recordList[i]);
        }

        // 생성한 오브젝트 리스트
        List<GameObject> instantObj = new List<GameObject>();
        for (int i = 0; i < recordList.Count; i++)
        {
            instantObj.Add(Instantiate(gradientPrefab, contents));
            instantObj[i].GetComponentInChildren<TextMeshProUGUI>().text = fileNameList[i].Replace(".xml","");           
        }
    }
}
