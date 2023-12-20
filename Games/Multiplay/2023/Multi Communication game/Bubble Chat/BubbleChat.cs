using Agora.Rtc;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Logger = Agora_RTC_Plugin.API_Example.Logger;

/// <summary>
/// 버블챗
/// 아고라 엔진 
/// </summary>

public class BubbleChat : MonoBehaviour
{
    static BubbleChat instance;
    public static BubbleChat Instance { get { return instance; } }

    // 채널 입장 관련
    public string appID;
    public string token;
    string channelName;
    uint remoteUid;
        
    [SerializeField]
    Text logText;

    GameObject muteButtonGroup;
    IRtcEngine rtcEngine;
    Logger logger;

    public bool bubbleOnOff = true;
    bool isMuteMic;
    bool isMuteVideo;
    public int channelInt;

    // 채널 이름, 인원수 관리, 동기화
    Dictionary<string, int> channel = new Dictionary<string, int>();
    // 테스트 뷰어    
    public TextMeshProUGUI viewer;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        logger = new Logger(logText);
        muteButtonGroup = UIManager.Instance.bCMuteGroup;

        SetupEngineAndEventHandler();
        SetBasicConfiguration();
    }

    // 아고라 엔진 셋업
    void SetupEngineAndEventHandler()
    {
        // Create an instance of the video SDK.
        rtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();

        // Creates a UserEventHandler instance.
        UserEventHandler handler = new UserEventHandler(this);

        // Specify the context configuration to initialize the created instance.
        RtcEngineContext context = 
            new RtcEngineContext(appID, 0,
            CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
            AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT, AREA_CODE.AREA_CODE_GLOB, new LogConfig("./log.txt"));

        // Initialize the instance.
        rtcEngine.Initialize(context);
        rtcEngine.InitEventHandler(handler);

        int success = rtcEngine.InitEventHandler(handler);
        string successText = success == 0 ? "성공" : "실패";
        Debug.Log("아고라 엔진 셋업 " + successText);

        var logFile = Application.persistentDataPath + "/rtc.log";
        rtcEngine.SetLogFile(logFile);
    }

    // 베이직 세팅
    void SetBasicConfiguration()
    {
        rtcEngine.EnableAudio();
        rtcEngine.EnableVideo();
        VideoEncoderConfiguration config = new VideoEncoderConfiguration();
        // 화면 영역
        config.dimensions = new VideoDimensions(640, 640);
        config.frameRate = 15;
        config.bitrate = 0;
        rtcEngine.SetVideoEncoderConfiguration(config);
        rtcEngine.SetChannelProfile(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION);
        rtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
    }

    // 화상 통화 채널 조인 및 뷰 활성
    public void Join(int channel)
    {
        channelInt = channel;
        // Join a channel.
        rtcEngine.JoinChannel(token, channel.ToString());

        // 채널 이름
        channelName = channel.ToString();
        //Debug.Log("채널 이름 " + GetChannelName());

        // 비디오 모듈 on
        rtcEngine.EnableVideo();
        // 뷰 생성
        //MakeVideoView(0);
        MakeVideoView(0, channelName);
        // 뮤트 버튼
        muteButtonGroup.SetActive(true);
    }
    public bool isJoining()
    {
        return muteButtonGroup.activeSelf;
    }

    // 채널 떠나기 및 뷰 종료
    public void Leave()
    {
        muteButtonGroup.SetActive(false);
        // Leaves the channel.
        rtcEngine.LeaveChannel();
        // Disable the video modules.
        rtcEngine.DisableVideo();
        DestroyVideoView(0);        
    }

    internal string GetChannelName()
    {
        return channelName;
    }

    // 뮤트는 아래 함수 사용해야 함 stack overflow 참고

    public void MuteMic()
    {
        rtcEngine.EnableLocalAudio(isMuteMic);
        isMuteMic = !isMuteMic;
    }

    public void MuteVideo()
    {
        rtcEngine.EnableLocalVideo(isMuteVideo);
        isMuteVideo = !isMuteVideo;
    }

    // 뷰 생성
    internal static void MakeVideoView(uint uid, string channelId = "")
    {
        var go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            go.SetActive(true);
            return; // reuse
        }

        // create a GameObject and assign to this new user
        var videoSurface = MakeImageSurface(uid.ToString());
        if (ReferenceEquals(videoSurface, null)) return;
        // configure videoSurface
        if (uid == 0)
        {
            videoSurface.SetForUser(uid, channelId);
        }
        else
        {
            videoSurface.SetForUser(uid, channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            // kcw
            //uids.Add(uid);
        }

        videoSurface.OnTextureSizeModify += (int width, int height) =>
        {
            // 화면 비율이 1:1이 아닐 때 보정
            float scale = (float)height / (float)width;
            videoSurface.transform.localScale = new Vector3(-3f * scale, 3f * scale, 1);
            Debug.Log("OnTextureSizeModify: " + width + "  " + height);
        };

        videoSurface.SetEnable(true);
    }

    internal static void DestroyVideoView(uint uid)
    {
        var go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            Destroy(go);
        }
    }

    // 비디오 서페이스
    // 현재 리모트 플레이어가 여기를 안 타고 있는 문제 (화면uid정상이지만, 사이즈와 rawImage널임.)
    private static VideoSurface MakeImageSurface(string goName)
    {
        GameObject go = new GameObject();

        if (go == null)
        {
            return null;
        }

        go.name = goName;
        // to be renderered onto
        go.AddComponent<RawImage>();
        // make the object draggable
        //go.AddComponent<UIElementDrag>();
        var canvas = GameObject.Find("Bubble Layout");
        if (canvas != null)
        {
            //go.transform.parent = canvas.transform;
            go.transform.SetParent(canvas.transform, false);
            Debug.Log("add video view");
        }
        else
        {
            Debug.Log("Canvas is null video view");
        }

        // set up transform
        go.transform.Rotate(0f, 0.0f, 180.0f);
        go.transform.localPosition = Vector3.zero;
        //go.transform.localScale = new Vector3(2f, 3f, 1f);
        go.transform.localScale = new Vector3(1f, 1f, 1f);

        // configure videoSurface
        var videoSurface = go.AddComponent<VideoSurface>();

        return videoSurface;
    }

    // 게임 종료 시 리소스 정리
    void OnApplicationQuit()
    {
        if (rtcEngine != null)
        {
            Leave();
            rtcEngine.Dispose();
            rtcEngine = null;
        }
    }

    // 채널 인원 수 관리
    public void AddHeadcount(string channelName)
    {
        // 채널 이름이 이미 있다면 인원수 증가
        if (channel.ContainsKey(channelName))
        {
            channel[channelName]++;

            //Debug.Log("채널 이름 : " + channelName + " 채널 인원 수 : " + GetHeadcount(channelName));

            //viewer.text = "channelName : " + channelName + ", headCount : " + GetHeadcount(channelName);
        }
        // 채널 이름이 없다면 채널 이름과 1명 추가, 현재 로직 상 채널이 없을 수가 없기때문에 여기로 들어오면 에러.
        // 채널이 존재하지 않는다면 새 방을 위한 넘버 부여
        else
        {
            Debug.Log("새 채널 생성");
            channel.Add(channelName, 0);
            //Debug.Log("채널 이름 : " + channelName + " 채널 인원 수 : " + GetHeadcount(channelName));
            //if (channelName >= nextChannelNumber)
            //{
            //    nextChannelNumber = channelName + 1;
            //}
        }
    }

    public void RemoveHeadcount(string channelName)
    {
        if (channel.ContainsKey(channelName))
        {
            channel[channelName]--;
            Debug.Log("채널 이름 : " + channelName + " 채널 인원 수 : " + GetHeadcount(channelName));

            // 채널에 아무도 없다면 해당 채널 제거, 버블챗 BG 비활성
            if (channel[channelName] == 0)
            {
                channel.Remove(channelName);
                UIManager.Instance.bCUpAndDown.SetActive(false);
                Debug.Log(channelName + "번 채널 제거됨");
            }
        }
    }

    public int GetHeadcount(string channelName)
    {
        if (channel.ContainsKey(channelName))
        {
            return channel[channelName];
        }
        else
        {
            return 0;
        }
    }

    // 새로운 채널 생성     
    public void CreateChannel()
    {
        int currentChannelNumber = int.Parse(channelName);
        // 1번 채널, 0명으로 시작. (이후에도 x번 채널, 0명)
        //channel.Add(currentChannelNumber, 0);
    }   

    // 유저 이벤트 핸들러
    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly BubbleChat _videoSample;

        internal UserEventHandler(BubbleChat videoSample)
        {
            _videoSample = videoSample;
        }

        public override void OnError(int err, string msg)
        {
            _videoSample.logger.UpdateLog(string.Format("OnError err: {0}, msg: {1}", err, msg));
        }

        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            int build = 0;
            Debug.Log("Agora: OnJoinChannelSuccess ");
            _videoSample.logger.UpdateLog(string.Format("sdk version: ${0}",
                _videoSample.rtcEngine.GetVersion(ref build)));
            _videoSample.logger.UpdateLog(string.Format("sdk build: ${0}",
              build));
            _videoSample.logger.UpdateLog(
                string.Format("OnJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}",
                                connection.channelId, connection.localUid, elapsed));
        }

        public override void OnRejoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            _videoSample.logger.UpdateLog("OnRejoinChannelSuccess");
        }

        public override void OnLeaveChannel(RtcConnection connection, RtcStats stats)
        {
            _videoSample.logger.UpdateLog("OnLeaveChannel");
        }

        public override void OnClientRoleChanged(RtcConnection connection, CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole, ClientRoleOptions newRoleOptions)
        {
            _videoSample.logger.UpdateLog("OnClientRoleChanged");
        }

        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            _videoSample.logger.UpdateLog(string.Format("OnUserJoined uid: ${0} elapsed: ${1}", uid, elapsed));
            BubbleChat.MakeVideoView(uid, _videoSample.GetChannelName());
        }

        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            _videoSample.logger.UpdateLog(string.Format("OnUserOffLine uid: ${0}, reason: ${1}", uid,
                (int)reason));
            BubbleChat.DestroyVideoView(uid);
        }
    }
}
