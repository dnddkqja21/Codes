using UnityEngine;
using System.Collections;
using VolumetricLines;

public class Laser : MonoBehaviour 
{	
	[SerializeField]
	Color[] colors;
	[SerializeField]
	GameObject hitEffect;

    public int actorNumber;

    VolumetricLineBehavior lineBehavior;
	Material material;

    float time = 0;
    float speed = 10f;
    float detectionRadius = 0.7f;   // 감지 범위 보정

    void Awake() 
	{
        lineBehavior = GetComponent<VolumetricLineBehavior>();
    }

    void OnEnable()
    {
        time = 0;
		if(material == null)
		{
			// 최초 머터리얼 세팅 및 생성
			int random = Random.Range(0, colors.Length);
			lineBehavior.m_templateMaterial.color = colors[random];

			material = GetComponent<MeshRenderer>().sharedMaterial;
		}
		else
		{
            int random = Random.Range(0, colors.Length);
			material.color = colors[random];
        }
        Invoke("Inactive", 3f);
    }

    void OnDisable()
    {
        // 비활성화 때 인보크 취소를 해줘야 오작동을 피할 수 있다.
        CancelInvoke("Inactive");
    }

    void Update ()
    {        
        time += Time.deltaTime;
        if(time < 0.3f)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        else
        {
            GuidedMissile();
        }
    }

    void GuidedMissile()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        Transform closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                Vector3 targetPosition = hitCollider.transform.position;

                float distanceToPlayer = Vector3.Distance(transform.position, targetPosition);

                // 가장 가까운 플레이어
                if (distanceToPlayer < closestDistance)
                {
                    closestPlayer = hitCollider.transform;
                    closestDistance = distanceToPlayer;
                }
            }
        }

        // 플레이어를 찾았다면 유도탄 운동 시작
        if (closestPlayer != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, closestPlayer.position, speed * Time.deltaTime);

            Vector3 directionToTarget = closestPlayer.position - transform.position;
            transform.rotation = Quaternion.LookRotation(directionToTarget);
        }
        // 플레이어 없다면 직선 운동
        else
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
    }

    void Inactive()
	{
		if(gameObject.activeSelf)
		{
			gameObject.SetActive(false);
		}
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("LaserZone"))
		{
            Instantiate(hitEffect, other.ClosestPointOnBounds(transform.position), transform.rotation);
            Inactive();
        }		
    }
}
