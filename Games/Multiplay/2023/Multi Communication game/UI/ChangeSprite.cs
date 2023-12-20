using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;

    Image image;
    bool isChange;

    BubbleButton bubbleButton;

    void Awake()
    {
        bubbleButton = GetComponent<BubbleButton>();
    }

    void Start()
    {
        image = GetComponent<Image>();
        bubbleButton.SetButton += SetPlayer;
    }

    void SetPlayer()
    {
        isChange = PhotonManagerWorld.Instance.player.GetComponent<PlayerAttributes>().bubbleAccept;
        OnChangeSprite();
    }

    public void OnChangeSprite()
    {
        SoundManager.Instance.PlaySFX(SFX.MenuClick);

        isChange = !isChange;
        image.sprite = isChange ? sprites[1] : sprites[0];
    }
}
