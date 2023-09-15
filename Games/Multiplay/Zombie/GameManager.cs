using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

// 점수와 게임 오버 여부, 게임 UI를 관리
public class GameManager : MonoBehaviourPunCallbacks, IPunObservable 
{    
    public static GameManager instance
    {
        get
        {           
            if (m_instance == null)
            {            
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }

    static GameManager m_instance; 

    public GameObject playerPrefab; 

    int score = 0; // 현재 게임 점수
    public bool isGameover { get; private set; } // 게임 오버 상태

    // 포톤 동기화 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
    {
        // 로컬 오브젝트라면 쓰기 부분이 실행됨
        if (stream.IsWriting)
        {
            // 네트워크를 통해 score 값을 보내기
            stream.SendNext(score);
        }
        else
        {
            // 리모트 오브젝트라면 읽기 부분이 실행됨 
            // 네트워크를 통해 score 값 받기
            score = (int) stream.ReceiveNext();
            // 동기화하여 받은 점수를 UI로 표시
            UIManager.instance.UpdateScoreText(score);
        }
    }


    void Awake() 
    {        
        if (instance != this)
        { 
            Destroy(gameObject);
        }
    }

    // 게임 시작과 동시에 플레이어가 될 게임 오브젝트를 생성
    void Start() 
    {      
        Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;    
        randomSpawnPos.y = 0f;

        // 네트워크 상의 모든 클라이언트들에서 생성 
        PhotonNetwork.Instantiate(playerPrefab.name, randomSpawnPos, Quaternion.identity);
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore) 
    {
        // 게임 오버가 아닌 상태에서만
        if (!isGameover)
        {
            score += newScore;
            UIManager.instance.UpdateScoreText(score);
        }
    }

    // 게임 오버 처리
    public void EndGame() 
    {
        isGameover = true;
        UIManager.instance.SetActiveGameoverUI(true);
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom() 
    {
        // 룸을 나가면 로비 씬으로 돌아감
        SceneManager.LoadScene("Lobby");
    }
}