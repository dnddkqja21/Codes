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

    Button buttonLocalization;
    Image image;

    const string Key = "Language";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
   
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.Init)
        {
            buttonLocalization = Config.FindChild(UIManagerInit.Instance.canvas, "Button Localization").GetComponent<Button>();
        }
        else if (SceneManager.GetActiveScene().buildIndex == (int)SceneList.World)
        {
            buttonLocalization = Config.FindChild(UIManagerWorld.Instance.canvas, "Button Localization").GetComponent<Button>();
        }
        
        image = buttonLocalization.GetComponent<Image>();

        buttonLocalization.onClick.AddListener(OnChangeLanguage);

        int savedLanguageIndex = PlayerPrefs.GetInt(Key, 0);
        SetLanguage(languagePairs[savedLanguageIndex].locale);
    }

    public void OnChangeLanguage()
    {
        SoundManager.Instance.PlaySFX(SFX.Click);
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

    // 로컬 테이블
    public string LocaleTable(string key)
    {
        Locale curLang = LocalizationSettings.SelectedLocale;
        string text = LocalizationSettings.StringDatabase.GetLocalizedString("My Table", key, curLang);
        return text;
    }

    //웨이포인트
    public string WaypointTable(string key)
    {
        Locale curLang = LocalizationSettings.SelectedLocale;
        string text = string.Empty;

        if (curLang.ToString().Equals("Korean (ko)"))
        {
            text = LocalizationSettings.StringDatabase.GetLocalizedString("My Table", key, curLang) +
                   LocalizationSettings.StringDatabase.GetLocalizedString("My Table", "포탈안내", curLang);
        }
        else
        {
            text = LocalizationSettings.StringDatabase.GetLocalizedString("My Table", "포탈안내", curLang) +
                   LocalizationSettings.StringDatabase.GetLocalizedString("My Table", key, curLang) + "?";
        }
        if (key.Equals("랜덤장소"))
        {
            text += LocalizationSettings.StringDatabase.GetLocalizedString("My Table", "포탈주의", curLang);
        }
        return text;
    }

    // 미로게임 결과
    public string MazePoint()
    {
        Locale curLang = LocalizationSettings.SelectedLocale;
        string text = LocalizationSettings.StringDatabase.GetLocalizedString("My Table", "미로결과", curLang);

        string[] parts = text.Split(',');
        text = parts[0] + " : " + MazeSpawnManager.Instance.clearTime + " " + parts[1];

        return text;
    }

    // 컬러 게임 결과
    public string ColorPoint(int point)
    {
        Locale curLang = LocalizationSettings.SelectedLocale;
        string text = LocalizationSettings.StringDatabase.GetLocalizedString("My Table", "컬러결과", curLang);

        string[] parts = text.Split(',');
        text = parts[0] + " : " + point + " " + parts[1];

        return text;
    }
}
