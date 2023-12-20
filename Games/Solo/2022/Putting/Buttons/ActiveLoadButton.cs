using UnityEngine;
using UnityEngine.UI;

public class ActiveLoadButton : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void Loaded()
    {
        GameOption.Instance.isLoaded = true;
        button.interactable = false;
    }

    public void UnLoaded()
    {
        GameOption.Instance.isLoaded = false;
        button.interactable = true;
    }
}
