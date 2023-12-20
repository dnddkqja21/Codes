using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class OpenWebPopup : MonoBehaviour
{
    TextMeshProUGUI desc;

    void Awake()
    {
        desc = GetComponentInChildren<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        Locale curLang = LocalizationSettings.SelectedLocale;
        string enter = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "웹", curLang);
        string key = EnumToData.Instance.BuildingNameToKey(GameManager.Instance.buildingName);
        string building = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", key, curLang);
        
        desc.text = building + enter;
    }
}
