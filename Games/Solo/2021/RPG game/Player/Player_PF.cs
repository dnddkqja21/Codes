using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class Player_PF : MonoBehaviour
{
    public float maxHP;
    public float curHP;

    public float maxMP;
    public float curMP;

    public int gold = 300;

    public int attack = 1;

    public int defense = 1;

    public Joystick joystick;

    float moveSpeed = 4f;

    float runSpeed = 7f;

    float setSpeed;

    // float turnSpeed = 300f;

    public Animator ani;

    bool wakeUp = false;

    bool isSleep = true;

    bool isDamaged = false;

    float recoverySpeed = 6f;

    Coroutine coroutineHp;

    Coroutine coroutineZZZ;

    public TextMeshPro zzZ;

    public TextMeshProUGUI HpState;

    public Image hpBar;

    public TextMeshProUGUI MpState;

    public Image MpBar;    

    void Start()
    {
        ani.SetTrigger("toSleep");
        setSpeed = moveSpeed;
        coroutineZZZ = StartCoroutine(ZZZ());        
    }

    void Update()
    {
        //TryRun();
        //MovePlayer();
        //TurnPlayer();
        //Rolling();
        HealthUp();
        Sleep();
        WakeUp();
        HpStateUpdate();
        HpBarUpdate();
        SetHP();
        MpStateUpdate();
        MpBarUpdate();
        SetMP();
        MpRecovery();
        LookTarget();        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MonstersAttack")
        {
            if(!isDamaged)
            {
                MonstersAttack monsterAttack = other.GetComponent<MonstersAttack>();
                curHP -= monsterAttack.damage - defense;
                StartCoroutine(OnDamage()); // ���� Ÿ�� 
            }
        }
        else if (other.tag == "MonstersMagic")
        {
            if (!isDamaged)
            {
                MonstersMagic monsterAttack = other.GetComponent<MonstersMagic>();
                curHP -= monsterAttack.damage - defense;
                StartCoroutine(OnDamage()); // ���� Ÿ�� 
            }
            Destroy(other.gameObject);
        }

        if (other.tag == "InvincibleArea")
        {
            this.gameObject.layer = 11;
            Invoke("NonInvincible", 5f);
        }
    }

    
    private void OnTriggerStay(Collider other)  // ���� ������ �ӹ��� ���ȿ��� ����
    {
        if (other.tag == "InvincibleArea")
        {
            //this.gameObject.layer = 11;
        }        
    }

    private void OnTriggerExit(Collider other)  // ����� �ٽ� ������ ���� �� �ֵ�����. �� ������ ���ٸ� ĳ���ʹ� ��� ������.
    {
        if(other.tag == "InvincibleArea")
        {
            gameObject.layer = 3;
        }
    }
    
    void LookTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.tag == "Monster")
                {
                    transform.LookAt(hit.transform);
                }                
            }
        }
    }

    IEnumerator OnDamage()
    {
        isDamaged = true;
        ani.SetTrigger("toDamaged");
        yield return new WaitForSeconds(1f);

        isDamaged = false;
    }

    private void MovePlayer()
    {
        Vector3 tmp = transform.position;
        tmp.x += joystick.STICKDIR.x * Time.deltaTime * setSpeed;
        tmp.z += joystick.STICKDIR.y * Time.deltaTime * setSpeed;

        transform.position = tmp;        
    }

    void TurnPlayer()
    {
        // ���̽�ƽ�� ������ �÷��̾� ȸ���� ����
        if (Input.GetMouseButton(0) && joystick.ISCLICK == true) // ���̽�ƽ�� ���� ���Ͱ� ���콺�� ������ �� ���ΰ� �Ǳ� ������ �÷��̾��� ������ 0,0,0�� �Ǵ� �� ����
        {
            transform.eulerAngles = new Vector3(0, Mathf.Atan2(joystick.STICKDIR.x, joystick.STICKDIR.y) * Mathf.Rad2Deg, 0);
        }
    }

    void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Running();
            ani.SetBool("isRun", true);
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            CancelRunning();
            ani.SetBool("isRun", false);
        }
    }

    void Running()
    {
        //isRun = true;
        setSpeed = runSpeed;
    }

    void CancelRunning()
    {
        //isRun = false;
        setSpeed = moveSpeed;
    }

    void Rolling()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            ani.SetTrigger("toRoll");
        }
    }

    void HealthUp()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ani.SetTrigger("toVictory");

            StartCoroutine(HpUp());
        }
        // ��ų ���ӽð� ���� ��Ÿ���� ª���� �ִ� ü���� ��� �þ�� ���� �߻���.
    }

    IEnumerator HpUp()
    {
        float tmpHp = maxHP;

        maxHP += (maxHP / 5f);
        
        yield return new WaitForSeconds(60f);

        maxHP = tmpHp;
    }

    void Sleep()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            wakeUp = false;
            isSleep = true;
            ani.SetTrigger("toSleep");

            StopCoroutine(coroutineZZZ);
            coroutineZZZ = StartCoroutine(ZZZ());
            coroutineHp = StartCoroutine(HpRecovery());     // hp�� float�� �ƴϱ� ������ ��û���� ������ ȸ���ǰ� ����.       
        }
    }

    IEnumerator HpRecovery()
    {
        yield return new WaitForSeconds(1f);

        while(curHP <= maxHP)
        {
            curHP += recoverySpeed * Time.deltaTime;
            if (curHP > maxHP)
            {
                curHP = maxHP;
            }
            yield return null;            
        }
    }

    void WakeUp()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            wakeUp = true;
        }
        if (wakeUp && isSleep == true)
        {
            ani.SetTrigger("WakeUp");
            isSleep = false;
            zzZ.gameObject.SetActive(false);
            if (coroutineHp != null)
            {
                StopCoroutine(coroutineHp);    // ��ž�ڷ�ƾ �ص� �� ��Ұ� �� �Ǵ���? -> �ڷ�ƾ�� ������ �־ �ذ�
            }
            if (coroutineZZZ != null)
            {
                StopCoroutine(coroutineZZZ);    
            }
        }
    }

    IEnumerator ZZZ()
    {
        yield return new WaitForSeconds(0.7f);

        zzZ.gameObject.SetActive(true);
        yield return null;

        while(true)
        {
            zzZ.text = "z";
            yield return new WaitForSeconds(0.5f);

            zzZ.text = "zz";
            yield return new WaitForSeconds(0.5f);

            zzZ.text = "zzZ";
            yield return new WaitForSeconds(0.5f);

            zzZ.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    void HpStateUpdate()
    {
        HpState.text = (int)curHP + " / " + maxHP;  // ���� ��ġ�� �ƴ� �������� ��ġ�� ��Ʈ�� ����    
    }

    void HpBarUpdate()
    {
        hpBar.fillAmount = curHP / maxHP;
    }

    void SetHP()
    {
        if (curHP >= maxHP)
        {
            curHP = maxHP;
        }
    }

    void MpStateUpdate()
    {
        MpState.text = (int)curMP + " / " + maxMP;  
    }

    void MpBarUpdate()
    {
        MpBar.fillAmount = curMP / maxMP;
    }

    void SetMP()
    {
        if (curMP >= maxMP)
        {
            curMP = maxMP;
        }
    }

    void NonInvincible()
    {
        gameObject.layer = 3;
    } 

    void MpRecovery()
    {
        curMP += Time.deltaTime * (recoverySpeed / 2f);
    }
}
