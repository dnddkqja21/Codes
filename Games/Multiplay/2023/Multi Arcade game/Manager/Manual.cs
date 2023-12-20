using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Manual : MonoBehaviour
{
    [SerializeField]
    Animator panelManual;
    [SerializeField]
    TextMeshProUGUI description;
    [SerializeField]
    Image frame;
    [SerializeField]
    Sprite[] sprites;
    [SerializeField]
    Button[] buttons;   // prev, next, exit 순
    [SerializeField]
    TextMeshProUGUI page;
    int curPage = 0;

    void Start()
    {
        UIManagerWorld.Instance.buttonManual.onClick.AddListener(() => OpenManual(true));

        buttons[(int)ManualButtonIndex.Prev].onClick.AddListener(() => UpdatePage(ref curPage, false));
        buttons[(int)ManualButtonIndex.Next].onClick.AddListener(() => UpdatePage(ref curPage, true));
        buttons[(int)ManualButtonIndex.Exit].onClick.AddListener(() => OpenManual(false));

        Invoke("FirstRun", 2f);
    }

    void FirstRun()
    {      
        if (!PlayerPrefs.HasKey("FirstRun"))
        {
            OpenManual(true);
            PlayerPrefs.SetInt("FirstRun", 1);
            PlayerPrefs.Save();
        }
    }

    void OpenManual(bool open)
    {
        curPage = 0;
        frame.sprite = sprites[curPage];
        InitPage();
        panelManual.SetBool("isShow", open);
    }    

    void InitPage()
    {
        SoundManager.Instance.PlaySFX(SFX.Panel);

        buttons[(int)ManualButtonIndex.Prev].gameObject.SetActive(curPage != 0);
        buttons[(int)ManualButtonIndex.Next].gameObject.SetActive(curPage != sprites.Length -1);

        string desc = LocalizationManager.Instance.LocaleTable(curPage.ToString());
        description.text = desc;

        page.text = (curPage + 1).ToString() + " / " + (sprites.Length).ToString();
    }

    void UpdatePage(ref int page, bool increment)
    {
        if(increment)
        {
            page++;
        }
        else
        {
            page--;
        }

        frame.sprite = sprites[page];
        InitPage();
    }
}
