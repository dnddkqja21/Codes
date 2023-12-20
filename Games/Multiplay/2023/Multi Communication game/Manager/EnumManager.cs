/// <summary>
/// enum 관리
/// </summary>

public class EnumToData
{
    static EnumToData instance = null;
    public static EnumToData Instance
    {
        get { if(instance == null) { instance = new EnumToData(); } return instance; }
    }
    // 함수 작성
    public string SceneNameToString(SceneName sceneName)
    {
        string temp = "";

        switch (sceneName)
        {
            case SceneName.Init:
                temp = "00. Init";
                break;
            case SceneName.World:
                temp = "01. World";
                break;
            case SceneName.Interior:
                temp = "02. Interior";
                break;            
        }
        return temp;
    }

    // 건물 이름을 URL로
    public string BuildingNameToURL(string buildingName)
    {
        string temp = "";

        switch (buildingName)
        {
            
            case "ExperienceCenter":
                temp = URL_CONFIG.MAIN_FRONT + URL_CONFIG.EXPERIENCE_CENTER;                            
                break;
            case "ClassRoom":
                temp = URL_CONFIG.MAIN_FRONT + URL_CONFIG.CLASS_ROOM;                
                break;
            case "VODCenter":
                temp = URL_CONFIG.MAIN_FRONT + URL_CONFIG.VOD_CENTER;
                break;
            case "SeminarRoom":
                temp = URL_CONFIG.MAIN_FRONT + URL_CONFIG.SEMINAR_ROOM;
                break;
            case "ProfessorOffice":
                temp = URL_CONFIG.MAIN_FRONT + URL_CONFIG.PROFESSOR_OFFICE;
                break;
            case "StudyRoom":
                temp = URL_CONFIG.MAIN_FRONT + URL_CONFIG.STUDY_ROOM;
                break;
            case "MyRoom":
                temp = URL_CONFIG.MAIN_FRONT + URL_CONFIG.MY_ROOM;
                break;
            case "ServiceCenter":
                temp = URL_CONFIG.MAIN_FRONT + URL_CONFIG.SERVICE_CENTER;
                break;
            case "HSKCenter":
                // 중국학 연구소와 마찬가지로 웹 페이지 띄움
                temp = URL_CONFIG.HSK_CENTER;
                break;
            case "ChineseStudies":
                // 중국학 연구소는 open url로 웹 페이지 띄움
                temp = URL_CONFIG.CHINESE_STUDIES;
                break;
            case "Schedule":
                temp = URL_CONFIG.MAIN_FRONT + URL_CONFIG.SCHEDULE;
                break;
        }
        return temp;
    }

    // 건물 이름을 문자열로
    public string BuildingNameToString(BuildingName buildingName)
    {
        string temp = "";

        switch (buildingName)
        {
            case BuildingName.ExperienceCenter:
                temp = "ExperienceCenter";
                break;
            case BuildingName.ClassRoom:
                temp = "ClassRoom";
                break;
            case BuildingName.VODCenter:
                temp = "VODCenter";
                break;
            case BuildingName.SeminarRoom:
                temp = "SeminarRoom";
                break;
            case BuildingName.ProfessorOffice:
                temp = "ProfessorOffice";
                break;
            case BuildingName.StudyRoom:
                temp = "StudyRoom";
                break;
            case BuildingName.MyRoom:
                temp = "MyRoom";
                break;
            case BuildingName.ServiceCenter:
                temp = "ServiceCenter";
                break;
            case BuildingName.HSKCenter:
                temp = "HSKCenter";
                break;
            case BuildingName.ChineseStudies:
                temp = "ChineseStudies";
                break;
            case BuildingName.Schedule:
                temp = "Schedule";
                break;
        }
        return temp;
    }

    // 건물 이름을 로컬라이징 키로
    public string BuildingNameToKey(string buildingName)
    {
        string temp = "";

        switch (buildingName)
        {
            case "ExperienceCenter":
                temp = "체험관";
                break;
            case "ClassRoom":
                temp = "강의실";
                break;
            case "VODCenter":
                temp = "VOD관";
                break;
            case "SeminarRoom":
                temp = "세미나실";
                break;
            case "ProfessorOffice":
                temp = "교수실";
                break;
            case "StudyRoom":
                temp = "스터디룸";
                break;
            case "MyRoom":
                temp = "마이룸";
                break;
            case "ServiceCenter":
                temp = "행정지원실";
                break;
            case "HSKCenter":
                temp = "HSK센터";
                break;
            case "ChineseStudies":
                temp = "중국학연구센터";
                break;
            case "Schedule":
                temp = "달력";
                break;
        }
        return temp;
    }
}

// enum 정리

// 씬
public enum SceneName
{
    Init,               // 00. Init
    World,              // 01. World
    Interior            // 02. Interior
}

// 건물 이름
public enum BuildingName
{
    ExperienceCenter,   // 체험관
    ClassRoom,          // 강의실
    VODCenter,          // VOD관
    SeminarRoom,        // 세미나실
    ProfessorOffice,    // 교수실
    StudyRoom,          // 스터디룸
    MyRoom,             // 마이룸
    ServiceCenter,      // 행정지원실
    HSKCenter,          // HSK
    ChineseStudies,     // 중국학
    Schedule            // 달력
}

// SFX
public enum SFX
{
    MenuClick,
    OpenDoor,
    BubbleIn,
    BubbleOut = 5
}

// Gallery Contents
public enum GalleryContents
{
    Frame,
    Title,
    Desc
}

public enum FrameRatio
{
    OneByOne,
    WideLandscape,
    WidePortrait,
    UltraWide
}

