using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    [Header("�ε� ��ư")]
    public Button button;
    private void OnEnable()
    {
        button.interactable = false;
    }
}
