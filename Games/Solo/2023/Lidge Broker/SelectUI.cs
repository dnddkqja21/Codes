using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    public KeyCode mappingKey;
    Button button;
    void Start()
    {
        button = GetComponent<Button>();    
    }

    void Update()
    {
        if(GameManager.isGameOver) { return; }

        if(Input.GetKeyDown(mappingKey))
        {
            // 버튼의 onClick에 등록된 함수가 호출된다.
            button.onClick.Invoke();
        }
    }
}
