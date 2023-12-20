using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour
{
    // �׺� ���� ������

    bool isWalk;    // �ȴ���
    bool isRun;
    bool isAction;  // �ൿ�ϴ���

    float walkTime = 3f; // �ȴ� �ð�
    float walkSpeed = 1f;

    float runTime = 2f;
    float runSpeed = 2f;
    float moveDis = 5f; // ������ �Ÿ�    

    float waitTime = 5f; // ��� �ð�(���̵�, ����, �θ���)
    float savedTime;  // ����� �ð�

    Vector3 dir; // ����

    public Animator ani;
    //public Rigidbody rigid;
    public NavMeshAgent nav;

    ActionController player;
    void Start()
    {
        savedTime = waitTime;
        isAction = true;
        player = FindObjectOfType<ActionController>();
    }

    
    void Update()
    {
        ElapseTime();
        Move();
        //Rotation();
    }

    void Move()
    {
        if(isWalk || isRun)
        {
            nav.SetDestination(transform.position + dir * moveDis);
        }
    }    

    void ElapseTime()   // �ð� ���
    {
        if(isAction)
        {
            savedTime -= Time.deltaTime;  // ���� �ð��� �پ���, ���� �ൿ ����
            if(savedTime <= 0)
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
        nav.speed = walkSpeed;
        nav.ResetPath();
        ani.SetBool("doWalk", isWalk);
        ani.SetBool("doRun", isRun);
        dir.Set(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-1f, 1f));
        RandomAction();
    }

    void RandomAction()
    {
        isAction = true;

        int random = Random.Range(0, 4);

        if(random == 0)
        {            
            Wait();
        }

        else if(random == 1)
        {
            Eat();
        }

        else if (random == 2)
        {
            LookOut();
        }

        else if (random == 3)
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

    void LookOut()
    {
        savedTime = waitTime;
        ani.SetTrigger("toLookOut");        
    }

    void Walk()
    {
        savedTime = walkTime;
        isWalk = true;
        nav.speed = walkSpeed;
        ani.SetBool("doWalk", isWalk);        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player.PlayClips(3);
            Run(other.transform.forward);
        }
    }  
    
    void Run(Vector3 _PlayerDir)
    {
        // ���� ������ �÷��̾��� �ݴ� �������� �����Ѵ�. 
        // ���� ���忡���� �÷��̾��� �����尡 �÷��̾��� �ݴ� ������ �ȴ�.
        dir = new Vector3(_PlayerDir.x, 0f, _PlayerDir.z).normalized;

        savedTime = runTime;
        isWalk = false;
        isRun = true;
        nav.speed = runSpeed;
        ani.SetBool("doRun", isRun);
    }
}
