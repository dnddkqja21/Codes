using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreUI : MonoBehaviour
{
    Text score;

    void Start()
    {
        score = GetComponent<Text>();
    }

    void LateUpdate()
    {
        score.text = "점수 : " + string.Format("{0:F0}", GameManager.score);
    }
}
