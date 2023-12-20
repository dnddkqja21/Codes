using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour
{
    // 네비에 의한 움직임

    bool isWalk;    // 걷는지
    bool isRun;
    bool isAction;  // 행동하는지

    float walkTime = 3f; // 걷는 시간
    float walkSpeed = 1f;

    float runTime = 2f;
    float runSpeed = 2f;
    float moveDis = 5f; // 움직일 거리    

    float waitTime = 5f; // 대기 시간(아이들, 이팅, 두리번)
    float savedTime;  // 저장된 시간

    Vector3 dir; // 방향

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

    void ElapseTime()   // 시간 경과
    {
        if(isAction)
        {
            savedTime -= Time.deltaTime;  // 현재 시간이 줄어들고, 다음 행동 개시
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
        // 방향 설정을 플레이어의 반대 방향으로 설정한다. 
        // 몬스터 입장에서는 플레이어의 포워드가 플레이어의 반대 방향이 된다.
        dir = new Vector3(_PlayerDir.x, 0f, _PlayerDir.z).normalized;

        savedTime = runTime;
        isWalk = false;
        isRun = true;
        nav.speed = runSpeed;
        ani.SetBool("doRun", isRun);
    }
}
