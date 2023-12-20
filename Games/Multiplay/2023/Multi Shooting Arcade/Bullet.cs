using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject effect;

    // �Ѿ� �߻��� �÷��̾� ���� ��ȣ
    public int actorNumber;

    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000f);
        Destroy(this.gameObject, 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        // �浹 ���� 
        var contact = collision.GetContact(0);
        // ����Ʈ ����
        var obj = Instantiate(effect, contact.point, Quaternion.LookRotation(-contact.normal));
        Destroy(obj, 2f);
        Destroy(this.gameObject);
    }
}
