using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeLanguage : MonoBehaviour
{
    [System.Serializable]
    public struct LanguageSpritePair
    {
        public Sprite sprite;
        public Locale locale;
    }

    [SerializeField]
    LanguageSpritePair[] languagePairs;

    Image image;

    const string Key = "Language";

    void Start()
    {
        image = GetComponent<Image>();

        int savedLanguageIndex = PlayerPrefs.GetInt(Key, 0);
        SetLanguage(languagePairs[savedLanguageIndex].locale);
    }

    public void OnChangeLanguage()
    {
        ToggleLanguage();
    }

    void ToggleLanguage()
    {
        Locale selectedLocale = LocalizationSettings.SelectedLocale;

        for (int i = 0; i < languagePairs.Length; i++)
        {
            if (languagePairs[i].locale == selectedLocale)
            {
                int nextIndex = (i + 1) % languagePairs.Length;
                SetLanguage(languagePairs[nextIndex].locale);
                PlayerPrefs.SetInt(Key, nextIndex);
                return;
            }
        }
    }

    void SetLanguage(Locale locale)
    {
        LocalizationSettings.SelectedLocale = locale;

        for (int i = 0; i < languagePairs.Length; i++)
        {
            if (languagePairs[i].locale == locale)
            {
                image.sprite = languagePairs[i].sprite;
                return;
            }
        }
    }
}
