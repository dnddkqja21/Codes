using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bossHp : MonoBehaviour
{
    public GameObject hpBarPos;

    public Transform hpBar;

    public Image fillHpBar;

    public Image fillHpBar2;

    public Monster_Boss boss;

    Vector3 offSet = new Vector3(0, 1.5f, 0);

    public bool backHpBar = false;

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
        Monster_Boss enemy = boss.GetComponent<Monster_Boss>();
        fillHpBar.fillAmount = Mathf.Lerp(fillHpBar.fillAmount, (float)enemy.curHP / enemy.maxHP, Time.deltaTime * 5f);

        if(backHpBar)
        {
            fillHpBar2.fillAmount = Mathf.Lerp(fillHpBar2.fillAmount, fillHpBar.fillAmount, Time.deltaTime * 10f);
            // 두 체력바가 가까워 지면 액션 초기화를 해서 다음 히트에도 작동하도록 작성
            if(fillHpBar.fillAmount >= fillHpBar2.fillAmount - 0.01f)
            {
                backHpBar = false;
                fillHpBar2.fillAmount = fillHpBar.fillAmount;
            }
        }
    }
}
