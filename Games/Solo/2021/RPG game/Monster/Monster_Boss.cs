using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster_Boss : MonoBehaviour
{
    public Transform target = null;

    public BoxCollider attackArea;

    public BoxCollider breath;

    public GameObject fireBall;

    public Transform fireBallPos;

    public GameObject dmgText;

    public Transform dmgTextPos;

    public GameObject hpBar;

    public GameObject dropItem;

    public bossHp hpEffect;

    public GameObject warningLine;

    public GameObject stone;

    public SphereCollider doom;    

    public Image warningImage;

    public GameObject breathEf;

    public ParticleSystem DoomEf;

    Animator ani;

    NavMeshAgent nav;

    SkinnedMeshRenderer[] mat;

    BoxCollider box;

    Coroutine coroutineThink;

    Warning_Line warning;       

    Vector3 offset = new Vector3(0, 10f, 0);

    Vector3 createPos;    

    RaycastHit hitInfo;

    [SerializeField]
    LayerMask layerMask;

    public int maxHP;

    public int curHP;

    float senseRange = 12f;

    bool isFollow;

    bool isDroped = false;    

    bool isThinking = false;

    ActionController player;

    Player_PF playerAttack;

    public ParticleSystem hitAttack;
    public ParticleSystem hitArrow;
    public ParticleSystem hitMagic;

    public Texture2D cursorNormal;
    public Texture2D cursorMonster;

    public CameraShaking cameraShaking;

    void Start()
    {
        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        mat = GetComponentsInChildren<SkinnedMeshRenderer>();
        box = GetComponent<BoxCollider>();
        warning = warningLine.GetComponent<Warning_Line>();
        player = FindObjectOfType<ActionController>();
        playerAttack = FindObjectOfType<Player_PF>();
        //StartCoroutine(Think());        
    }


    void Update()
    {
        // ������ ������ �ϴ��� ������ ������ ����.
        Sense();
        ThinkAI();
        LookTarget();
        ActivateHpBar();
        SetWarningLinePos();
        //Follow();
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(cursorMonster, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            hitAttack.Play();

            Weapon weapon = other.GetComponent<Weapon>();
            curHP -= weapon.damage + playerAttack.attack;

            GameObject tmp = Instantiate(dmgText);
            tmp.transform.position = dmgTextPos.position;
            tmp.GetComponent<DamageText>().damage = weapon.damage + playerAttack.attack;

            StartCoroutine("OnDamage");
        }
        else if (other.tag == "Arrow")
        {
            hitArrow.Play();

            Arrow arrow = other.GetComponent<Arrow>();
            curHP -= arrow.damage + playerAttack.attack;

            GameObject tmp = Instantiate(dmgText);
            tmp.transform.position = dmgTextPos.position;
            tmp.GetComponent<DamageText>().damage = arrow.damage + playerAttack.attack;

            //Destroy(other.gameObject);
            ObjectPool_PF.objectPoolInstance.AddPoolObject(other.gameObject);
            StartCoroutine("OnDamage");
        }
        else if (other.tag == "MagicArrow")
        {
            hitMagic.Play();

            MagicArrow MagicArrow = other.GetComponent<MagicArrow>();
            curHP -= MagicArrow.damage + playerAttack.attack;

            GameObject tmp = Instantiate(dmgText);
            tmp.transform.position = dmgTextPos.position;
            tmp.GetComponent<DamageText>().damage = MagicArrow.damage + playerAttack.attack;

            //Destroy(other.gameObject);
            ObjectPool_PF.objectPoolInstance.AddPoolObject(other.gameObject);
            StartCoroutine("OnDamage");
        }
    }

    IEnumerator OnDamage()
    {
        foreach (SkinnedMeshRenderer mesh in mat)
        {
            mesh.material.color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        if (curHP > 0)
        {
            player.PlayClips(0);

            foreach (SkinnedMeshRenderer mesh in mat)
            {
                mesh.material.color = Color.white;
            }
            // 0.5�� �� back hp�� �׼� ȣ��
            Invoke("BackHpEffectOn", 0.3f);
        }
        else
        {
            foreach (SkinnedMeshRenderer mesh in mat)
            {
                mesh.material.color = Color.gray;
            }

            target = null;

            int layerMask = 1 << LayerMask.NameToLayer("Monster");
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + offset, Vector3.down, out hitInfo, Mathf.Infinity, ~layerMask))
            {
                createPos = hitInfo.point;  // ���� ������ �Ʒ��� ���̸� �R�Ƽ� �� ��ġ�� ������ ���� ����
            }
            if (isDroped == false)
            {   // ���� �Ŀ��� ��� ��Ʈ�� �Ǿ� �������� �����Ǵ� ���� ���� + �׾��� �� �� ���� ����Ǿ�� �ϴ� �͵�.
                
                //StartCoroutine(SetAlpha());
                //Instantiate<GameObject>(dropItem, createPos, Quaternion.identity);
                GameObject tmp = Instantiate(dropItem);
                tmp.transform.position = createPos;
            }
            isDroped = true;
            //isFollow = false;

            nav.enabled = false;
            box.enabled = false;

            player.PlayClips(13);

            ani.SetTrigger("toDie");

            //StopCoroutine(coroutineThink);
            StopAllCoroutines();

            Destroy(gameObject, 4.5f);
        }
    }
    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.5f);  // 0.5�ʰ� ���� �� ����, �� �ð��� ����� ���� ���Ͽ� ��ó�� ������

        int randomAction = Random.Range(0, 5);  // 0 ~ 4�� ���� �ൿ

        switch(randomAction)    // ����, �극��, ���̾, �ĸ� 4������ �ൿ
        {
            case 0:                
            case 1: 
                StartCoroutine(Attack());
                break;
                // ������ (0,1) 2/5 �� Ȯ��
                // ������ ��ų ������ 1/5 �� Ȯ���� ����
            case 2:
                StartCoroutine(Breath());
                break;
            case 3:
                StartCoroutine(Fireball());
                break;
            case 4:
                StartCoroutine(Doom());
                break;

        }

    }

    IEnumerator Attack()
    {        
        ani.SetTrigger("toAttack");
        Debug.Log("�Ϲ� ����");

        player.PlayClips(11);

        yield return new WaitForSeconds(0.8f);

        attackArea.enabled = true;

        yield return new WaitForSeconds(0.3f);

        attackArea.enabled = false;

        yield return new WaitForSeconds(1f);    // �׼� �� 3�� �� �ٽ� ���ο� ���� �׼��� ����
        StartCoroutine(Think());
    }

    IEnumerator Breath()
    {
        ani.SetTrigger("toBreath");
        Debug.Log("�극��");
        warningImage.enabled = true;
        yield return new WaitForSeconds(1.5f);

        player.PlayClips(12);
        breathEf.SetActive(true);
        
        breath.enabled = true;
        
        yield return new WaitForSeconds(0.3f);
        cameraShaking.ShakeCam();  
       
        yield return new WaitForSeconds(0.9f);
        cameraShaking.ResetCam();
        
        breath.enabled = false;
        breathEf.SetActive(false);               

        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }

    IEnumerator Fireball()
    {
        ani.SetTrigger("toFireball");
        Debug.Log("���̾");

        yield return new WaitForSeconds(0.3f);             

        GameObject tmp = Instantiate(fireBall);
        tmp.transform.position = fireBallPos.position;

        GameObject tmp2 = Instantiate(warningLine);
        tmp2.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        
        yield return new WaitForSeconds(1.7f);

        player.PlayClips(12);

        Rigidbody rigid = tmp.GetComponent<Rigidbody>();
        rigid.velocity = fireBallPos.forward * 5f;

        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }

    IEnumerator Doom()
    {
        ani.SetTrigger("toDoomReady");
        Debug.Log("�ĸ�");
        cameraShaking.ShakeCam();

        yield return new WaitForSeconds(0.5f);
        GameObject tmp = Instantiate(stone);
        tmp.transform.position = transform.position + new Vector3(Random.Range(-15,15), Random.Range(10,15), Random.Range(-20, 15));

        yield return new WaitForSeconds(0.5f);
        GameObject tmp1 = Instantiate(stone);
        tmp1.transform.position = transform.position + new Vector3(Random.Range(-15, 15), Random.Range(10, 15), Random.Range(-20, 15));

        yield return new WaitForSeconds(0.5f);
        cameraShaking.ResetCam();
        GameObject tmp2 = Instantiate(stone);
        tmp2.transform.position = transform.position + new Vector3(Random.Range(-15, 15), Random.Range(10, 15), Random.Range(-20, 15));

        yield return new WaitForSeconds(0.5f);
        GameObject tmp3 = Instantiate(stone);
        tmp3.transform.position = transform.position + new Vector3(Random.Range(-15, 15), Random.Range(10, 15), Random.Range(-20, 15));

        yield return new WaitForSeconds(0.5f);
        GameObject tmp4 = Instantiate(stone);
        tmp4.transform.position = transform.position + new Vector3(Random.Range(-15, 15), Random.Range(10, 15), Random.Range(-20, 15));

        yield return new WaitForSeconds(2f);
        ani.SetTrigger("toDoom");

        yield return new WaitForSeconds(3f);
        player.PlayClips(15);        

        yield return new WaitForSeconds(1f);
        cameraShaking.ShakeCam();
        doom.enabled = true;
        DoomEf.Play();
        player.PlayClips(14);

        yield return new WaitForSeconds(0.3f);
        cameraShaking.ResetCam();
        doom.enabled = false;

        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }

    void Sense()
    {
        if (curHP > 0)
        {
            if (Physics.SphereCast(transform.position , transform.lossyScale.x, transform.forward, out hitInfo, senseRange, layerMask))
            {   // ���̸� ��Ƽ� ���� �Ÿ� �ȿ� ������ Ÿ������ �ν��� �� ����.
                if (hitInfo.transform.tag == "Player")
                {
                    target = hitInfo.transform;
                }
                //SetFollow();
            }
        }
    }

    void SetFollow()
    {
        isFollow = true;
    }

    void Follow()
    {
        if (target != null && isFollow)
        {
            ani.SetBool("isMove", true);
            nav.SetDestination(target.position);
        }
    }

    void LookTarget()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }

    void ThinkAI()
    {
        if (target != null && isThinking == false)
        {
            coroutineThink = StartCoroutine(Think());
            isThinking = true;
        }
    }

    void ActivateHpBar()
    {
        if (target != null)
        {
            hpBar.SetActive(true);
        }
        else
        {
            hpBar.SetActive(false);
        }
    }

    IEnumerator SetAlpha()  // �׾��� �� Ŀ���� ���� ���� ���̴��� �̿��� ���� ������������ ����.
    {
        yield return new WaitForSeconds(1.5f);

        while (true)
        {
            foreach (SkinnedMeshRenderer mesh in mat)
            {
                Color color = mesh.material.color;

                color.a -= Time.deltaTime;
                mesh.material.color = color;
            }
            yield return new WaitForSeconds(0.03f);
        }
    }

    void BackHpEffectOn()
    {
        hpEffect.backHpBar = true;
    }

    void SetWarningLinePos()
    {
        if(target != null)
        {
            warning.targetPos = target.position;
        }
    }
}
