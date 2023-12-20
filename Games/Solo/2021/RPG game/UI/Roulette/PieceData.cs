using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   // 직렬화
public class PieceData
{
    public Sprite icon;

    public string desc;

    public Item reward;


    [Range(1, 100)]
    public int chance = 100;


    public int index;

    // 확률의 합
    public int weight;
}
