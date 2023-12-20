using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    static LocalizationManager instance = null;
    public static LocalizationManager Instance { get { return instance; } }

    [System.Serializable]
    public struct LanguageSpritePair
    {
        public Sprite sprite;
        public Locale locale;
    }

    [SerializeField]
    LanguageSpritePair[] languagePairs;

    Button localizationButton;
    Image image;

    const string Key = "Language";

    void Awake()
    {
        if (instance == null)
            instance = this;        
    }

    void Start()
    {
        localizationButton = GameObject.Find("Localization Button").GetComponent<Button>();
        image = localizationButton.GetComponent<Image>();

        localizationButton.onClick.AddListener(OnChangeLanguage);

        int savedLanguageIndex = PlayerPrefs.GetInt(Key, 0);
        SetLanguage(languagePairs[savedLanguageIndex].locale);

        //CurrentLocale();
    }

    public void OnChangeLanguage()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneName.World)
            SoundManager.Instance.PlaySFX(SFX.MenuClick);

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
                WebviewManager.Instance.ChangeLangMessage();
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
                WebviewManager.Instance.ChangeLangMessage();
                return;
            }
        }        
    }

    // 웹뷰와 통신할 때 현재 언어 상태 가져오기.
    public string CurrentLocale()
    {
        // locale 한국 : Korean (ko), 중국 : Chinese (Simplified) (zh-Hans)
        Locale locale = LocalizationSettings.SelectedLocale;
        string lang = locale.ToString();
        if(lang.Equals("Korean (ko)"))
        {
            lang = "KO_KR";
        }
        else
        {
            lang = "ZH_CN";
        }
        
        return lang;
    }
}
