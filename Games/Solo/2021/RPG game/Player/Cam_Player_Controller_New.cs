using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Player_Controller_New : MonoBehaviour
{
    public Transform player;

    public Transform cameraArm;

    public GameObject ending;

    Animator ani;

    Player_PF mana;

    float moveSpeed = 6f;

    float runSpeed = 9f;

    float rollSpeed = 15f;

    float setSpeed;

    void Start()
    {
        ani = player.GetComponent<Animator>();
        mana = player.GetComponent<Player_PF>();
        setSpeed = moveSpeed;
    }

    
    void Update()
    {
        //Move();
        //LookAround();
        TryRun();
        Rolling();
        GameOver();
    }

    public void Move(Vector2 _stickDir)
    {
        //Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));  // 키보드 이동

        Vector2 moveInput = _stickDir;

        bool isWalk = moveInput.magnitude != 0; // 입력 있을 시

        ani.SetBool("isWalk", isWalk);

        if(isWalk)
        {
            Vector3 lookAt = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookAt * moveInput.y + lookRight * moveInput.x;

            // 게임의 기획에 따른 플레이어 방향 수정
            //player.forward = lookAt;
            player.forward = moveDir;

            transform.position += moveDir * Time.deltaTime * setSpeed;
        }
    }

    public void LookAround(Vector3 _stickDir)
    {
        // 마우스 움직임을 담는 변수
        //Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // 조이스틱으로 카메라 회전
        Vector2 mouseDelta = _stickDir;

        // 카메라 각도
        Vector3 camRot = cameraArm.rotation.eulerAngles;

        // 카메라 회전의 제한
        float x = camRot.x - mouseDelta.y;

        if(x < 180f)    // 180보다 작은 경우 (위쪽 회전)
        {
            x = Mathf.Clamp(x, 1f, 70f);    // 카메라가 수평면 아래를 비춰 맵 아래쪽을 보여주면 안 되기 때문에 회전각의 제한을 둠
        }
        else
        {   // 아랫쪽 회전
            x = Mathf.Clamp(x, 335f, 361f);
        }
        // 마우스 방향과 바라보는 캠의 방향을 일치하도록 함.
        cameraArm.rotation = Quaternion.Euler(x, camRot.y + mouseDelta.x, camRot.z);        
    }

    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && mana.curMP >= 1f) // 마나가 1보다 클 때만 달릴 수 있음
        {
            Running();
            mana.curMP -= Time.deltaTime * 5f;
            ani.SetBool("isRun", true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
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
        if (Input.GetKeyDown(KeyCode.W) && mana.curMP >= 5f)
        {
            mana.curMP -= 8f;
            StartCoroutine(RollingSpeed());
            ani.SetTrigger("toRoll");
        }
    }

    IEnumerator RollingSpeed()
    {
        yield return null;
        setSpeed = rollSpeed;

        yield return new WaitForSeconds(0.5f);
        setSpeed = moveSpeed;
    }

    void GameOver()
    {
        if(Input.GetKeyDown(KeyCode.F4))
        {
            ending.SetActive(true);
        }
    }
}
