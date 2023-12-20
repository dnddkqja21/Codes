using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShowGradient : MonoBehaviour
{
    [Header("파일 이름 텍스트")]
    public TextMeshProUGUI file;
    LoadGradient loadGradient;
    Toggle toggle;
    string originName = "";
    bool isShow = false;

    private void Awake()
    {
        loadGradient = FindObjectOfType<LoadGradient>();
    }

    private void Start()
    {
        originName = file.text;
       
        toggle = GetComponent<Toggle>();
        toggle.group = GameObject.Find("Content").GetComponent<ToggleGroup>();
    }
    public void OnShowGradient()
    {
        isShow = !isShow;
        if(isShow)
        {
            GradientRecord gradient = new GradientRecord();
            LoadRecord.Instance.nameToGradient.TryGetValue(originName + ".xml", out gradient);
            file.text = "저장된 기울기  :  ";
            for (int i = 0; i < gradient.gradients.Length; i++)
            {
                if(i < 3)
                {
                    file.text += gradient.gradients[i] + " / ";
                }
                else
                {
                    file.text += gradient.gradients[i];
                }
                loadGradient.gradient[i] = gradient.gradients[i];
            }            
        }
        else
        {
            file.text = originName;
        }
    }
}
