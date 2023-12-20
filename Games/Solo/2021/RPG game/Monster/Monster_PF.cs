using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster_PF : MonoBehaviour
{  
    public enum Type
    {
        Low,
        Normal,
        Charge,
        Range
    }
    public Type monsterType;

    ObjectPool_PF pooling = null;

    public ObjectPool_PF POOLING   // �ǽð����� �߰��Ǵ� ��ũ��Ʈ�� �˰��ϱ� ���� ������Ƽ
    {
        get { return pooling; }
        set { pooling = value; }
    }

    public int maxHP;
    public int curHP;

    Rigidbody rigid;

    public BoxCollider attackArea;

    SkinnedMeshRenderer[] mat;

    public Transform target = null;

    NavMeshAgent nav;

    RaycastHit hitInfo;

    float senseRange;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    GameObject dropItem1;

    [SerializeField]
    GameObject dropItem2;

    [SerializeField]
    GameObject dropItem3;

    [SerializeField]
    GameObject dropItem4;

    Vector3 offset = new Vector3(0, 10f, 0);

    Vector3 createPos;

    Vector3 originPos;

    Animator ani;

    bool isFollow;

    bool isDroped = false;

    bool isAttack;

    public float attackRate;
    float attackDelay;
    bool isAttackReady;

    public GameObject dmgText;
    public Transform dmgTextPos;

    float knockBack = 1f;

    public GameObject senserMark;
    public Transform markPos;
    bool isMarked = false;

    bool isCharged = false;

    BoxCollider box;

    public GameObject magic;
    public Transform magicPos;
    public Transform magicShootingPos;

    float setFollowDis;
    float setTurnAwayDis;

    public GameObject hpBar;

    ActionController player;

    Player_PF playerAttack;

    public ParticleSystem hitAttack;
    public ParticleSystem hitArrow;
    public ParticleSystem hitMagic;

    public int exp;

    LevelUp level;

    public Texture2D cursorNormal;
    public Texture2D cursorMonster;

    string itemName = "";    

    void Start()
    {   
        rigid = GetComponent<Rigidbody>();
        box = GetComponent<BoxCollider>();
        mat = GetComponentsInChildren<SkinnedMeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        originPos = transform.position;
        player = FindObjectOfType<ActionController>();
        playerAttack = FindObjectOfType<Player_PF>();
        level = FindObjectOfType<LevelUp>();

    }
    void Update()
    {
        Sense();
        Follow();
        ExitTarget();
        ExitFollow();
        SetIdle();
        AttackDelay();
        LookTarget();
        ActivateHpBar();
        RayCast();
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

            transform.LookAt(other.transform);
            
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

            transform.LookAt(other.transform);

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

            transform.LookAt(other.transform);

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

    void RayCast()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Monster");
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + offset, Vector3.down, out hitInfo, Mathf.Infinity, ~layerMask))
        {
            createPos = hitInfo.point;  // ���� ������ �Ʒ��� ���̸� �R�Ƽ� �� ��ġ�� ������ ���� ����
        }
    }    

    IEnumerator OnDamage()
    {
        foreach(SkinnedMeshRenderer mesh in mat)
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
        }
        else
        {
            foreach (SkinnedMeshRenderer mesh in mat)
            {
                mesh.material.color = Color.gray;
            }

            target = null;
            
            /*
            int layerMask = 1 << LayerMask.NameToLayer("Monster");
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position + offset, Vector3.down, out hitInfo, Mathf.Infinity, ~layerMask))
            {
                createPos = hitInfo.point;  // ���� ������ �Ʒ��� ���̸� �R�Ƽ� �� ��ġ�� ������ ���� ����
            }
            */
            if(isDroped == false)
            {   // ���� �Ŀ��� ��� ��Ʈ�� �Ǿ� �������� �����Ǵ� ���� ���� + �׾��� �� �� ���� ����Ǿ�� �ϴ� �͵�.
                nav.Move(-transform.forward * knockBack);

                StartCoroutine(SetAlpha());
                //Instantiate<GameObject>(dropItem, createPos, Quaternion.identity);

                int rand = Random.Range(0, 5);

                switch(rand)
                {       
                    case 0:
                        /*
                        GameObject tmp2 = Instantiate(dropItem2);
                        tmp2.transform.position = createPos;
                        */
                        int random = Random.Range(0, 2);
                        switch(random)
                        {
                            case 0:
                                itemName = "PotionHP";
                                break;

                            case 1:
                                itemName = "PotionMP";
                                break;
                        }
                        GameObject tmpItem2 = ObjectPool_PF.objectPoolInstance.CreateObject(itemName);

                        tmpItem2.transform.position = createPos;
                        
                        break;

                    case 1:
                        /*
                        GameObject tmp3 = Instantiate(dropItem3);
                        tmp3.transform.position = createPos;
                        */
                        switch (monsterType)
                        {
                            case Type.Low:
                                itemName = "CoinCopper";
                                break;

                            case Type.Normal:
                                itemName = "CoinSilver";
                                break;

                            case Type.Charge:
                                itemName = "CoinGold";
                                break;

                            case Type.Range:
                                itemName = "CoinGold";
                                break;
                        }

                        GameObject tmpItem3 = ObjectPool_PF.objectPoolInstance.CreateObject(itemName);

                        tmpItem3.transform.position = createPos;
                        
                        break;

                    case 2:
                        /*
                        GameObject tmp4 = Instantiate(dropItem4);
                        tmp4.transform.position = createPos;                        
                        break;
                        */

                    case 3: 
                        /*
                        GameObject tmp7 = Instantiate(dropItem1);
                        tmp7.transform.position = createPos;
                        break;
                        */

                    case 4:
                        /*
                        GameObject tmp = Instantiate(dropItem1);
                        tmp.transform.position = createPos;
                        */


                        switch (monsterType)
                        {
                            case Type.Low:
                                itemName = "QuestShell";
                                break;

                            case Type.Normal:
                                itemName = "QuestBone";
                                break;

                            case Type.Charge:
                                itemName = "QuestIron";
                                break;

                            case Type.Range:
                                itemName = "QuestScroll";
                                break;
                        }

                        GameObject tmpItem = ObjectPool_PF.objectPoolInstance.CreateObject(itemName);

                        tmpItem.transform.position = createPos;                        

                        break;
                }                
            }
            isDroped = true;
            isFollow = false;

            nav.enabled = false;
            box.enabled = false;

            level.GetExp(exp);

            int r = Random.Range(5, 8);
            player.PlayClips(r);
            RemoveMonster();
            ani.SetTrigger("toDie");
            
            Destroy(gameObject, 4f);            
        }
    }
    

    private void OnEnable() // Ȱ�� �� �۵��ؾ� �ϹǷ� ���ο��̺� �Լ��� �ۼ�
    {
        //Invoke("RemoveMonster", 5f);    // 5�� �� �Լ� ���� �ض�.
    }
    
    void LookTarget()
    {
        if(target != null)
        {
            transform.LookAt(target);
        }
    }

    void RemoveMonster()
    {
        //pooling.AddPoolList(this);  // �� ��ũ��Ʈ�� ������ �ִ� �ڽ�
        switch(monsterType)
        {
            case Type.Low:
                RespawnManager.instanceRespawn.turtleCount.Remove(this.gameObject);
                RespawnManager.instanceRespawn.isSpawnTurtle[int.Parse(transform.parent.name) - 1] = false;
                break;

            case Type.Normal:
                RespawnManager.instanceRespawn.skelCount.Remove(this.gameObject);
                RespawnManager.instanceRespawn.isSpawnSkel[int.Parse(transform.parent.name) - 1] = false;
                break;

            case Type.Charge:
                RespawnManager.instanceRespawn.orcCount.Remove(this.gameObject);
                RespawnManager.instanceRespawn.isSpawnOrc[int.Parse(transform.parent.name) - 1] = false;
                break;

            case Type.Range:
                RespawnManager.instanceRespawn.mageCount.Remove(this.gameObject);
                RespawnManager.instanceRespawn.isSpawnMage[int.Parse(transform.parent.name) - 1] = false;
                break;

        }       
    }

    void Sense()
    {
        switch(monsterType) // ���� Ÿ�Կ� ���� ���� �Ÿ� 
        {
            case Type.Low:
                senseRange = 4f;
                break;

            case Type.Normal:
                senseRange = 5f;
                break;

            case Type.Charge:
                senseRange = 7f;
                break;

            case Type.Range:
                senseRange = 11f;
                break;
        }

        if (curHP > 0)
        {
            if (Physics.SphereCast(transform.position, transform.lossyScale.x, transform.forward, out hitInfo, senseRange, layerMask))
            {   // ���̸� ��Ƽ� ���� �Ÿ� �ȿ� ������ Ÿ������ �ν��� �� ����.
                if (hitInfo.transform.tag == "Player")
                {
                    if(isMarked == false)
                    {
                        GameObject tmp = Instantiate(senserMark);
                        tmp.transform.position = markPos.position;
                        Destroy(tmp.gameObject, 0.5f);
                        isMarked = true;
                    }                    

                    target = hitInfo.transform;

                    // ���� Ÿ�Կ� ���� �ٸ� ���� ����
                    switch(monsterType)
                    {
                        case Type.Low:
                            SetFollow();
                            break;

                        case Type.Normal:
                            SetFollow();
                            break;

                        case Type.Charge:
                            if(isCharged == false)
                            {
                                Charge();
                            }                            
                            break;

                        case Type.Range:
                            SetFollow();
                            break;                            
                    }                    
                }
            }
        }
    }

    void Charge()
    {
        rigid.AddForce(transform.forward * 100f, ForceMode.Impulse);
        ani.SetTrigger("toCharge");
        isFollow = true;
        isCharged = true;
    }

    void SetFollow()
    {
        isFollow = true;        
    }

    void Follow()
    {
        if(target != null && isFollow)
        {
            ani.SetBool("isWalk", true);
            nav.SetDestination(target.position);
        }
    }

    void ExitFollow()
    {
        if(monsterType == Type.Range)
        {
            setFollowDis = 8.5f;
        }
        else
        {
            setFollowDis = 2.5f;
        }       

        if (target != null)
        {   // �÷��̾��� �ձ��� ���� ������� �� ����
            if(Vector3.Distance(target.position, transform.position) < setFollowDis)
            {
                nav.isStopped = true;
                ani.SetBool("isWalk", false);
                isAttack = true;
                
                if (isAttack)
                {
                    if (isAttackReady && curHP > 0)
                    {
                        ani.SetTrigger("toAttack");
                        StartCoroutine(Attack());
                        attackDelay = 0;
                    }                    
                }                
            }
            else
            {   // �Ÿ��� �ٽ� �������� �ٽ� ������
                nav.isStopped = false;
                isAttack = false;                
            }
        }
    }

    void ExitTarget()
    {
        switch (monsterType)
        {
            case Type.Low:
                setTurnAwayDis = 7f;
                break;

            case Type.Normal:
                setTurnAwayDis = 8f;
                break;

            case Type.Charge:
                setTurnAwayDis = 9f;
                break;

            case Type.Range:
                setTurnAwayDis = 10f;
                break;
        }
        if (target != null)
        {   // �Ÿ��� �־����� Ÿ���� ������鼭 �� ��ġ�� ���ư�
            if (Vector3.Distance(target.position, transform.position) > setTurnAwayDis)
            {
                target = null;
                nav.SetDestination(originPos);
                isMarked = false;
                isCharged = false;
            }
        }        
    }

    void SetIdle()  // ����ġ�� ���ư��� �� ���̵�� �����ϱ� ����.
    {
        // �׺�޽��� �µ���Ƽ���̼��� �� �� �������� ���� ���� �ƴ϶� ���� ���� ���̹Ƿ�
        // transform.position == originPos �� �ذ� �Ұ���.
        if(Vector3.Distance(originPos, transform.position) < 1f)
        {
            ani.SetBool("isWalk", false);
        }        
    }

    IEnumerator Attack()
    {   
        if(monsterType == Type.Range)
        {
            yield return new WaitForSeconds(0.5f);
            
            GameObject instantMagic = Instantiate(magic, magicPos.position, magicShootingPos.rotation);
            
            Rigidbody magicRigid = instantMagic.GetComponent<Rigidbody>();
            //magicRigid.velocity = magicPos.forward * 15f;
            magicRigid.velocity = magicShootingPos.forward * 15f;

            //Debug.DrawLine(transform.position, magicPos.forward, Color.red, Mathf.Infinity);
            //magicRigid.AddForce(target.transform.forward * 10f, ForceMode.Acceleration);

            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(0.6f);
        
            attackArea.enabled = true;

            yield return new WaitForSeconds(0.3f);

            attackArea.enabled = false;                
        }
    }

    void AttackDelay() 
    {
        attackDelay += Time.deltaTime;
        isAttackReady = attackRate < attackDelay;
    }   
    
    IEnumerator SetAlpha()  // �׾��� �� Ŀ���� ���� ���� ���̴��� �̿��� ���� ������������ ����.
    {        
        yield return new WaitForSeconds(1f);
        
        while(true)
        {            
            foreach (SkinnedMeshRenderer mesh in mat)
            {
                Color color = mesh.material.color;
                                
                color.a -= Time.deltaTime;
                mesh.material.color = color;
            }
            yield return new WaitForSeconds(0.02f);            
        }        
    }

    void ActivateHpBar()
    {
        if(target != null)
        {
            hpBar.SetActive(true);
        }
        else
        {
            hpBar.SetActive(false);
        }
    }
}

