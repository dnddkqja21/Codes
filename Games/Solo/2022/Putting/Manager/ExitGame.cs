using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{   
    string sceneName = "";
    public void Exit()
    {        
        GameOption.Instance.chargedTime -= RealTimer.Instance.elapseTime;
        sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
        GameOption.Instance.ResetSettings();
    } 
}
