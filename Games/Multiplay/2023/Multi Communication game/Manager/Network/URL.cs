public static class URL_CONFIG
{
    public static string MAIN_BACK = BuildConfig.isRelease ? "https://schback2.musicen.com/" : "https://schback.musicen.com/";
    public static string MAIN_FRONT = BuildConfig.isRelease ? "https://schback2.musicen.com/front/" : "https://schback.musicen.com/front/";

    // 차후 사용 고려
    public static string Room_ID = BuildConfig.isRelease ? "https://schback2.musicen.com/front/" : "https://schback.musicen.com/front/";

    // app store
    public const string Download_PC     = "https://kongzi.sch.ac.kr/#paper04";
    public const string Download_AOS    = "https://play.google.com/store/apps/details?id=com.musicen.metaverse";
    public const string Download_IOS    = "https://apps.apple.com/kr/app/%EB%A9%94%ED%83%80%EB%B2%84%EC%8A%A4-%EA%B3%B5%EC%9E%90%EC%95%84%EC%B9%B4%EB%8D%B0%EB%AF%B8/id6464238648";

    //api
    public const string APP_CHECK = "login/appVerChk.api";
    public const string LOGIN = "login/login.api";
    public const string REFRESH_TOKEN = "login/refreshToken.api";
    public const string USER_INFO = "cmmn/userInfo.api";
    public const string WEATHER = "weather/getCurrentWeather.api";

    public const string CHECK_LIKE = "follow/likeCount.api";
    public const string LIKE_UP = "follow/userLike.api";
                                      
    public const string ALARM_LIST = "alarm/selecUserAlarm.api";
    public const string ALARM_CHECK = "alarm/updateUserAlarmCheck.api";
    
    public const string GALLERY = "experience/selectGongjarueUnityList.api";

    // 탈퇴
    public const string SECESSION = "login/userDelete.api";


    //webview
    public const string FIND_ID = "auth/login/IdFindView";
    public const string FIND_PASS = "auth/login/PwFindView";
    public const string SIGN_UP = "auth/login/SigninFormView";
    public const string COURSE = "auth/login/LessonSigninFormView"; 
    public const string TRANSLATOR = "translation";    

    // 신고하기
    public const string MAIN_REPORT = "sttment?";

    // Interior
    public const string EXPERIENCE_CENTER = "experience/menu";
    public const string CLASS_ROOM = "lecture/menu";
    public const string VOD_CENTER = "vod/menu";
    public const string SEMINAR_ROOM = "semina/menu";
    public const string PROFESSOR_OFFICE = "professorClassroom/menu";
    public const string STUDY_ROOM = "study/menu";
    public const string MY_ROOM = "mypage/menu";
    public const string SERVICE_CENTER = "support/menu";
    public const string SCHEDULE = "lecture/mySchedule";

    // Open Web Browser
    public const string HSK_CENTER = "http://xn--ob0btg19m4mai66amijyvfn8ee7n9seuzx9za.com/bbs/content.php?co_id=Introduction";
    public const string CHINESE_STUDIES = "http://schcisc.or.kr/";

    // 홍보 영상, 이미지
    public const string VIDEO = "banner/adVideo.mp4";
    public const string IMAGE = "banner/adImage.png";    
}