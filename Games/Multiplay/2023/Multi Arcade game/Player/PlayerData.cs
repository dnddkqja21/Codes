using BackEnd;
using UnityEngine;

/// <summary>
/// 플레이어 데이터를 저장
/// </summary>

public static class PlayerData 
{
    public static string nickName       = string.Empty;
    public static int avatarNumber      = -1;
    public static int grade             = -1;
    public static float recordMaze      = 0;
    public static int recordColor       = 0;
    public static int recordShooting    = 0;
    public static float recordMazeHOF   = 0;
    public static int recordColorHOF    = 0;
    public static int recordShootingHOF = 0;

    public static void SetGameData()
    {
        Param param = new Param();
        param.Add("nickName", nickName);
        param.Add("avatarNumber", avatarNumber);
        param.Add(Config.Record_Maze, recordMaze);
        param.Add(Config.Record_Color, recordColor);
        param.Add(Config.Record_Shooting, recordShooting);
        param.Add(Config.Record_Maze_HOF, recordMazeHOF);
        param.Add(Config.Record_Color_HOF, recordColorHOF);
        param.Add(Config.Record_Shooting_HOF, recordShootingHOF);

        var bro = Backend.GameData.Insert("PlayerData", param);

        if (bro.IsSuccess())
        {
            Debug.Log("게임정보 데이터 삽입에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("게임정보 데이터 삽입에 실패했습니다. : " + bro);
        }
    }

    public static bool GetGameData()
    {
        Debug.Log("게임 정보 조회 함수를 호출합니다.");
        var bro = Backend.GameData.GetMyData("PlayerData", new Where());
        bool isReady = false;
        if (bro.IsSuccess())
        {
            Debug.Log("게임 정보 조회에 성공했습니다. : " + bro);


            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터를 받아옵니다.  

            // 받아온 데이터의 갯수가 0이라면 데이터가 존재하지 않는 것입니다.  
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("데이터가 존재하지 않습니다.");
                string text = LocalizationManager.Instance.LocaleTable("데이터없음");
                PopupManager.Instance.ShowOneButtnPopup(true, text, UIManagerInit.Instance.GoToReRegistration);

                return isReady;
            }
            else
            {
                nickName        = gameDataJson[0]["nickName"].ToString();
                avatarNumber    = int.Parse(gameDataJson[0]["avatarNumber"].ToString());
                recordMaze      = float.Parse(gameDataJson[0][Config.Record_Maze].ToString());
                recordColor     = int.Parse(gameDataJson[0][Config.Record_Color].ToString());
                recordShooting  = int.Parse(gameDataJson[0][Config.Record_Shooting].ToString());
                isReady = true;

                //foreach (string itemKey in gameDataJson[0]["inventory"].Keys)
                //{
                //    userData.inventory.Add(itemKey, int.Parse(gameDataJson[0]["inventory"][itemKey].ToString()));
                //}

                //foreach (LitJson.JsonData equip in gameDataJson[0]["equipment"])
                //{
                //    userData.equipment.Add(equip.ToString());
                //}

                Debug.Log("가져온 정보 : \n" +
                    "닉네임 : " + nickName + ", \n" +
                    "아바타 넘버 : " + avatarNumber + " \n" +
                    "미로 점수 : " + recordMaze + " \n" +
                    "컬러 점수 : " + recordColor + "\n" +
                    "슈팅 점수 : " + recordShooting);
                return isReady;
            }
        }
        else
        {
            Debug.LogError("게임 정보 조회에 실패했습니다. : " + bro);
            return isReady;
        }
    }

    public static void GradeUp()
    {
        // Step 4. 게임정보 수정 구현하기
    }

    public static void UpdateGameData()
    {
        // Step 4. 게임정보 수정 구현하기
    }
}
