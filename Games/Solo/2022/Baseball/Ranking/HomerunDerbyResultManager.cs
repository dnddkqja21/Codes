using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

/// <summary>
/// Class : HomerunDerbyResultManager
/// Desc  : 홈런더비모드 랭킹을 위한 결과 저장, 로드 싱글톤 매니저 
/// Date  : 2022-08-20
/// Autor : Kang Cheol Woong

public class HomerunDerbyResultManager : MonoBehaviour {

    private static HomerunDerbyResultManager instance = null;
    public static HomerunDerbyResultManager Instance { get { return instance; } }

    // Record를 가진 List, 게임 종료 또는 시작 시 List로 가져오거나 내보내야 한다. 
    public List<Record> recordList = new List<Record>();
    
    // 난이도 별 랭킹 데이터 (class) 게임 중에는 난이도 별로 나누어 랭킹보드에 출력하고,
    // 게임 종료 시 나누었던 것을 다시 합쳐 저장해야 함.
    public List<Record> rookieData  = new List<Record>();
    public List<Record> amateurData = new List<Record>();
    public List<Record> minorData   = new List<Record>();
    public List<Record> majorData   = new List<Record>();

    // 2022-08-31 전국 랭킹 전용 리스트
    public List<Record> globalRookieData    = new List<Record>();
    public List<Record> globalAmateurData   = new List<Record>();
    public List<Record> globalMinorData     = new List<Record>();
    public List<Record> globalMajorData     = new List<Record>();

    // 2022-08-26 최근 플레이어 출력만을 위한 변수
    public LastPlayerRecord LastPlayerData = new LastPlayerRecord();

    public int maxRecord = 3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // 2022-08-22 로비 씬과 게임 씬에서 사용해야 하므로 [파괴 안 함] 처리해줌
        DontDestroyOnLoad(gameObject);

        // 게임 시작 시 최근 기록에서 샵 이름을 가져와 세팅한다.
        var tempRecord = XmlSerializerManager<LastPlayerRecord>.Load(Application.dataPath + "/StreamingAssets/" + "LastPlayerRecord.xml");
        SaveGameSystem.homerunDerbyPlayerInfo.SetShopName(tempRecord.shopName);
        Debug.Log(SaveGameSystem.homerunDerbyPlayerInfo.GetShopName());

        // 게임시작 시 불러와서 게임하는 동안 갖고 있는다.
        // 다른 곳에서 사용하므로 awake에서 미리 불러와야 함.
        LoadRecord();
    }
        
    // class데이터 로드
    public void LoadRecord()
    {
        // 2022-08-24 로드 이후 세이브를 하기 때문에
        // 게임 중에 갱신된 랭킹보드를 출력하기 위해 로드 시 리스트를 초기화 해준다.
        // 리스트를 비워주지 않으면 동일한 기록이 중복된다.
        rookieData.Clear();
        amateurData.Clear();
        minorData.Clear();
        majorData.Clear();        
        LastPlayerData = null;
        globalRookieData.Clear();
        globalAmateurData.Clear();
        globalMinorData.Clear();
        globalMajorData.Clear();


        string path = Application.dataPath + "/StreamingAssets/";
        string fileName = "Record.xml";

        // 2022-08-26 파일 존재 여부 확인
        string FileExist = path + fileName;
        FileInfo isFileExist = new FileInfo(FileExist);

        if (isFileExist.Exists)
        {
            var loadAllDatas = XmlSerializerManager<List<Record>>.Load(path + fileName);

            // 스위치 문에서 분기 조건으로 사용하기 위한 임시str
            string tempStr = "";

            // 취합된 정보를 가져와 나의 매장 정보만 골라낸다.
            // 레코드 요소 중 매장명 변수가 없는 요소가 있으면 오류가 난다.
            List<Record> loadDatas = loadAllDatas.FindAll(x => x.shopName.Equals(SaveGameSystem.homerunDerbyPlayerInfo.GetShopName())).ToList();

            // 2022-09-06 모든 매장(전국)데이터에서 우리 매장의 데이터만 제거
            loadAllDatas.RemoveAll(x => x.shopName.Equals(SaveGameSystem.homerunDerbyPlayerInfo.GetShopName()));

            // 가져온 Record리스트의 원소 각각 Record 클래스의 level을 확인 후 난이도 별 리스트에 저장.         
            for (int i = 0; i < loadDatas.Count; i++)
            {
                tempStr = loadDatas[i].level;

                switch (tempStr)
                {
                    case "JUNIOR":
                        rookieData.Add(loadDatas[i]);
                        break;
                    case "AMATEUR":
                        amateurData.Add(loadDatas[i]);
                        break;
                    case "SEMI_PRO":
                        minorData.Add(loadDatas[i]);
                        break;
                    case "PRO":
                        majorData.Add(loadDatas[i]);
                        break;
                }
            }

            // 2022-09-06 제거 전에 매장 데이터 소팅
            SortingData();

            // 난이도 별로 3개 이상의 기록은 제거하여 필요한 개수만 가지고 있도록 한다.
            RemoveRecord();

            // 위에서 3개 이상의 기록 제거 후 다시 전국 데이터에 추가한다.
            loadAllDatas.AddRange(rookieData);
            loadAllDatas.AddRange(amateurData);
            loadAllDatas.AddRange(minorData);
            loadAllDatas.AddRange(majorData);

            // 전국 매장의 순위
            for (int i = 0; i < loadAllDatas.Count; i++)
            {
                tempStr = loadAllDatas[i].level;
                switch (tempStr)
                {
                    case "JUNIOR":
                        globalRookieData.Add(loadAllDatas[i]);
                        break;
                    case "AMATEUR":
                        globalAmateurData.Add(loadAllDatas[i]);
                        break;
                    case "SEMI_PRO":
                        globalMinorData.Add(loadAllDatas[i]);
                        break;
                    case "PRO":
                        globalMajorData.Add(loadAllDatas[i]);
                        break;
                }
            }
        }

        // 2022-09-06 최종적으로 전국 데이터 소팅
        SortingGlobalData();

        // 2022-08-26 최근 기록
        fileName = "LastPlayerRecord.xml";
        
        FileExist = path + fileName;
        isFileExist = new FileInfo(FileExist);

        // 불러올 파일이 없을 때의 예외처리
        if (isFileExist.Exists)
        {
            var loadLastData = XmlSerializerManager<LastPlayerRecord>.Load(path + fileName);
            LastPlayerData = loadLastData;
        }
        else
        {
            Debug.Log("최근 기록 없음. 최소 1회 이상 홈런더비를 플레이해야 합니다.");            
        }
    }

    // 데이터 세이브
    public void SaveRecord()
    {
        // 게임을 종료하지 않고 로비로 돌아가 다시 시작하는 경우에
        // 저장하게되면 recordList가 계속 쌓이기 때문에 초기화 필요하다.
        recordList.Clear();
        LastPlayerData = null;

        // 순위를 Sorting하기위해 필요한 data는 [홈런 갯수, 최대 거리, 시간, 이름, 난이도]이다.
        // 홈런 갯수를 최우선으로 따지며, 동일 갯수이면 최대 거리, 동일하면 시간으로 정렬한다. 
        
        // xml 저장
        string path = Application.dataPath + "/StreamingAssets/";
        string fileName = "Record.xml";

        // 2022-09-06 전국 기록을 다시 저장
        recordList.AddRange(globalRookieData);
        recordList.AddRange(globalAmateurData);
        recordList.AddRange(globalMinorData);
        recordList.AddRange(globalMajorData);

        // 현재 게임의 기록을 리스트에 저장
        Record presentData = new Record();

        presentData.homerunCount = SaveGameSystem.homerunDerbyPlayerInfo.homerunCount.ToString();
        presentData.bestScore = SaveGameSystem.GetBestScore().ToString();
        presentData.playDateTime = System.DateTime.Now.ToString();
        presentData.playerName = SaveGameSystem.homerunDerbyPlayerInfo.GetPlayerName();
        presentData.level = SaveGameSystem.homerunDerbyPlayerInfo.GameLevel.ToString();
        presentData.shopName = SaveGameSystem.homerunDerbyPlayerInfo.GetShopName();

        // List에 추가, 기록 저장
        recordList.Add(presentData);
        XmlSerializerManager<List<Record>>.Save(path + fileName, recordList);

        // 2022-08-26 최근 기록만을 위한 저장 기능
        LastPlayerRecord lastData = new LastPlayerRecord();

        lastData.homerunCount  = SaveGameSystem.homerunDerbyPlayerInfo.homerunCount.ToString();
        lastData.playerName    = SaveGameSystem.homerunDerbyPlayerInfo.GetPlayerName();
        lastData.shopName      = SaveGameSystem.homerunDerbyPlayerInfo.GetShopName();
        XmlSerializerManager<LastPlayerRecord>.Save(path + "LastPlayerRecord.xml", lastData);
    }

    // 2022-08-31 매장 데이터 소팅
    public void SortingData()
    {
        rookieData  = Order(rookieData);
        amateurData = Order(amateurData);
        minorData   = Order(minorData);
        majorData   = Order(majorData);        
    }

    // 2022-08-26 글로벌 데이터 소팅
    public void SortingGlobalData()
    {
        globalRookieData    = Order(globalRookieData);
        globalAmateurData   = Order(globalAmateurData);
        globalMinorData     = Order(globalMinorData);
        globalMajorData     = Order(globalMajorData);
    }

    // 홈런 개수, 최장타, 최근 기록 순으로 소팅한다.
    public List<Record> Order(List<Record> list)
    {           // string으로 기록하였기 때문에 int형으로 바꿔야만 소팅 시 버그 안 남
        return list.OrderByDescending(x => int.Parse(x.homerunCount)).
                    ThenByDescending(x => x.bestScore).
                    ThenByDescending(x => x.playDateTime).ToList();
    }

    // 약속한 개수를 초과하는 데이터 삭제
    public void RemoveRecord()
    {    
        if (rookieData.Count > maxRecord)
        {
            rookieData.RemoveRange(maxRecord, rookieData.Count - maxRecord);
        }
        if (amateurData.Count > maxRecord)
        {
            amateurData.RemoveRange(maxRecord, amateurData.Count - maxRecord);
        }
        if (minorData.Count > maxRecord)
        {
            minorData.RemoveRange(maxRecord, minorData.Count - maxRecord);
        }
        if (majorData.Count > maxRecord)
        {
            majorData.RemoveRange(maxRecord, majorData.Count - maxRecord);
        }
    }
}


