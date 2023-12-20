using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffPanel : MonoBehaviour
{
    [Header("패널")]
    public GameObject panel;

    bool isOnPanel = false;

    private void Start()
    {
        if (gameObject.name == "Green Setting Button")
        {
            // 그린 변경은 자유 훈련모드 또는 실전 훈련의 기울기 없음 모드에서만 가능함.
            if (GameOption.Instance.selectedMode == (int)GameMode.FREE)
            {
                GetComponent<Button>().interactable = true;
            }
            else if (GameOption.Instance.tranningMode == (int)TranningMode.ACTUAL)
            {
                if (GameOption.Instance.actualGradient == (int)ActualGradient.NONE)
                {
                    GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    public void SwitchPanel()
    {
        isOnPanel = !isOnPanel;
                
        panel.SetActive(isOnPanel);        
    }
}
