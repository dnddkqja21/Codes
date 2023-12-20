using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{    
    public GameObject panelTutorial;
    [SerializeField]
    TextMeshProUGUI description;
    [SerializeField]
    Image mainImage;
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    Button buttonPrev;
    [SerializeField]
    Button buttonNext;
    [SerializeField]
    Button buttonClose;
    [SerializeField]
    TextMeshProUGUI page;
    int curPage = 0;   

    void Start()
    {
        UIManager.Instance.buttonTutorial.onClick.AddListener(OpenPage);
        buttonNext.onClick.AddListener(NextPage);
        buttonPrev.onClick.AddListener(PrevPage);
        buttonClose.onClick.AddListener(InitPage);

        Invoke("OpenTutorial", 2f);
    }

    void OpenTutorial()
    {
        if (!PlayerPrefs.HasKey("FirstTutorial"))
        {
            OpenPage();

            PlayerPrefs.SetInt("FirstTutorial", 1);
            PlayerPrefs.Save();
        }
    }

    void InitTutorial()
    {
        buttonPrev.gameObject.SetActive(false);
        if(curPage != 0)
        {
            buttonPrev.gameObject.SetActive(true);
        }

        buttonNext.gameObject.SetActive(true);
        if(curPage == sprites.Length - 1)
        {
            buttonNext.gameObject.SetActive(false);
        }

        Locale curLang = LocalizationSettings.SelectedLocale;
        string desc = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", curPage.ToString(), curLang);
        description.text = desc;

        page.text = (curPage + 1).ToString() + "/" + (sprites.Length).ToString();
    }

    void NextPage()
    {
        curPage++;
        mainImage.sprite = sprites[curPage];
        InitTutorial();
    }

    void PrevPage()
    {
        curPage--;
        mainImage.sprite = sprites[curPage];
        InitTutorial();
    }

    void InitPage()
    {
        curPage = 0;
        mainImage.sprite = sprites[curPage];
        InitTutorial();

        panelTutorial.SetActive(false);
    }

    void OpenPage()
    {
        curPage = 0;
        mainImage.sprite = sprites[curPage];
        InitTutorial();

        panelTutorial.SetActive(true);
    }
}
