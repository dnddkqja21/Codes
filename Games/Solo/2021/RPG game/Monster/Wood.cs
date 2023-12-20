using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    public int maxHP;
    public int curHP;

    Material mat;
    
    Vector3 offset = new Vector3(0, 10f, 0);

    Vector3 createPos;

    bool isDroped = false;

    ActionController player;

    public int exp;

    LevelUp level;

    void Start()
    {        
        mat = GetComponentInChildren<MeshRenderer>().material;
        player = FindObjectOfType<ActionController>();
        level = FindObjectOfType<LevelUp>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {  
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHP -= weapon.damage;            

            StartCoroutine("OnDamage");
        }
        else if (other.tag == "Arrow")
        {
            Arrow arrow = other.GetComponent<Arrow>();
            curHP -= arrow.damage;
            ObjectPool_PF.objectPoolInstance.AddPoolObject(other.gameObject);
            //Destroy(other.gameObject);
            StartCoroutine("OnDamage");
        }
        else if (other.tag == "MagicArrow")
        {
            MagicArrow MagicArrow = other.GetComponent<MagicArrow>();
            curHP -= MagicArrow.damage;
            ObjectPool_PF.objectPoolInstance.AddPoolObject(other.gameObject);
            //Destroy(other.gameObject);
            StartCoroutine("OnDamage");
        }
    }

    IEnumerator OnDamage()
    {
        yield return new WaitForSeconds(0.1f);

        if (curHP > 0)
        {
            player.PlayClips(0);
            
            mat.color = Color.white;
        }
        else
        {
            mat.color = Color.gray;

            //GameObject tmp = Resources.Load<GameObject>("ObjectPool/QuestWood");                 

            int layerMask = 1 << LayerMask.NameToLayer("Monster");
            RaycastHit hitInfo;
            if(Physics.Raycast(transform.position + offset, Vector3.down, out hitInfo, Mathf.Infinity, ~layerMask))
            {
                createPos = hitInfo.point;
            }
            if(isDroped == false)
            {
                GameObject tmp = ObjectPool_PF.objectPoolInstance.CreateObject("QuestWood");
                tmp.transform.position = createPos;
                //Instantiate<GameObject>(tmp, createPos, Quaternion.identity);
            }
            isDroped = true;

            level.GetExp(exp);

            Destroy(gameObject, 1f);
        }
    }
}
