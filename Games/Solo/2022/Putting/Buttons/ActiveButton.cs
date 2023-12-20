using UnityEngine;
using UnityEngine.UI;

public class ActiveButton : MonoBehaviour
{
    Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        button.interactable = GameOption.Instance.isPressed;
    }

    public void Unpress()
    {
        GameOption.Instance.isPressed = false;
        button.interactable = GameOption.Instance.isPressed;
    }

    public void Press()
    {
        GameOption.Instance.isPressed = true;
        button.interactable = GameOption.Instance.isPressed;
    } 
    
    public void SetOriginGradients()
    {
        for (int i = 0; i < GameOption.Instance.gradients.Length; i++)
        {
            GameOption.Instance.originGradients[i] = GameOption.Instance.gradients[i];
        }
    }    
}
