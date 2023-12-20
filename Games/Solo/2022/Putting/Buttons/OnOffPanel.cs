using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffPanel : MonoBehaviour
{
    [Header("�г�")]
    public GameObject panel;

    bool isOnPanel = false;

    private void Start()
    {
        if (gameObject.name == "Green Setting Button")
        {
            // �׸� ������ ���� �Ʒø�� �Ǵ� ���� �Ʒ��� ���� ���� ��忡���� ������.
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
