using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fox : MonoBehaviour
{  
    bool isWalk;    // �ȴ���
    bool isRun;
    bool isAction;  // �ൿ�ϴ���

    float walkTime = 3f; // �ȴ� �ð�
    float walkSpeed = 5f;
    float runSpeed = 10f;

    float waitTime = 5f; // ��� �ð�(���̵�, �ȱ�, �ٱ�)
    float savedTime;  // ����� �ð�

    Vector3 dir; // ����

    public Animator ani;
    public Rigidbody rigid;
    public NavMeshAgent nav;
    public Transform target;    // �÷��̾�
    public InventoryUI inventory;

    void Start()
    {
        savedTime = waitTime;
        isAction = true;
    }


    void Update()
    {
        ElapseTime();
        Move();
        Rotation();
        FollowMom();
    }

    void Move()
    {
        if (isWalk)
        {
            rigid.MovePosition(transform.position + (transform.forward * walkSpeed * Time.deltaTime));
        }

        else if(isRun)
        {
            rigid.MovePosition(transform.position + (transform.forward * runSpeed * Time.deltaTime));
        }
    }

    void Rotation()
    {
        if (isWalk || isRun)
        {
            Vector3 tmp = Vector3.Lerp(transform.eulerAngles, dir, 0.01f);
            rigid.MoveRotation(Quaternion.Euler(tmp));
        }
    }

    void ElapseTime()   // �ð� ���
    {
        if (isAction)
        {
            savedTime -= Time.deltaTime;  // ���� �ð��� �پ���, ���� �ൿ ����
            if (savedTime <= 0)
            {
                ResetAction();
            }
        }
    }

    void ResetAction()
    {
        isWalk = false;
        isRun = false;
        isAction = true;
        ani.SetBool("isWalk", isWalk);
        ani.SetBool("isRun", isRun);
        dir.Set(0f, Random.Range(0f, 360f), 0f);
        RandomAction();
    }

    void RandomAction()
    {
        isAction = true;

        int random = Random.Range(0, 3);

        if (random == 0)
        {
            Wait();
        }

        else if (random == 1)
        {
            Run();
        }

        else if (random == 2)
        {
            Walk();
        }
    }

    void Wait()
    {
        savedTime = waitTime;
    }

    void Run()
    {
        savedTime = walkTime;
        isRun = true;
        ani.SetBool("isRun", isRun);
    }

    void Walk()
    {
        savedTime = walkTime;
        isWalk = true;
        ani.SetBool("isWalk", isWalk);
    }


    void FollowMom()
    {
        float dis = Vector3.Distance(transform.position, target.transform.position);
        if (dis > 10f)
        {
            isRun = true;
            nav.SetDestination(target.transform.position);
            ani.SetBool("isRun", isRun);
        }
        else if (dis < 3f)
        {
            isRun = false;
            ani.SetBool("isRun", isRun);
            nav.ResetPath();
        }
    }

    void FindItem()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            if (other.transform != null)
            {
                // ��� �������� �ƴϸ�
                if (other.transform.GetComponent<ItemPickup>().item.itemType != Item.ItemType.Equipment)
                {
                    // �κ��丮 ���Կ� ������ �߰�
                    inventory.AddSlotItem(other.transform.GetComponent<ItemPickup>().item);
                    Destroy(other.gameObject);
                    ani.SetTrigger("toRoll");
                }
            }
        }
    }
}
