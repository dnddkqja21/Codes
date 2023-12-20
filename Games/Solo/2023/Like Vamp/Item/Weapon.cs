using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 풀링 매니저에서 가져온 무기를 관리
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
            // 0번 무기일 경우 회전시킴
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

        // 지정된 축을 중심으로 목표를 향해 회전시키는 함수
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        AudioManager.instance.PlaySFX(AudioManager.SFX.Range);
    }

    public void Init(ItemData data)
    {
        // 기본 세팅
        name = "Weapon" + data.itemID;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // 프로퍼티 세팅
        id = data.itemID;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int i = 0; i < GameManager.Instance.poolingManager.prefabs.Length; i++)
        {
            // 투사체일 경우 아이디 지정
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

                // 원거리 무기
            default:
                // 원거리 무기에서 스피드는 연사 속도
                speed = 0.5f * Character.WeaponRate;
                break;
        }
        // 핸드 셋
        Hand hand = player.hands[(int)data.itemType];
        hand.spriteRenderer.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // 특정 함수 호출을 모든 자식에게 방송
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // 무기 배치
    void WeaponPlacement()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;

            // i가 자식의 갯수보다 작으면 즉, 이미 자식이 있다면 그대로 사용
            if(i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            // 갯수가 늘어났다면(레벨업) 오브젝트 풀에서 꺼내옴, 부모를 재설정
            else
            {
                bullet = GameManager.Instance.poolingManager.Get(prefabId).transform;
                bullet.parent = transform;
            }

            // 배치 이전 로컬포지션 초기화
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // 근접 무기는 360도에서 갯수로 나눠 각도 배치 
            Vector3 rot = Vector3.forward * 360 * i / count;
            bullet.Rotate(rot);

            // 무기의 위 방향으로 1.5 띄움
            bullet.Translate(bullet.up * 1.5f, Space.World);

            // 근접무기는 관통이 의미가 없음 -100 : infinity
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
        }
    }

    // 무기 레벨업 시 데미지와 카운트 증가
    public void LevelUp(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count + Character.Count;

        // 갯수가 늘었으므로 무기 재배치
        if(id == 0)
        {
            WeaponPlacement();
        }
        // 특정 함수 호출을 모든 자식에게 방송
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
}
