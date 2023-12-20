using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chick : MonoBehaviour
{
    // ������ �ٵ� ���� �̵�, �׺� ���� ���󰡱�

    bool isWalk;    // �ȴ���
    bool isRun;
    bool isAction;  // �ൿ�ϴ���

    float walkTime = 3f; // �ȴ� �ð�
    float walkSpeed = 2f;
    
    float waitTime = 5f; // ��� �ð�(���̵�, ����, �θ���)
    float savedTime;  // ����� �ð�

    Vector3 dir; // ����

    public Animator ani;
    public Rigidbody rigid;
    public NavMeshAgent nav;
    public Transform target;

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
    }

    void Rotation()
    {
        if (isWalk)
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
        ani.SetBool("doWalk", isWalk);
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
            Eat();
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

    void Eat()
    {
        savedTime = waitTime;
        ani.SetTrigger("toEat");
    }    

    void Walk()
    {
        savedTime = walkTime;
        isWalk = true;
        ani.SetBool("doWalk", isWalk);
    }


    void FollowMom()
    {
        float dis = Vector3.Distance(transform.position, target.transform.position);
        if (dis > 3f)
        {
            isRun = true;
            nav.SetDestination(target.transform.position);
            ani.SetBool("doRun", isRun);            
        }
        else if (dis < 1.5f)
        {
            isRun = false;
            ani.SetBool("doRun", isRun);
            nav.ResetPath();
        }
    }
}
