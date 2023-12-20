using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    [Header("변경할 스프라이트")]
    public Sprite[] sprites;

    SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Change();
    }

    public void Change()
    {
        sprite.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
