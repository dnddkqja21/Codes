using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 공격 타입
    public enum Type
    {
        Fist,
        Melee,
        Range,
        Wand
    }

    public Type attackType;

    public int damage;  // 데미지

    public float attackRate;    // 속도

    public BoxCollider attackArea;  // 범위

    public TrailRenderer effect;    // 이펙트

    public Transform arrowPos;  // 생성위치

    public Transform arrowPos2;

    public Transform arrowPos3;

    public GameObject arrow;

    public GameObject arrow2;

    //public GameObject guideArrow;

    public Transform shottingPos;   // 발사 위치

    Player_PF player;

    public ParticleSystem bowEf;

    public ParticleSystem magicEf;

    ActionController sound;

    private void Start()
    {
        player = FindObjectOfType<Player_PF>();
        sound = FindObjectOfType<ActionController>();
    }
    
    public void Attack()
    {        
        if(attackType == Type.Melee)    // 밀리 타입이면 스윙
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if (attackType == Type.Range)
        {
            // 모션이 같은 원거리는 여기서 랜덤
            StartCoroutine("Shot");
        }
        else if(attackType == Type.Fist)
        {
            StopCoroutine("Fist");
            StartCoroutine("Fist");
        }
        else if (attackType == Type.Wand)
        {
            StartCoroutine("MagicArrow");
        }
    }

    IEnumerator MagicArrow()
    {
        int rand = Random.Range(0, 3);
        switch(rand)
        {
            case 0:
                yield return new WaitForSeconds(0.4f);
                //GameObject instantArrow = Instantiate(arrow, arrowPos.position, shottingPos.rotation);

                GameObject instantArrow = ObjectPool_PF.objectPoolInstance.CreateObject("MagicArrow");
                instantArrow.transform.position = arrowPos.position;
                instantArrow.transform.rotation = shottingPos.rotation;

                Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
                //arrowRigid.velocity = arrowPos.forward * 50f;
                arrowRigid.AddForce(arrowPos.forward * 50f, ForceMode.Impulse);
                player.curMP -= 7f;
                break;

            case 1:
                yield return new WaitForSeconds(0.4f);
                //GameObject instantArrow2 = Instantiate(arrow2, arrowPos.position, shottingPos.rotation);

                GameObject instantArrow2 = ObjectPool_PF.objectPoolInstance.CreateObject("MagicArrow5");
                instantArrow2.transform.position = arrowPos.position;
                instantArrow2.transform.rotation = shottingPos.rotation;

                Rigidbody arrowRigid2 = instantArrow2.GetComponent<Rigidbody>();
                //arrowRigid2.velocity = arrowPos.forward * 30f;
                arrowRigid2.AddForce(arrowPos.forward * 30f, ForceMode.Impulse);
                player.curMP -= 12f;
                break;

            case 2:
                magicEf.Play();
                yield return new WaitForSeconds(0.4f);
                sound.PlayClips(20);
                //GameObject instantArrow3 = Instantiate(arrow, arrowPos.position, shottingPos.rotation);

                GameObject instantArrow3 = ObjectPool_PF.objectPoolInstance.CreateObject("MagicArrow");
                instantArrow3.transform.position = arrowPos.position;
                instantArrow3.transform.rotation = shottingPos.rotation;

                Rigidbody arrowRigid3 = instantArrow3.GetComponent<Rigidbody>();
                //arrowRigid3.velocity = arrowPos.forward * 40f;
                arrowRigid3.AddForce(arrowPos.forward * 40f, ForceMode.Impulse);
                player.curMP -= 6f;

                yield return new WaitForSeconds(0.4f);
                sound.PlayClips(21);
                //GameObject instantArrow5 = Instantiate(arrow, arrowPos2.position, shottingPos.rotation);

                GameObject instantArrow4 = ObjectPool_PF.objectPoolInstance.CreateObject("MagicArrow");
                instantArrow4.transform.position = arrowPos.position;
                instantArrow4.transform.rotation = shottingPos.rotation;

                Rigidbody arrowRigid4 = instantArrow4.GetComponent<Rigidbody>();
                //arrowRigid4.velocity = arrowPos2.forward * 40f;
                arrowRigid4.AddForce(arrowPos2.forward * 40f, ForceMode.Impulse);
                player.curMP -= 6f;

                yield return new WaitForSeconds(0.4f);
                
                //GameObject instantArrow6 = Instantiate(arrow, arrowPos3.position, shottingPos.rotation);

                GameObject instantArrow5 = ObjectPool_PF.objectPoolInstance.CreateObject("MagicArrow");
                instantArrow5.transform.position = arrowPos.position;
                instantArrow5.transform.rotation = shottingPos.rotation;

                Rigidbody arrowRigid5 = instantArrow5.GetComponent<Rigidbody>();
                //arrowRigid5.velocity = arrowPos3.forward * 40f;
                arrowRigid5.AddForce(arrowPos3.forward * 40f, ForceMode.Impulse);
                player.curMP -= 7f;
                break;


        }        
    }

    IEnumerator Fist()
    {
        yield return new WaitForSeconds(0.15f);
        attackArea.enabled = true;        

        yield return new WaitForSeconds(0.35f);
        attackArea.enabled = false;        

        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.15f);
        attackArea.enabled = true;
        effect.enabled = true;
        player.curMP -= 5f;
        yield return new WaitForSeconds(0.1f);
        attackArea.enabled = false;
        

        yield return new WaitForSeconds(0.5f);
        effect.enabled = false;
    }

    IEnumerator Shot()  // 활 쏘는 모션 이후 화살이 나가야 함. 
    {
        int rand = Random.Range(0, 3);
        switch(rand)
        {
            case 0:
                yield return new WaitForSeconds(0.5f);
                //GameObject instantArrow = Instantiate(arrow, arrowPos.position, shottingPos.rotation);

                GameObject instantArrow = ObjectPool_PF.objectPoolInstance.CreateObject("WeaponArrow");
                
                instantArrow.transform.position = arrowPos.position;
                instantArrow.transform.rotation = shottingPos.rotation;

                Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
                //arrowRigid.velocity = arrowPos.forward * 50f;
                arrowRigid.AddForce(arrowPos.forward * 50f, ForceMode.Impulse);
                player.curMP -= 7f;
                break;

            case 1:
                bowEf.Play();
                yield return new WaitForSeconds(0.5f);
                player.curMP -= 20f;
                //GameObject instantArrow2 = Instantiate(arrow, arrowPos.position, shottingPos.rotation);

                GameObject instantArrow2 = ObjectPool_PF.objectPoolInstance.CreateObject("WeaponArrow2");
                
                instantArrow2.transform.position = arrowPos.position;
                instantArrow2.transform.rotation = shottingPos.rotation;
                Rigidbody arrowRigid2 = instantArrow2.GetComponent<Rigidbody>();
                //arrowRigid2.velocity = arrowPos.forward * 40f;
                arrowRigid2.AddForce(arrowPos.forward * 40f, ForceMode.Impulse);

                //GameObject instantArrow3 = Instantiate(arrow, arrowPos2.position, shottingPos.rotation);

                GameObject instantArrow3 = ObjectPool_PF.objectPoolInstance.CreateObject("WeaponArrow3");
                
                instantArrow3.transform.position = arrowPos.position;
                instantArrow3.transform.rotation = shottingPos.rotation;
                Rigidbody arrowRigid3 = instantArrow3.GetComponent<Rigidbody>();
                //arrowRigid3.velocity = arrowPos2.forward * 40f;
                arrowRigid3.AddForce(arrowPos2.forward * 40f, ForceMode.Impulse);
                //GameObject instantArrow4 = Instantiate(arrow, arrowPos3.position, shottingPos.rotation);

                GameObject instantArrow4 = ObjectPool_PF.objectPoolInstance.CreateObject("WeaponArrow4");
                
                instantArrow4.transform.position = arrowPos.position;
                instantArrow4.transform.rotation = shottingPos.rotation;
                Rigidbody arrowRigid4 = instantArrow4.GetComponent<Rigidbody>();
                //arrowRigid4.velocity = arrowPos3.forward * 40f;
                arrowRigid4.AddForce(arrowPos3.forward * 40f, ForceMode.Impulse);
                break;

            case 2:
                yield return new WaitForSeconds(0.5f);
                sound.PlayClips(18);
                GameObject instantArrow5 = ObjectPool_PF.objectPoolInstance.CreateObject("WeaponArrow");

                instantArrow5.transform.position = arrowPos.position;
                instantArrow5.transform.rotation = shottingPos.rotation;

                Rigidbody arrowRigid5 = instantArrow5.GetComponent<Rigidbody>();
                //arrowRigid.velocity = arrowPos.forward * 50f;
                arrowRigid5.AddForce(arrowPos.forward * 50f, ForceMode.Impulse);
                player.curMP -= 7f;

                yield return new WaitForSeconds(0.5f);
                GameObject instantArrow6 = ObjectPool_PF.objectPoolInstance.CreateObject("WeaponArrow");

                instantArrow6.transform.position = arrowPos.position;
                instantArrow6.transform.rotation = shottingPos.rotation;

                Rigidbody arrowRigid6 = instantArrow6.GetComponent<Rigidbody>();
                //arrowRigid.velocity = arrowPos.forward * 50f;
                arrowRigid6.AddForce(arrowPos.forward * 50f, ForceMode.Impulse);
                player.curMP -= 7f;
                break;
        }
    }
}
