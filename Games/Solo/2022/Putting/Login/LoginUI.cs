using UnityEngine;
using System;
using TMPro;
//using UnityEngine.SceneManagement;


public class LoginUI : MonoBehaviour
{
    [Serializable]
    public class Login
    {
        public TMP_InputField ID;
        public TMP_InputField PW;
        public TextMeshProUGUI caution;        
    }

    [Space(20)]

    [Header("ID/PW 로그인")]
    public Login login;

    //string nextScene = "1. Lobby";    

    int guestNumber;

    [Space(20)]
    [Header("씬 전환 페이드 인")]
    public GameObject fadeIn;

    void Awake()
    {
        // 로그인 유저 초기화 
        GameOption.Instance.ClearUser();
        guestNumber = 1;
    }

    private void OnEnable()
    {
        login.ID.text = "";
        login.PW.text = "";
    }

    public void OnEnterGuest()
    {
        MyPuttUser guest = new MyPuttUser();
        guest.id = "Guest";
        guest.nickName = "Guest" + guestNumber++;
        guest.isGuest = true;
        AddPlayer(guest);
    }

    public void AddPlayer(MyPuttUser user)
    {
        var option = GameOption.Instance;

        if(option.playerList.Count == 0)
        {
            option.player = user;
        }

        option.playerList.Add(user);

        //gameObject.SetActive(false);
        //SceneManager.LoadScene(nextScene);
        fadeIn.SetActive(true);
    }

    public void OnEnterMember()
    {
        var option = GameOption.Instance;
        if(login.ID.text != option.id || login.PW.text != option.password && !login.caution.enabled)
        {
            login.caution.gameObject.SetActive(true);
            login.caution.enabled = true;
            return;
        }

        MyPuttUser master = new MyPuttUser();
        master.id = option.id;
        master.password = option.password;
        master.nickName = option.nickName;
        master.isGuest = false;
        AddPlayer(master);

        login.caution.enabled = false;
    }
}
