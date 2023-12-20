using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using UnityEngine.UI;
using TMPro;
using System;

public class RankManager 
{
    static RankManager instance = null;

    public static RankManager Instance { get { if (instance == null) { instance = new RankManager(); } return instance; } }

    List<GameObject> rankItemPool = new List<GameObject>();

    const string Record_Maze        = "recordMaze";
    const string Record_Color       = "recordColor";
    const string Record_Shooting    = "recordShooting";

    const string Record_Maze_HOF        = "recordMazeHOF";
    const string Record_Color_HOF       = "recordColorHOF";
    const string Record_Shooting_HOF    = "recordShootingHOF";

    public void InsertGameRecord<T>(string uuid, string record, T value)
    {
        string rankUUID = uuid; 

        string tableName = "PlayerData";
        string rowInDate = string.Empty;

        // 랭킹을 삽입하기 위해서는 게임 데이터에서 사용하는 데이터의 inDate값이 필요.          
        Debug.Log("데이터 조회를 시도합니다.");
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("데이터 조회 중 문제가 발생했습니다 : " + bro);
            return;
        }

        Debug.Log("데이터 조회에 성공했습니다 : " + bro);

        if (bro.FlattenRows().Count > 0)
        {
            rowInDate = bro.FlattenRows()[0]["inDate"].ToString();
        }
        else
        {
            Debug.Log("데이터가 존재하지 않습니다. 데이터 삽입을 시도합니다.");
            var bro2 = Backend.GameData.Insert(tableName);

            if (bro2.IsSuccess() == false)
            {
                Debug.LogError("데이터 삽입 중 문제가 발생했습니다 : " + bro2);
                return;
            }

            Debug.Log("데이터 삽입에 성공했습니다 : " + bro2);

            rowInDate = bro2.GetInDate();
        }
        //Debug.Log("내 게임 정보의 rowInDate : " + rowInDate); 

        Param param = new Param();
        switch (record)
        {
            case Record_Maze:
            case Record_Maze_HOF:
                if (typeof(T) == typeof(float))
                {                    
                    float originScore = record.Equals(Config.Record_Maze) ? float.Parse(bro.FlattenRows()[0][Config.Record_Maze].ToString()) :
                                                                            float.Parse(bro.FlattenRows()[0][Config.Record_Maze_HOF].ToString());

                    float currentScore = (float)(object)value;

                    if(originScore == 0)
                    {
                        Debug.Log("주간 초기화 후 첫 데이터 갱신입니다.");
                        param.Add(record, value);
                    }
                    else if (originScore <= currentScore)
                    {
                        Debug.Log("기존 점수가 현재 점수보다 높기 때문에 데이터 갱신하지 않습니다.");
                        return;
                    }
                    else
                    {
                        param.Add(record, value);
                    }
                }
                break;

            case Record_Color:
            case Record_Color_HOF:
                if (typeof(T) == typeof(int))
                {
                    int originScore = record.Equals(Config.Record_Color) ? int.Parse(bro.FlattenRows()[0][Config.Record_Color].ToString()) :
                                                                            int.Parse(bro.FlattenRows()[0][Config.Record_Color_HOF].ToString());
                    int currentScore = (int)(object)value;

                    if (originScore >= currentScore)
                    {
                        Debug.Log("기존 점수가 현재 점수보다 높기 때문에 데이터 갱신하지 않습니다.");
                        return;
                    }
                    else
                    {
                        param.Add(record, value);
                    }
                }
                break;

            case Record_Shooting:
            case Record_Shooting_HOF:
                if (typeof(T) == typeof(int))
                {
                    int originScore = record.Equals(Config.Record_Shooting) ? int.Parse(bro.FlattenRows()[0][Config.Record_Shooting].ToString()) : 
                                                                             int.Parse(bro.FlattenRows()[0][Config.Record_Shooting_HOF].ToString());                    
                    //Debug.Log("현재 점수 : " + originScore);
                    value = (T)Convert.ChangeType(Convert.ToInt32(value) + originScore, typeof(T));
                    //Debug.Log("합산 점수 : " + value);

                    param.Add(record, value);                    
                }
                break;
        }        

        // 추출된 rowIndate를 가진 데이터에 param값으로 수정을 진행하고 랭킹에 데이터를 업데이트.  
        Debug.Log("랭킹 삽입을 시도합니다.");
        var rankBro = Backend.URank.User.UpdateUserScore(rankUUID, tableName, rowInDate, param);

        if (rankBro.IsSuccess() == false)
        {
            Debug.LogError("랭킹 등록 중 오류가 발생했습니다. : " + rankBro);
            return;
        }

        Debug.Log("랭킹 삽입에 성공했습니다. : " + rankBro);
    }


    public void GetGameRecord<T>(string uuid) where T : RecordBase
    {
        string userUuid = uuid;  

        BackendReturnObject bro = Backend.URank.User.GetRankList(userUuid);

        if(rankItemPool != null)
        {
            foreach (var item in rankItemPool)
            {
                item.SetActive(false);
            }
        }

        if (bro.IsSuccess())
        {
            LitJson.JsonData rankListJson = bro.GetFlattenJSON();

            for (int i = 0; i < rankListJson["rows"].Count; i++)
            {
                T rankItem = Activator.CreateInstance<T>();                

                rankItem.nickname = rankListJson["rows"][i]["nickname"].ToString();
                rankItem.score = rankListJson["rows"][i]["score"].ToString();
                rankItem.rank = rankListJson["rows"][i]["rank"].ToString();

                if (i < rankItemPool.Count)
                {
                    // 오브젝트 풀링 사용
                    UpdateRankItem(rankItemPool[i], rankItem);
                }
                else
                {
                    // 오브젝트 풀링에 더 이상 오브젝트가 없는 경우
                    GameObject rankItemObject = InitializeRankItem();
                    UpdateRankItem(rankItemObject, rankItem);
                }
            }
        } 
    }

    GameObject InitializeRankItem()
    {
        GameObject tempRank = GameObject.Instantiate(UIManagerWorld.Instance.rankItem, UIManagerWorld.Instance.contentRank);
        tempRank.SetActive(false);
        rankItemPool.Add(tempRank);
        return tempRank;
    }

    void UpdateRankItem<T>(GameObject rankItem, T record) where T : RecordBase 
    {
        rankItem.SetActive(true);

        int ranking = int.Parse(record.rank);

        // 아이콘 설정
        if (ranking < 4)
        {
            rankItem.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = UIManagerWorld.Instance.medals[ranking];
        }
        else
        {
            rankItem.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = UIManagerWorld.Instance.medals[0];
            rankItem.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = record.rank;
        }

        // 닉네임 설정
        rankItem.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = record.nickname;

        // 기록 (소수점 3자리까지 표기) 
        rankItem.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text = 
            record.score.Length >= 6 ? record.score.Substring(0, 6) : record.score;
    }
}
