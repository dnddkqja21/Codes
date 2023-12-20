using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    Text combo;

    void Start()
    {
        combo = GetComponent<Text>();
    }

    void LateUpdate()
    {
        combo.text = GameManager.combo.ToString() + " Combo!";
    }
}
