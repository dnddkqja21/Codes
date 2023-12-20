using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class LoadRecord : MonoBehaviour
{
    private static LoadRecord instance = null;
    public static LoadRecord Instance { get { return instance; } }

    [Header("���� ������")]
    public GameObject gradientPrefab;
    [Header("������ ������ �θ� Ʈ������")]
    public Transform contents;

    // meta���� ������ ������ �̸� ����Ʈ
    List<string> fileNameList = new List<string>();

    // ���� ����Ʈ
    public List<GradientRecord> recordList = new List<GradientRecord>();

    // �̸�, ����
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
        // �ε� �� ����Ʈ Ŭ����
        fileNameList.Clear();
        recordList.Clear();
        nameToGradient.Clear();

        GameOption.Instance.isLoaded = true;

        string path = Application.dataPath + "/StreamingAssets/SaveGradient/";

        // ��� ���� ����
        DirectoryInfo di = new DirectoryInfo(path);

        if (!di.Exists)
        {
            Debug.Log("����� ������ �����ϴ�.");
            return;
        }
        FileInfo[] files = di.GetFiles();
        
        // ���� �̸� ����
        foreach (var item in files)
        {
            if (!item.Name.Contains("meta"))
            {                
                fileNameList.Add(item.Name);
            }
        }        

        // 0929 �ǹ� ���� - �̸��� �����͸� ��ġ���Ѿ� �ϹǷ� �ٸ� �����̳� ��� �ϳ�? 
        // ���� ����
        for (int i = 0; i < fileNameList.Count; i++)
        {
            recordList.Add(XmlSerializerManager<GradientRecord>.Load(path + fileNameList[i]));
        }

        for (int i = 0; i < recordList.Count; i++)
        {
            nameToGradient.Add(fileNameList[i], recordList[i]);
        }

        // ������ ������Ʈ ����Ʈ
        List<GameObject> instantObj = new List<GameObject>();
        for (int i = 0; i < recordList.Count; i++)
        {
            instantObj.Add(Instantiate(gradientPrefab, contents));
            instantObj[i].GetComponentInChildren<TextMeshProUGUI>().text = fileNameList[i].Replace(".xml","");           
        }
    }
}
