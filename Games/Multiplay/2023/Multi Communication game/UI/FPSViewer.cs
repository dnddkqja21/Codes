using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FPS뷰어 테스트 용도
/// </summary>

public class FPSViewer : MonoBehaviour
{
#if UNITY_EDITOR_WIN

    [Range(10, 150)]
    public int fontSize = 30;
    public Color color = new Color(.0f, .0f, .0f, 1.0f);
    public float width, height;

    void OnGUI()
    {
        Rect position = new Rect(width, height, Screen.width, Screen.height);

        float fps = 1.0f / Time.deltaTime;
        float ms = Time.deltaTime * 1000.0f;
        string text = string.Format("{0:N1} FPS ({1:N1}ms)", fps, ms);

        GUIStyle style = new GUIStyle();

        style.fontSize = fontSize;
        style.normal.textColor = color;

        GUI.Label(position, text, style);
    }
#endif
    //void Start()
    //{
    //    Debug.Log("유효 토큰 : " + UserData.Instance.avatarData.accessToken);
    //    Debug.Log("유저 시퀀스 : " + UserData.Instance.avatarData.userSeq);
    //    Debug.Log("유저 종류 : " + UserData.Instance.avatarData.psitnNm);
    //}

    //void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.Space))
    //    {
    //        UserData.Instance.avatarData.accessToken = "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJVU0VSXzAwMDAwMjc1Iiwicm9sZXMiOiLjhYfjhYciLCJpYXQiOjE2OTYzNzc3NTUsImV4cCI6MTY5NjM3Nzc1Nn0.QPnID3SaJL5zWt1smK61NOygEDOg1_KToelECyBsCjA";
    //        Debug.Log("만료토큰 주입");
    //    }
    //}
}
