using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Piece : MonoBehaviour
{
    [SerializeField]
    Image icon;

    [SerializeField]
    TextMeshProUGUI desc;

    [SerializeField]
    Item reward;

    public void SetPieceData(PieceData _pieceData)
    {
        icon.sprite = _pieceData.icon;
        desc.text = _pieceData.desc;
        reward = _pieceData.reward;
    }
}
