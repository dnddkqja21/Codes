using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class ChangeScenePopup : MonoBehaviour
{    
    TextMeshProUGUI interiorNameText; 

    void Awake()
    {
        // 첫 번째 자식 가져옴
        interiorNameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        Locale curLang = LocalizationSettings.SelectedLocale;
        string enter = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "입장", curLang);
        string key = EnumToData.Instance.BuildingNameToKey(GameManager.Instance.buildingName);
        string building = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", key, curLang);
        interiorNameText.text = building + enter;
    }
}
