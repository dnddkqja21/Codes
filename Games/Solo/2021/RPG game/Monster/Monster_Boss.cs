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
        // 보스의 설정을 일단은 고정형 보스로 설정.
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
            // 0.5초 후 back hp바 액션 호출
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
                createPos = hitInfo.point;  // 몬스터 위에서 아래로 레이를 쏳아서 그 위치에 아이템 생성 위함
            }
            if (isDroped == false)
            {   // 죽은 후에도 계속 히트가 되어 아이템이 생성되는 것을 방지 + 죽었을 시 한 번만 실행되어야 하는 것들.
                
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
        yield return new WaitForSeconds(0.5f);  // 0.5초간 생각 후 실행, 이 시간이 길어질 수록 패턴에 대처가 쉬워짐

        int randomAction = Random.Range(0, 5);  // 0 ~ 4의 랜덤 행동

        switch(randomAction)    // 어택, 브레스, 파이어볼, 파멸 4가지의 행동
        {
            case 0:                
            case 1: 
                StartCoroutine(Attack());
                break;
                // 어택은 (0,1) 2/5 의 확률
                // 나머지 스킬 공격은 1/5 의 확률로 설정
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
        Debug.Log("일반 공격");

        player.PlayClips(11);

        yield return new WaitForSeconds(0.8f);

        attackArea.enabled = true;

        yield return new WaitForSeconds(0.3f);

        attackArea.enabled = false;

        yield return new WaitForSeconds(1f);    // 액션 후 3초 뒤 다시 새로운 랜덤 액션을 생각
        StartCoroutine(Think());
    }

    IEnumerator Breath()
    {
        ani.SetTrigger("toBreath");
        Debug.Log("브레스");
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
        Debug.Log("파이어볼");

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
        Debug.Log("파멸");
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
            {   // 레이를 쏘아서 일정 거리 안에 들어오면 타겟으로 인식한 후 따라감.
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

    IEnumerator SetAlpha()  // 죽었을 시 커스텀 알파 블렌딩 셰이더를 이용해 점점 투명해지도록 설정.
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
