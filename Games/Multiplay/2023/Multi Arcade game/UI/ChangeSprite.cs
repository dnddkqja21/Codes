using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour
{
    Button button;

    [SerializeField]
    Sprite[] sprites;

    Image image;
    bool isChange;

    void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnChangeSprite);
    }

    public void OnChangeSprite()
    {      
        isChange = !isChange;
        image.sprite = isChange ? sprites[1] : sprites[0];
    }
}
