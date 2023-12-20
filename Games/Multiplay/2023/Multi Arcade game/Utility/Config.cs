using UnityEditor;
using UnityEngine;
//using System.Threading.Tasks;

/// <summary>
/// 설정 및 관리
/// </summary>

public static class Config
{
    // 버전 관리 (릴리즈 : true, 테스트 : false)
    public static bool isRelease = false;

    public static string roomVersion    = isRelease ? "1.0.0" : "0.0.0";
    public static string roomName       = isRelease ? "Woongsaverse" : "WoongsverseTest";

    public static string Store_URL       = "https://play.google.com/store/apps/details?id=com.Woongs.Metaverse";

    // Api 관리
    public static string Service_Key    = "F1Q8lmr0PxIOQC9XmlzAfgRff2tgqYdKvBABkNkSY2iaf2YzpaNgdIbiFkVkuT8guYVIKC27r0WYvr9oqsFegg%3D%3D";
    public static string Base_URL       = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst";
    public static int Page_No           = 1;
    public static int Num_Of_Rows       = 1000;
    public static string Data_Type      = "JSON"; // 또는 "XML" 등
    public static int Coordinate_X      = 55; // x와 y 값을 원하는 좌표로 설정
    public static int Coordinate_Y      = 127;

    // 뒤끝 랭킹 uuid
    public static string Rank_Uuid_Maze     = "77ae82a0-787b-11ee-a769-ed819b604aa3";
    public static string Rank_Uuid_Color    = "194ca9c0-7877-11ee-b6d8-cd39f026424d";
    public static string Rank_Uuid_Shooting = "cc023340-880e-11ee-a697-93183c33732f";

    public static string Rank_Uuid_Maze_HOF = "b33a7eb0-8815-11ee-99aa-c581c0e14120";
    public static string Rank_Uuid_Color_HOF = "ca3cd860-8815-11ee-99aa-c581c0e14120";
    public static string Rank_Uuid_Shooting_HOF = "d6103060-8815-11ee-99aa-c581c0e14120";

    // 랭킹 테이블
    public static string Record_Maze        = "recordMaze";
    public static string Record_Color       = "recordColor";
    public static string Record_Shooting    = "recordShooting";

    public static string Record_Maze_HOF    = "recordMazeHOF";
    public static string Record_Color_HOF   = "recordColorHOF";
    public static string Record_Shooting_HOF = "recordShootingHOF";

    // 텔레포트 좌표
    public static Vector3 Position_Futsal        = new Vector3(-35f, 0, 37.5f);
    public static Vector3 Position_Basket_One    = new Vector3(30.5f, 0, -34.5f);
    public static Vector3 Position_Basket_Two    = new Vector3(-47.5f, 0, 0.2f);
    public static Vector3 Position_Park_One      = new Vector3(0, 0, 25.5f);
    public static Vector3 Position_Park_Two      = new Vector3(-52f, 0, 25f);
    public static Vector3 Position_Maze          = new Vector3(26f, 0, -8.5f);
    public static Vector3 Position_Color_Picker  = new Vector3(-37f, 0, -13f);
    public static Vector3 Position_Shooting      = new Vector3(-29f, 0, 11f);

    // 애니메이션 트리거
    public static string Floating   = "Floating";
    public static string Falling    = "Falling";
    public static string Fall       = "Fall";
    public static string Shoot      = "Shoot";
    public static string Death      = "Death";
    public static string Revival    = "Revival";
    public static string Hit        = "Hit";

    // 서버측 메시지와 일치하지 않아서 사용 불가능
    public static string ConvertErrorMessage(string message)
    {
        switch (message)
        {
            // 회원가입            
            case "중복된 customId 입니다":
                message = "중복된 ID입니다";
                break;

            // 닉네임
            case "nickname을(를) 확인할 수 없습니다":
                message = "닉네임을 입력해주세요";
                break;
            case "잘못된 nickname is too long 입니다":
                message = "닉네임은 20자 이상일 수 없습니다";
                break;
            case "잘못된 beginning or end of the nickname must not be blank 입니다":
                message = "닉네임의 앞뒤에는 공백이 불가능합니다";
                break;
            case "중복된 nickname 입니다":
                message = "중복된 닉네임입니다";
                break;

            // 로그인
            case "잘못된 customId 입니다":
                message = "아이디가 존재하지 않습니다";
                break;
            case "잘못된 customPassword 입니다":
                message = "비밀번호가 틀렸습니다";
                break;
            case "금지된 blocked user 입니다":
                message = "차단당한 유저입니다";
                break;
            case "사라진 user 입니다":
                message = "탈퇴가 진행 중인 계정입니다";
                break;

            // 공통
            case "device_unique_id을(를) 확인할 수 없습니다":
                message = "디바이스 정보가 없습니다";
                break;
            case "잘못된 serverStatus: maintenance 입니다":
                message = "서버가 점검 중입니다";
                break;
            case "금지된 blocked device":
                message = "차단당한 기기입니다";
                break;
        }
        return message;
    }

    // 앱 종료
    public static void ExitApp()
    {
        string text = LocalizationManager.Instance.LocaleTable("종료안내");
        PopupManager.Instance.ShowTwoButtnPopup(false, text, Quit);
    }

    static void Quit()
    {
        SoundManager.Instance.PlaySFX(SFX.Click);
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            // 플레이 모드 종료
            EditorApplication.isPlaying = false;
        }
#else 
        Application.Quit();
#endif
    }

    // 자식 찾는 함수
    public static Transform FindChild(Transform parent, string name)
    {
        Transform result = parent.Find(name);

        if (result != null)
        {
            return result; // 찾았을 경우 반환
        }

        // 자식 오브젝트를 모두 검색
        foreach (Transform child in parent)
        {
            result = FindChild(child, name);
            if (result != null)
            {
                return result; // 찾았을 경우 반환
            }
        }

        return null; // 찾지 못한 경우 null 반환
    }
}

    //async static void TrySetGameData()
    //{
    //    await Task.Run(() => {
    //        PlayerData.SetGameData();
    //    });
    //}

public enum SceneList
{
    Init,
    World
}

public enum PanelNumber
{
    Login       = 0,
    SignUp      = 1,
    SetPlayer   = 2
}

public enum PlayerState
{
    Idle,
    Walk,
    Run   
}

public enum Weather
{
    Sunny,
    Rainy,
    RainySnowy,
    Snowy,
    Raindrop = 5,
    RaindropSnowyblow,
    Snowblow
}

public enum VideoChannelName
{
    FootballOne,
    BasketballOne,
    BasketballTwo,
    ParkOne,
    ParkTwo
}

public enum SFX
{
    Click,
    Hover = 5,
    Panel = 7,
    PortalOpen = 10,
    PortalClose,
    Success,
    Fail,
    Coin,
    MiniGameOn,
    MiniGameOff,
    Walking,
    Running,
    Shooting,
    Hit = 21,
    Death = 24
}

public enum ManualButtonIndex
{
    Prev,
    Next,
    Exit
}