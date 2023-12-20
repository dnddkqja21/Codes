using UnityEngine;
using UnityEngine.SceneManagement;

// 임시 리셋 게임 버튼
public class ResetToInitScene : MonoBehaviour
{
    string initScene = "0. Init";
    public void ResetToInit()
    {
        // 기록 저장 미구현 기획서 20페이지

        SceneManager.LoadScene(initScene);
        GameOption.Instance = null;
    }
}
