using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Rigidbody[] characters;
    public int type;
    Ledge ledge;

    void Start()
    {
        ledge = GetComponentInParent<Ledge>();        
    }

    void LateUpdate()
    {
        // z축이 4만큼 앞으로 왔다면 맨 뒤로 이동
        if(transform.position.z == 4)
        {
            transform.Translate(0, 0, ledge.blockCount * -ledge.blockSize);
            Init();
        }
    }

    public void Init()
    {
        type = Random.Range(0, characters.Length);

        for (int i = 0; i < characters.Length; i++)
        {
            // 랜덤으로 캐릭터를 활성화
            characters[i].gameObject.SetActive(type == i);
        }
        StartCoroutine(InitRigid());
    }

    IEnumerator InitRigid()
    {
        // 리지드 바디의 속성을 초기화
        characters[type].isKinematic = false;
        yield return new WaitForFixedUpdate();

        characters[type].velocity = Vector3.zero;
        characters[type].angularVelocity = Vector3.zero;
        yield return new WaitForFixedUpdate();

        characters[type].transform.localPosition = Vector3.zero;
        characters[type].transform.localRotation = Quaternion.identity;
    }

    public bool Check(int selectType)
    {
        bool result = type == selectType;

        if (result)
        {
            StartCoroutine(Hit());
        }

        return result;
    }

    IEnumerator Hit()
    {
        // 키네마틱 체크 = 게임에서 외부 환경에 의해 제어되지 않고 스크립트로 제어 하도록 함
        // 날리기 전에 키네마틱 해제 (Add(Force, Torque) = 스크립트이지만 외부에서 힘을 주는 방식임)
        characters[type].isKinematic = false;
        // 키네마틱 해제 하자마자 힘을 주는 것보다는 물리 한 프레임 쉬고 주는 것이 자연스러움
        yield return new WaitForFixedUpdate();

        int random = Random.Range(0, 2);
        Vector3 forceVector = Vector3.zero;
        Vector3 torqueVector = Vector3.zero;

        switch (random)
        {
            case 0:
                forceVector = (Vector3.right + Vector3.up) * 5f;
                torqueVector = (Vector3.forward + Vector3.down) * 5f;
                characters[type].AddForce(forceVector, ForceMode.Impulse);
                characters[type].AddTorque(torqueVector, ForceMode.Impulse);
                break;

            case 1:
                forceVector = (Vector3.left + Vector3.up) * 5f;
                torqueVector = (Vector3.back + Vector3.up) * 5f;
                characters[type].AddForce(forceVector, ForceMode.Impulse);
                characters[type].AddTorque(torqueVector, ForceMode.Impulse);
                break;

        }
    }
}
