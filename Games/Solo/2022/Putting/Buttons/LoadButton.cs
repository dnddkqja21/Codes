using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    [Header("로드 버튼")]
    public Button button;
    private void OnEnable()
    {
        button.interactable = false;
    }
}
