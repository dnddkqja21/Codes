using UnityEngine;
using UnityEngine.SceneManagement;

// �ӽ� ���� ���� ��ư
public class ResetToInitScene : MonoBehaviour
{
    string initScene = "0. Init";
    public void ResetToInit()
    {
        // ��� ���� �̱��� ��ȹ�� 20������

        SceneManager.LoadScene(initScene);
        GameOption.Instance = null;
    }
}
