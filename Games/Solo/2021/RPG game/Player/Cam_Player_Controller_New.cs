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
        //Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));  // Ű���� �̵�

        Vector2 moveInput = _stickDir;

        bool isWalk = moveInput.magnitude != 0; // �Է� ���� ��

        ani.SetBool("isWalk", isWalk);

        if(isWalk)
        {
            Vector3 lookAt = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookAt * moveInput.y + lookRight * moveInput.x;

            // ������ ��ȹ�� ���� �÷��̾� ���� ����
            //player.forward = lookAt;
            player.forward = moveDir;

            transform.position += moveDir * Time.deltaTime * setSpeed;
        }
    }

    public void LookAround(Vector3 _stickDir)
    {
        // ���콺 �������� ��� ����
        //Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // ���̽�ƽ���� ī�޶� ȸ��
        Vector2 mouseDelta = _stickDir;

        // ī�޶� ����
        Vector3 camRot = cameraArm.rotation.eulerAngles;

        // ī�޶� ȸ���� ����
        float x = camRot.x - mouseDelta.y;

        if(x < 180f)    // 180���� ���� ��� (���� ȸ��)
        {
            x = Mathf.Clamp(x, 1f, 70f);    // ī�޶� ����� �Ʒ��� ���� �� �Ʒ����� �����ָ� �� �Ǳ� ������ ȸ������ ������ ��
        }
        else
        {   // �Ʒ��� ȸ��
            x = Mathf.Clamp(x, 335f, 361f);
        }
        // ���콺 ����� �ٶ󺸴� ķ�� ������ ��ġ�ϵ��� ��.
        cameraArm.rotation = Quaternion.Euler(x, camRot.y + mouseDelta.x, camRot.z);        
    }

    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && mana.curMP >= 1f) // ������ 1���� Ŭ ���� �޸� �� ����
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
