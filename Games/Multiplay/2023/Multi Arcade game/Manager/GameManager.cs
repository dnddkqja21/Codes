using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    public static GameManager Instance { get { return instance; } }

    [Header("시간")]
    [SerializeField]
    TextMeshProUGUI timer;

    [Header("배터리")]
    [SerializeField]
    GameObject battery;
    [SerializeField]
    GameObject batteryEmpty;
    [SerializeField]
    GameObject batteryCharging;
    [SerializeField]
    Slider batterySlider;

    [Header("밤낮 효과")]
    [SerializeField]
    Material skyBoxDay;
    [SerializeField]
    Material skyBoxMiddle;
    [SerializeField]
    Material skyBoxNight;
    Material skyBoxTemp;
    [SerializeField]
    GameObject dLight;

    [Header("날씨 효과")]
    [SerializeField]
    GameObject rain;
    [SerializeField]
    GameObject snow;

    [Header("컨트롤러")]
    GameObject player;
    PhotonView pv;
    Animator ani;
    public NavMeshAgent agent;
    bool isRunning;
    float originSpeed;
    [SerializeField]
    Sprite[] spritesRun;
    Image imageRun;
    [SerializeField]
    KeyCode runKey = KeyCode.R;    

    [Header("조이스틱")]
    [SerializeField]
    GameObject joystick;

    [Header("슈팅 게임")]
    public Transform shootingArea;
    public ParticleSystem respawnEffect;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        imageRun = UIManagerWorld.Instance.buttonRun.GetComponent<Image>();
        PhotonManager.Instance.PlayerCreated += SetPlayer;
        StartCoroutine(CheckWeather(1));
        StartCoroutine(CheackTime()); 
        
#if UNITY_EDITOR_WIN || UNITY_STANDALONE || UNITY_EDITOR_OSX
        joystick.SetActive(false);
#elif UNITY_ANDROID
        battery.SetActive(true);
        StartCoroutine(CheckBattery(1));
#endif
    }    

    void Update()
    {
        if (Input.GetKeyDown(runKey))
        {
            ToggleRun();
        }
    }

    void SetPlayer()
    {
        player = PhotonManager.Instance.player.gameObject;
        ani = player.GetComponent<Animator>();
        agent = player.GetComponent<NavMeshAgent>();
        originSpeed = agent.speed;
        pv = player.GetComponent<PhotonView>();
        respawnEffect = player.transform.Find("Magic Circle").GetComponent<ParticleSystem>();
    }

    IEnumerator CheckWeather(float time)
    {
        while(true)
        {
            // 현재 시간과 밤낮효과
            DateTime currentTime = DateTime.Now;
            int currentHour = currentTime.Hour;

            #region Day and Night
            dLight.SetActive(true);
            string hour = string.Empty;
            if (currentHour >= 7 && currentHour < 16)
            {
                hour = "아침";
                skyBoxTemp = skyBoxDay;
            }
            else if (currentHour >= 16 && currentHour < 19)
            {
                hour = "오후";
                skyBoxTemp = skyBoxMiddle;
            }
            else if (currentHour >= 19 || currentHour < 4)
            {
                hour = "밤";
                dLight.SetActive(false);
                skyBoxTemp = skyBoxNight;
            }
            else if (currentHour >= 4 && currentHour < 7)
            {
                hour = "새벽";
                skyBoxTemp = skyBoxMiddle;
            }
            RenderSettings.skybox = skyBoxTemp;
            Debug.Log("현재 시간 : " + hour + " " + currentHour + "시");

            #endregion

            #region Weather 

            string baseDate = currentTime.ToString();
            string baseTime = string.Empty;

            // 필요없는 문자 제거
            baseDate = new string(baseDate.Where(char.IsDigit).ToArray());
            baseDate = baseDate.Substring(0, 8); // "20231011"

            // 이전 코드
            #region            
            // open Api의 시간 갱신 시간이 3시간 텀이므로 하루를 8로 나눠준다.
            //int range = (currentHour / 300) % 8;
            //switch (range)
            //{
            //    case 0:
            //        baseTime = "0200";
            //        break;
            //    case 1:
            //        baseTime = "0500";
            //        break;
            //    case 2:
            //        baseTime = "0800";
            //        break;
            //    case 3:
            //        baseTime = "1100";
            //        break;
            //    case 4:
            //        baseTime = "1400";
            //        break;
            //    case 5:
            //        baseTime = "1700";
            //        break;
            //    case 6:
            //        baseTime = "2000";
            //        break;
            //    case 7:
            //        baseTime = "2300";
            //        break;
            //}
            #endregion  
                        
            // 매 시간 30분에 데이터가 생성되기 때문에 30분 이전의 시간이라면 한 시간 전의 정보를 받아야 한다.
            if(currentTime.Minute < 30)
            {
                currentTime = currentTime.AddHours(-1);
                baseTime = currentTime.ToString("HHmm");
                Debug.Log("30분 이전입니다. 한 시간 전 값 : " + baseTime);
            }
            else
            {
                baseTime = currentTime.ToString("HHmm");
                Debug.Log("30분 이후입니다. 현재 시간 값 : " + baseTime);
            }

            StartCoroutine(RequestWeatherData(baseDate, baseTime));

            #endregion

            yield return new WaitForSeconds(time * 3600f); // 3600 sec = 1 hour
        }        
    }

    // 배터리 관련
    IEnumerator CheckBattery(float seconds)
    {
        while (true)
        {
            // 배터리 잔량 0.0 - 1.0
            float batteryLevel = SystemInfo.batteryLevel;

            // 배터리 충전 유무에 따른 이미지 변경
            batteryCharging.SetActive(SystemInfo.batteryStatus == BatteryStatus.Charging);
            batteryEmpty.SetActive(SystemInfo.batteryStatus != BatteryStatus.Charging);

            // 배터리 량에 따른 이미지 변경
            batterySlider.value = batteryLevel;

            yield return new WaitForSeconds(seconds);
        }
    }

    // 타이머
    IEnumerator CheackTime()
    {    
        while(true)
        {
            timer.text = DateTime.Now.ToString("HH:mm"); ;
            yield return new WaitForSeconds(1f);
        }
    }

    public void ToggleRun()
    {
        if (!pv.IsMine)
            return;

        SoundManager.Instance.PlaySFX(SFX.Click);
        isRunning = !isRunning;
        agent.speed = isRunning ? originSpeed * 2f : originSpeed;
        imageRun.sprite = isRunning ? spritesRun[1] : spritesRun[0];        

        AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Idle"))
            return;

        SetAni();        
    }
        
    public void SetAni()
    {
        ani.SetInteger("PlayerState", isRunning ? (int)PlayerState.Run : (int)PlayerState.Walk);
        SoundManager.Instance.StopSFX(isRunning ? SFX.Running : SFX.Walking);
        SoundManager.Instance.PlaySFX(isRunning ? SFX.Running : SFX.Walking);
    }
    
    public void SetIdle()
    {
        ani.SetInteger("PlayerState", (int)PlayerState.Idle);
        SoundManager.Instance.StopSFX(isRunning ? SFX.Running : SFX.Walking);
    }

    IEnumerator RequestWeatherData(string baseDate, string baseTime)
    {
        string url = $"{Config.Base_URL}?ServiceKey={Config.Service_Key}&pageNo={Config.Page_No}&numOfRows={Config.Num_Of_Rows}&dataType={Config.Data_Type}" +
                        $"&base_date={baseDate}&base_time={baseTime}&nx={Config.Coordinate_X}&ny={Config.Coordinate_Y}";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                int requestAgain = 0;
                while (requestAgain <= 4)
                {
                    // 요청 실패 시 재요청, (5초의 간격, 5번만)
                    yield return new WaitForSeconds(5);
                    
                    StartCoroutine(RequestWeatherData(baseDate, baseTime));
                    requestAgain++;
                    Debug.Log("Error: " + www.error + " 재요청 횟수 : " + requestAgain);                    
                }                
                Debug.LogError("Failed after 5 retries. Network request failed.");                
            }
            else
            {
                // API 요청이 성공했을 때 결과 데이터 출력
                string results = www.downloadHandler.text;                            
                                
                try
                {
                    // JSON 데이터를 파싱
                    JObject parsedJson = JObject.Parse(results);

                    // 필요한 데이터에 접근
                    string obsrValue = parsedJson["response"]["body"]["items"]["item"][0]["obsrValue"].ToString();                                        
                    //Debug.Log("PTY의 obsrValue: " + obsrValue);
                    int weather = int.Parse(obsrValue);
                    SetWeather(weather);
                }
                catch (JsonException e)
                {
                    Debug.Log("JSON 파싱 오류: " + e.Message);
                }
            }
        }
    }

    void SetWeather(int weather)
    {
        Weather weatherEnum = (Weather)weather;
        Debug.Log("현재 날씨 : " + weatherEnum);

        // test
        //weather = 3;

        // 모두 비활성화 후 날씨에 따른 오브젝트 활성화
        rain.SetActive(false);
        snow.SetActive(false);

        if(weather.Equals((int)Weather.Sunny))
        {
            return;
        }
        else if(weather.Equals((int)Weather.Rainy) || weather.Equals((int)Weather.Raindrop)) 
        {
            rain.SetActive(true);
        }
        else
        {
            snow.SetActive(true);
        }
    }
}
