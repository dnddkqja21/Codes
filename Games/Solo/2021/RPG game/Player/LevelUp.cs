using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUp : MonoBehaviour
{
    public int level;

    public int curExp;

    public int maxExp;

    public Player_PF player;

    public ActionController ac;

    public Animator levelup;

    public Image expCircle;

    public TextMeshProUGUI infoLV;

    public TextMeshProUGUI invenLV;

    public TextMeshProUGUI imageLV;

    void Start()
    {
        
    }

    
    void Update()
    {
        SetExp();
        LevelUpPlayer();
    }

    public void LevelUpPlayer()
    {
        if (curExp >= maxExp)
        {
            level++;
            infoLV.text = level.ToString();
            invenLV.text = level.ToString();
            imageLV.text = level.ToString();

            curExp = 0;
            maxExp += level * 10;

            player.attack+= level;
            player.defense++;

            player.maxHP += level * 10f;
            player.curHP = player.maxHP;
            player.maxMP += level * 5f;
            player.curMP = player.maxMP;

            ac.PlayClips(8);   

            levelup.SetBool("isShow", true);
            Invoke("IsHide", 3f);
        }
    }

    public void GetExp(int _exp)
    {
        curExp += _exp;
    }

    void SetExp()
    {
        expCircle.fillAmount = (float)curExp / maxExp;
    }

    void IsHide()
    {
        levelup.SetBool("isShow", false);
    }
}
