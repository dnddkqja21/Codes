using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ǯ�� �Ŵ������� ������ ���⸦ ����
public class Weapon : MonoBehaviour
{
    PlayerMove player;

    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;

    void Awake()
    {
        player = GameManager.Instance.player;
    }

    void Update()
    {
        if (!GameManager.Instance.isLive)
            return;

        switch (id)
        {
            // 0�� ������ ��� ȸ����Ŵ
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default:
                timer += Time.deltaTime;
                if(timer > speed)
                {
                    timer = 0;
                    Fire();
                }
                break;
        }

        // test
        if(Input.GetButtonDown("Jump"))
        {
            LevelUp(20, 3);
        }
    }

    void Fire()
    {
        if (!player.scanner.target)
            return;

        Vector3 targetPos = player.scanner.target.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.Instance.poolingManager.Get(prefabId).transform;
        bullet.position = transform.position;

        // ������ ���� �߽����� ��ǥ�� ���� ȸ����Ű�� �Լ�
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySFX(AudioManager.SFX.Range);
    }

    public void Init(ItemData data)
    {
        // �⺻ ����
        name = "Weapon" + data.itemID;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // ������Ƽ ����
        id = data.itemID;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int i = 0; i < GameManager.Instance.poolingManager.prefabs.Length; i++)
        {
            // ����ü�� ��� ���̵� ����
            if (data.projectile == GameManager.Instance.poolingManager.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch(id)
        {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                WeaponPlacement();
                break;

                // ���Ÿ� ����
            default:
                // ���Ÿ� ���⿡�� ���ǵ�� ���� �ӵ�
                speed = 0.5f * Character.WeaponRate;
                break;
        }
        // �ڵ� ��
        Hand hand = player.hands[(int)data.itemType];
        hand.spriteRenderer.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // Ư�� �Լ� ȣ���� ��� �ڽĿ��� ���
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // ���� ��ġ
    void WeaponPlacement()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;

            // i�� �ڽ��� �������� ������ ��, �̹� �ڽ��� �ִٸ� �״�� ���
            if(i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            // ������ �þ�ٸ�(������) ������Ʈ Ǯ���� ������, �θ� �缳��
            else
            {
                bullet = GameManager.Instance.poolingManager.Get(prefabId).transform;
                bullet.parent = transform;
            }

            // ��ġ ���� ���������� �ʱ�ȭ
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // ���� ����� 360������ ������ ���� ���� ��ġ 
            Vector3 rot = Vector3.forward * 360 * i / count;
            bullet.Rotate(rot);

            // ������ �� �������� 1.5 ���
            bullet.Translate(bullet.up * 1.5f, Space.World);

            // ��������� ������ �ǹ̰� ���� -100 : infinity
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
        }
    }

    // ���� ������ �� �������� ī��Ʈ ����
    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count + Character.Count;

        // ������ �þ����Ƿ� ���� ���ġ
        if(id == 0)
        {
            WeaponPlacement();
        }
        // Ư�� �Լ� ȣ���� ��� �ڽĿ��� ���
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
}
