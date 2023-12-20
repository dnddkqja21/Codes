using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]   // ����ȭ
public class PieceData
{
    public Sprite icon;

    public string desc;

    public Item reward;


    [Range(1, 100)]
    public int chance = 100;


    public int index;

    // Ȯ���� ��
    public int weight;
}
