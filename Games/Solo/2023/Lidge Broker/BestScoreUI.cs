using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestScoreUI : MonoBehaviour
{
    Text bestScore;

    void Start()
    {
        if(!PlayerPrefs.HasKey("Score"))
        {
            return;
        }
        bestScore = GetComponent<Text>();
        bestScore.text = "최고점수 " + string.Format("{0:F0}", PlayerPrefs.GetFloat("Score"));
    }
}
