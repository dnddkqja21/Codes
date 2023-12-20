using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyHp : MonoBehaviour
{
    public GameObject hpBarPos;

    public Transform hpBar;

    public Image fillHpBar;

    Vector3 offSet = new Vector3(0, 1.7f, 0);

    void Start()
    {
        
    }

    void Update()
    {
        SetHpBarPos();
        SetHpBarValue();
    }

    void SetHpBarPos()
    {
        Vector3 tmp = Camera.main.WorldToScreenPoint(hpBarPos.transform.position + offSet);
        hpBar.transform.position = tmp;
    }

    void SetHpBarValue()
    {
        Monster_PF enemy = hpBarPos.GetComponent<Monster_PF>();
        fillHpBar.fillAmount = (float)enemy.curHP / enemy.maxHP;
    }
}
