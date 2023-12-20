using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestEnemy : MonoBehaviour
{

    public int maxHP;
    public int curHP;

    Rigidbody rigid;
    BoxCollider col;
    Material mat;

    Transform DamageCanvas;
    Transform damagePos;

    public TextMeshProUGUI damage;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        mat = GetComponent<MeshRenderer>().material;

        DamageCanvas = GetDamageText();
        damagePos = GetDamage(transform, "DAMAGE");

        Vector3 tmp = Camera.main.WorldToScreenPoint(damagePos.position);
        DamageCanvas.transform.position = tmp;
                
    }

    private void Update()
    {
        Vector3 tmp = Camera.main.WorldToScreenPoint(damagePos.position);
        DamageCanvas.transform.position = tmp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHP -= weapon.damage;
            
            damage.gameObject.SetActive(true);
            damage.text = weapon.damage.ToString();
            
                StartCoroutine("OnDamage");
        }
        else if(other.tag == "Arrow")
        {
            Arrow arrow = other.GetComponent<Arrow>();
            curHP -= arrow.damage;
            Destroy(other.gameObject);
            StartCoroutine("OnDamage");
        }
        else if (other.tag == "MagicArrow")
        {
            MagicArrow MagicArrow = other.GetComponent<MagicArrow>();
            curHP -= MagicArrow.damage;
            Destroy(other.gameObject);
            StartCoroutine("OnDamage");
        }
    }

    IEnumerator OnDamage()
    {        
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(curHP > 0)
        {
            mat.color = Color.white;
        }
        else
        {
            mat.color = Color.gray;
            Destroy(gameObject, 2f);
        }
    }

    public Transform GetDamage(Transform parentTr, string _hpTr)
    {
        Transform findTr = null;    // ó���� �� �ʱ�ȭ
        for (int i = 0; i < parentTr.childCount; i++)
        {
            findTr = parentTr.GetChild(i);
            if (findTr.name.Equals(_hpTr))
            {
                return findTr;  // ã������ ã�� �ڽ��� ����
            }
        }
        return null;    // �ƴϸ� �θ���
    }

    public Transform GetDamageText()
    {
        GameObject obj = GameObject.Find("Canvas/Damage/dmgText");
        return obj.transform;
    }
}
