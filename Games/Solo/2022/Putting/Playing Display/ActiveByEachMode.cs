using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveByEachMode : MonoBehaviour
{    
    void Start()
    {
        KioskPanelManager.Instance.tranningModeDisplays[GameOption.Instance.tranningMode - 1].SetActive(true);
        //Debug.Log(GameOption.Instance.tranningMode - 1);
    }  
}
