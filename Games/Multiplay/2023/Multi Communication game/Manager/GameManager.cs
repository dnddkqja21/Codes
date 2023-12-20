using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

/// <summary>
/// 게임 매니저
/// </summary>

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    //public GameObject player;

    [Header("조이스틱")]
    [SerializeField]
    GameObject[] joysticks;
    [Header("네임 오브젝트 프리팹")]
    public GameObject nameObjPrefab;
    [Header("목표 지점")]
    public GameObject destinationPrefab;
    public float destinationPosY;
    public string buildingName;

    // 더미 프로필 이미지
    public Texture2D dummyProfile;

    [Header("밤낮 효과")]
    [SerializeField]
    Material skyBoxDay;
    [SerializeField]
    Material skyBoxNight;
    [SerializeField]
    GameObject dLight;
    [SerializeField]
    Texture2D lightCookie;
    // test
    bool isChange = true;

    [Header("날씨 효과")]
    [SerializeField]
    GameObject rain;
    [SerializeField]
    GameObject snow;

    [Header("계절 효과 (0: 봄여름, 1: 가을, 2: 겨울)")]
    [SerializeField]
    GameObject[] environment;
    [SerializeField]
    GameObject[] mainGrounds;
    [SerializeField]
    GameObject mountain;
    [SerializeField]
    Material[] seasonMainGroundMaterials;
    [SerializeField]
    Material[] seasonMountainMaterials;

    // fps 감지 후 강제 퀄리티 조정
    float checkDuration = 5f;
    float startTime;
    int frameCount;    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {  
        if (UserData.Instance.avatarData.userAuthor == "G")
        {
            UIManager.Instance.courseButton.gameObject.SetActive(true);
            UIManager.Instance.courseButton.onClick.AddListener(() =>
            WebviewManager.Instance.LoadUrl(true,URL_CONFIG.MAIN_FRONT + URL_CONFIG.COURSE)
            );
        }
#if UNITY_EDITOR_WIN || UNITY_STANDALONE || UNITY_EDITOR_OSX  // 윈도우, 에디터에서 조이스틱 비활성
        foreach (var joystick in joysticks)
        {
            joystick.SetActive(false);
        }
#endif

        Debug.Log("User Nickname: " + UserData.Instance.avatarData.userNcnm);
        Debug.Log("Profile Image Content: " + UserData.Instance.avatarData.proflImageCn);
        Debug.Log("Profile Color: " + UserData.Instance.avatarData.proflColor);

        // 날씨 및 계절 효과
        SetSeasonMaterial();
        StartCoroutine(CheckTimeCoroutine());
        StartCoroutine(SetQuality());
    }    

    IEnumerator CheckTimeCoroutine()
    {
        while (true)
        {
            // 현재 시간과 밤낮효과
            DateTime currentTime = DateTime.Now;

            // 시간 체크 아침 7시, 저녁7시 기준
            bool isDay = currentTime.Hour >= 7 && currentTime.Hour < 19;            

            // 날씨 api호출
            Dictionary<string, object> requestData = new Dictionary<string, object>();
            requestData.Add("areaLa", 36.770536);
            requestData.Add("areaLo", 126.932576);

            StartCoroutine(HttpNetwork.Instance.PostSendNetwork(requestData, URL_CONFIG.WEATHER, (data) =>
            {
                if (data != null)
                {
                    Dictionary<string, object> obj = (Dictionary<string, object>)data;
                    string weather = (string)obj["weatherCode"];
                    RainyAndSnowy(weather);
                    Debug.Log("현재시간 : " + currentTime.Hour + ", 낮입니까? " + isDay + ", 날씨 : " + weather);
                }
            }));

            DayAndNight(isDay);

            yield return new WaitForSeconds(3600f); // 3600 sec = 1 hour
        }
    }

    void RainyAndSnowy(string weather)
    {
        //weatherCode - {'Rainy', 'Snowy', 'Sunny', 'Cloudy'}  
        // 모두 비활성화 후 날씨에 따른 오브젝트 활성화
        rain.SetActive(false);
        snow.SetActive(false);

        rain.SetActive(weather == "Rainy");
        snow.SetActive(weather == "Snowy");
    }

    void DayAndNight(bool isDay)
    {
        dLight.GetComponent<Light>().cookie = isDay ? null : lightCookie;
        
        RenderSettings.skybox = isDay ? skyBoxDay : skyBoxNight;
    }

    // test
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        isChange = !isChange;
    //        DayAndNight(isChange);
    //    }
    //    if(Input.GetKeyDown(KeyCode.Alpha1))
    //    {
    //        RainyAndSnowy("Rainy");
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha2))
    //    {
    //        RainyAndSnowy("Snowy");
    //    }
    //    if (Input.GetKeyDown(KeyCode.Alpha3))
    //    {
    //        RainyAndSnowy("...");
    //    }
    //}

    void SetSeasonMaterial()
    {
        int currentMonth = DateTime.Now.Month;
        int index = -1;
        //currentMonth = 6;
        if (currentMonth >= 3 && currentMonth <= 8)
        {
            index = 0;
            ApplyMaterial(index);
        }
        else if (currentMonth >= 9 && currentMonth <= 11)
        {
            index = 1;
            ApplyMaterial(index);
        }
        else 
        {
            index = 2;
            ApplyMaterial(index);
        }
    }

    void ApplyMaterial(int index)
    {
        environment[index].SetActive(true);
        mountain.GetComponent<Renderer>().material = seasonMountainMaterials[index];
        for (int i = 0; i < mainGrounds.Length; i++)
        {
            mainGrounds[i].GetComponent<Renderer>().material = seasonMainGroundMaterials[index];
        }
    }

    IEnumerator SetQuality()
    {
        yield return new WaitForSeconds(1f);

        startTime = Time.time;
        frameCount = 0;
        while (Time.time - startTime < checkDuration)
        {
            frameCount++;
            yield return null;
        }
        
        if(Time.time - startTime > checkDuration)
        {
            float averageFPS = frameCount / checkDuration;
            if( averageFPS < 25f )
            {
                QualitySettings.SetQualityLevel(0, true);
                Locale curLang = LocalizationSettings.SelectedLocale;
                string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "최적화", curLang);
                PopupManager.Instance.ShowOneButtnPopup(text);
            }
        }
    }
}
