using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agora.Rtc;
using UnityEngine.UI;

/// <summary>
/// 화상 채팅 관련
/// </summary>

public class VideoChat : MonoBehaviour
{   
    static VideoChat instance = null;
    public static VideoChat Instance {  get { return instance; } }

    string appID = "01e106a14152485ab624f626c39ad665";
    string token = "";
    string currentChannelName;

    [Header("화상 채팅 컴포넌트")]
    [SerializeField]
    Animator videoChatBG;    
    Button buttonUpDown;
    RectTransform upDownArrow;
    bool isShow = true;

    static IRtcEngine rtcEngine;
    static GameObject videoChatObj;
    static Transform videoChatLayout;
    static bool isMuteAudio;
    static bool isMuteVideo;
    
    static List<Sprite> spritesAudio = new List<Sprite>();    
    static List<Sprite> spritesVideo = new List<Sprite>();

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        buttonUpDown = videoChatBG.transform.GetChild(0).GetComponent<Button>();
        upDownArrow = buttonUpDown.transform.GetChild(0).GetComponent<RectTransform>();
        videoChatLayout = videoChatBG.transform.GetChild(1);
        videoChatObj = Resources.Load<GameObject>("Video Chat Obj");
        spritesAudio.Add(Resources.Load<Sprite>("Audio On"));
        spritesAudio.Add(Resources.Load<Sprite>("Audio Off"));
        spritesVideo.Add(Resources.Load<Sprite>("Video On"));
        spritesVideo.Add(Resources.Load<Sprite>("Video Off"));

        SetupVideoSDKEngine();
        InitEventHandler();
        SetBasicConfiguration();

        buttonUpDown.onClick.AddListener(() =>
        {
            OnUpAndDown();
        });
    }

    // 가상 배경 테스트
    //void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Q))
    //    {
    //        SetVirtualBackground();
    //    }
    //}

    void SetupVideoSDKEngine()
    {
        // 아고라 인스턴스 생성.
        rtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        // 세팅.
        RtcEngineContext context = new RtcEngineContext(appID, 0,
        CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
        AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT, AREA_CODE.AREA_CODE_GLOB, null);

        // rtc엔진 초기화
        rtcEngine.Initialize(context);
        Debug.Log("아고라 엔진 셋업");
    }

    void InitEventHandler()
    {
        UserEventHandler handler = new UserEventHandler(this);
        rtcEngine.InitEventHandler(handler);
        Debug.Log("아고라 유저 이벤트 핸들러");
    }

    // 베이직 세팅
    void SetBasicConfiguration()
    {
        rtcEngine.EnableAudio();
        rtcEngine.EnableVideo();
        VideoEncoderConfiguration config = new VideoEncoderConfiguration();

        // 화면 영역
        config.dimensions = new VideoDimensions(300, 300);
        config.frameRate = 15;
        config.bitrate = 0;
        rtcEngine.SetVideoEncoderConfiguration(config);
        rtcEngine.SetChannelProfile(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION);
        rtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
    }

    public void Join(string channelName)
    {
        currentChannelName = channelName;        
        rtcEngine.JoinChannel(token, channelName);        
        rtcEngine.EnableVideo();

        videoChatBG.SetBool("isCreate", true);        

        MakeVideoView(0, channelName);        
        Debug.Log("아고라 채널 조인 : " + channelName);        
    }

    public void Leave()
    {        
        rtcEngine.LeaveChannel();
        rtcEngine.DisableVideo();
        DestroyAll();

        videoChatBG.SetBool("isCreate", false);
        isShow = false;
        OnUpAndDown();

        Debug.Log("아고라 채널 떠남");
    }

    void DestroyAll()
    {
        List<uint> uids = new List<uint>();

        for (int i = 0; i < videoChatLayout.transform.childCount; i++)
        {
            uids.Add(uint.Parse(videoChatLayout.transform.GetChild(i).name));
        }

        for (int i = 0; i < uids.Count; i++)
        {
            DestroyVideoView(uids[i]);
        }
    }   

    // 뷰 생성
    static void MakeVideoView(uint uid, string channelId = "")
    {        
        var videoSurface = MakeImageSurface(uid.ToString());
        if (ReferenceEquals(videoSurface, null)) return;

        // 리모트, 로컬 유저
        if (uid == 0)
        {
            videoSurface.SetForUser(uid, channelId);
        }
        else
        {
            videoSurface.SetForUser(uid, channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);          
        }

        //videoSurface.OnTextureSizeModify += (int width, int height) =>
        //{
        //    // 화면 비율이 1:1이 아닐 때 보정
        //    float scale = (float)height / (float)width;
        //    videoSurface.transform.localScale = new Vector3(-3f * scale, 3f * scale, 1);
        //    Debug.Log("OnTextureSizeModify: " + width + "  " + height);
        //};

        videoSurface.SetEnable(true);
    }

    static void DestroyVideoView(uint uid)
    {
        var obj = Config.FindChild(UIManagerWorld.Instance.canvas, uid.ToString()).gameObject;
        if (!ReferenceEquals(obj, null))
        {
            Destroy(obj);
        }
    }

    // 비디오 서페이스
    static VideoSurface MakeImageSurface(string name)
    {
        GameObject videoChat = Instantiate(videoChatObj);

        if (videoChat == null)
        {
            return null;
        }
        videoChat.name = name;

        if (videoChatLayout != null)
        {
            videoChat.transform.SetParent(videoChatLayout, false);
            Debug.Log("Add video view");
        }
        else
        {
            Debug.Log("Layout is null");
        }
        // 오디오, 비디오 버튼
        videoChat.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
        {
            ToggleAudio(videoChat.name, videoChat.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>());
        });
        videoChat.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Button>().onClick.AddListener(() =>
        {
            ToggleVideo(videoChat.name, videoChat.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>());
        });

        videoChat.transform.Rotate(0f, 0.0f, 180.0f);
        videoChat.transform.localPosition = Vector3.zero;
        var videoSurface = videoChat.AddComponent<VideoSurface>();

        return videoSurface;
    }

    void OnUpAndDown()
    {
        isShow = !isShow;
        videoChatBG.SetBool("isShow", isShow);

        Invoke("ChangeArrow", 1f);
    }

    void ChangeArrow()
    {
        if (upDownArrow != null)
        {
            float z = isShow ? 0 : 180;
            upDownArrow.eulerAngles = new Vector3(0, 0, z);
        }
    }

    static void ToggleAudio(string name, Image image)
    {
        isMuteAudio = !isMuteAudio;
        image.sprite = isMuteAudio ? spritesAudio[1] : spritesAudio[0];
        // 0이면 나
        if (name.Equals("0"))
        {
            rtcEngine.EnableLocalAudio(isMuteAudio);
        }
        else
        {
        // uid 지정 뮤트
            rtcEngine.MuteRemoteAudioStream(uint.Parse(name), isMuteAudio);
        }
    }

    static void ToggleVideo(string name, Image image)
    {
        isMuteVideo = !isMuteVideo;
        image.sprite = isMuteVideo ? spritesVideo[1] : spritesVideo[0];

        if (name.Equals("0"))
        {
            rtcEngine.EnableLocalVideo(isMuteVideo);
        }
        else
        {
            rtcEngine.MuteRemoteVideoStream(uint.Parse(name), isMuteAudio);
        }
    }

    void SetVirtualBackground()
    {        
        VirtualBackgroundSource virtualBackgroundSource = new VirtualBackgroundSource();

        // Set a solid background color
        virtualBackgroundSource.background_source_type = BACKGROUND_SOURCE_TYPE.BACKGROUND_COLOR;

        uint[] options = { 0xFFBB7, 0xFFA477, 0xED7855, 0xB85C4, 0xAC6D7, 0x63465A };
        int random = Random.Range(0, options.Length);
        virtualBackgroundSource.color = options[random];        

        // Set processing properties for background
        SegmentationProperty segmentationProperty = new SegmentationProperty();
        segmentationProperty.modelType = SEG_MODEL_TYPE.SEG_MODEL_AI; // Use SEG_MODEL_GREEN if you have a green background
        segmentationProperty.greenCapacity = 0.5F; // Accuracy for identifying green colors (range 0-1)

        rtcEngine.EnableVirtualBackground(true, virtualBackgroundSource, segmentationProperty);
    }

    internal class UserEventHandler : IRtcEngineEventHandler
    {
        readonly VideoChat _videoSample;

        internal UserEventHandler(VideoChat videoSample)
        {
            _videoSample = videoSample;
        }
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            Debug.Log("You joined channel: " + connection.channelId);
        }
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            DestroyVideoView(uid);
        }
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {            
            MakeVideoView(uid, _videoSample.currentChannelName);
        }
    }
}
