using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MiniGameTrigger : MonoBehaviourPun
{
    Transform shootingPos;
    Transform shootingArea;
    Animator animator;
    NavMeshAgent agent;
    FollowPlayer followPlayer;
    CapsuleCollider capsuleCollider;
    Image cooldown;

    int maxHP = 100;
    int curHP;

    [Header("슈팅")]
    [SerializeField]
    KeyCode shootingKey = KeyCode.Space;
    bool shootingCooldown;
    float shootingCooldownDuration = 1f;
    float shootingTimer;

    void Start()
    {
        curHP = maxHP;
        shootingArea = GameManager.Instance.shootingArea;
        shootingPos = transform.Find("Shooting Pos");
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        followPlayer = FindObjectOfType<FollowPlayer>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        UIManagerWorld.Instance.buttonShooting.onClick.AddListener(() =>
        {
            TryLaser();
        });
        cooldown = UIManagerWorld.Instance.buttonShooting.transform.Find("Cooldown").GetComponent<Image>();
        UIManagerWorld.Instance.buttonShooting.gameObject.SetActive(false);
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(shootingKey) && !shootingCooldown)
    //    {
    //        TryLaser();
    //    }
    //}  

    public void TryLaser()
    {
        if(photonView.IsMine)
        {
            ShootingLaser(photonView.Owner.ActorNumber);
            photonView.RPC("ShootingLaser", RpcTarget.Others, photonView.Owner.ActorNumber);
            StartCoroutine(SkillCooldown());
        }
    }

    [PunRPC]
    void ShootingLaser(int actorNumber)
    {        
        StartCoroutine(Shooting(actorNumber));
    }

    IEnumerator Shooting(int actorNumber)
    {
        animator.SetTrigger(Config.Shoot);
        yield return new WaitForSeconds(0.5f);

        SoundManager.Instance.PlaySFX(SFX.Shooting);
        GameObject laser = ShootingManager.Instance.GetLaser();
        laser.GetComponent<Laser>().actorNumber = actorNumber;
        laser.transform.position = shootingPos.position;
        laser.transform.rotation = shootingPos.rotation;

        laser.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;

        #region 미로게임
        if (other.CompareTag("MazeTrigger"))
        {
            string text = LocalizationManager.Instance.LocaleTable("미로안내");
            PopupManager.Instance.ShowTwoButtnPopup(true, text, MazeSpawnManager.Instance.CreateMaze);
        }

        if (other.CompareTag("Coin"))
        {
            MazeSpawnManager.Instance.CountCoin();
            other.gameObject.SetActive(false);
        }
        #endregion

        #region 컬러게임
        if (other.CompareTag("ColorPickerTrigger"))
        {
            string text = LocalizationManager.Instance.LocaleTable("컬러안내");
            PopupManager.Instance.ShowTwoButtnPopup(true, text,
                () => UIManagerWorld.Instance.colorPicker.SetBool("isShow", true), ColorPickerManager.Instance.InitGame);
        }
        #endregion

        #region 슈팅게임       

        if (other.CompareTag("Laser") && curHP > 0)
        {
            curHP -= 50;
            // 죽음 처리
            if (curHP <= 0)
            {
                SoundManager.Instance.PlaySFX(SFX.Death);

                int actorNum = other.GetComponent<Laser>().actorNumber;
                Player shootPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actorNum);
                string message = string.Format("<color=#00ff00>{0}</color> is killed by <color=#ff0000>{1}</color>",
                                                    photonView.Owner.NickName, shootPlayer.NickName);
                ChattingManager.Instance.SendChat(message);
                photonView.RPC("AmIKiller", RpcTarget.All, shootPlayer.NickName);

                StartCoroutine(DeathAndRespawn());
            }
            // 피격 처리
            else
            {
                SoundManager.Instance.PlaySFX(SFX.Hit);
                photonView.RPC("Hit", RpcTarget.All);
            }
        }

        if(other.CompareTag("LaserTrigger"))
        {
            // 취소 버튼 콜백에 지정된 좌표로 이동 추가.
            if(!UIManagerWorld.Instance.buttonShooting.gameObject.activeSelf)
            {
                string text = LocalizationManager.Instance.LocaleTable("슈팅안내");
                PopupManager.Instance.ShowTwoButtnPopup(true, text,
                    () => UIManagerWorld.Instance.buttonShooting.gameObject.SetActive(true), 
                    null,
                    () => transform.position = Config.Position_Shooting);
            }
        }
        #endregion
    }

    void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine) return;

        if(other.CompareTag("LaserTrigger"))
        {
            if(other.isTrigger)
            {
                // 벽 오브젝트의 트리거 비활성화
                ShootingManager.Instance.TriggerOnOff(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine)
            return;

        if (other.CompareTag("MazeTrigger"))
        {
            PopupManager.Instance.ClosePopup();
        }

        if (other.CompareTag("LaserTrigger"))
        {
            // 퇴장 시에는 원버튼 팝업으로 알려주기만 함.
            UIManagerWorld.Instance.buttonShooting.gameObject.SetActive(false);
            string text = LocalizationManager.Instance.LocaleTable("슈팅퇴장");
            PopupManager.Instance.ShowOneButtnPopup(false, text);
            ShootingManager.Instance.TriggerOnOff(true);
        }
    }

    //void CheckLaserZone()
    //{
    //    int groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
    //    Ray ray = new Ray(transform.position, Vector3.down);
    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, 3f, groundLayerMask))
    //    {
    //        UIManagerWorld.Instance.buttonShooting.gameObject.SetActive(hit.collider.CompareTag("LaserZone"));
    //        Debug.Log(hit.collider.tag);
    //    }
    //}

    [PunRPC]
    void Hit()
    {
        animator.SetTrigger(Config.Hit);
    }

    [PunRPC]
    void Death()
    {
        animator.SetTrigger(Config.Death);
    }

    [PunRPC]
    void Rivival()
    {
        animator.SetTrigger(Config.Revival);

        if(photonView.IsMine)
        {
            curHP = maxHP;
            GameManager.Instance.respawnEffect.Play();
        }
    }

    [PunRPC]
    void AmIKiller(string name)
    {
        //Debug.Log("RPC로 넘긴 파라미터 : " + name);
        //Debug.Log("포톤네트워크로컬네임 : " +  PhotonNetwork.LocalPlayer.NickName);        
        
            if(PhotonNetwork.LocalPlayer.NickName == name)
            {
                // 점수 삽입
                TryInsertRank();
                Debug.Log("점수 삽입 성공 : " + name);
            }
            else 
            {
                Debug.Log("피해자입니다.");
            }
    }

    IEnumerator DeathAndRespawn()
    {
        UIManagerWorld.Instance.untouchable.SetActive(true);
        
        photonView.RPC("Death", RpcTarget.All);

        capsuleCollider.enabled = false;

        if(agent.enabled)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        followPlayer.player = null;

        yield return new WaitForSeconds(3f);

        transform.position = GetRandomPositionInBounds();
        followPlayer.player = transform;

        photonView.RPC("Rivival", RpcTarget.All);

        capsuleCollider.enabled = true;
        agent.enabled = true;

        UIManagerWorld.Instance.untouchable.SetActive(false);
    }

    Vector3 GetRandomPositionInBounds()
    {
        // 오브젝트의 바운드 가져오기
        Bounds bounds = shootingArea.GetComponent<Renderer>().bounds;

        // x와 z의 범위 내에서 무작위 좌표 생성, y는 일정한 값으로 유지 (오브젝트가 위아래로 이동하지 않는다고 가정)
        Vector3 randomPosition = new Vector3(Random.Range(shootingArea.position.x - bounds.extents.x, shootingArea.position.x + bounds.extents.x), 0f,
                                                Random.Range(shootingArea.position.z - bounds.extents.z, shootingArea.position.z + bounds.extents.z));

        Debug.Log("랜덤 포지션 : " + randomPosition);
        return randomPosition;
    } 
    
    IEnumerator SkillCooldown()
    {
        UIManagerWorld.Instance.buttonShooting.interactable = false;
        shootingCooldown = true;

        shootingTimer = shootingCooldownDuration;

        while(shootingTimer > 0)
        {
            yield return null;
            shootingTimer -= Time.deltaTime;
            UpdateCooldown();
        }

        cooldown.fillAmount = 0f;
        UIManagerWorld.Instance.buttonShooting.interactable = true;
        shootingCooldown = false;
    }

    void UpdateCooldown()
    {
        if (cooldown != null)
        {
            cooldown.fillAmount = shootingCooldown ? shootingTimer / shootingCooldownDuration : 0f;
        }
    }

    async void TryInsertRank()
    {
        await Task.Run(() =>
        {
            RankManager.Instance.InsertGameRecord(Config.Rank_Uuid_Shooting, Config.Record_Shooting, 1);
            RankManager.Instance.InsertGameRecord(Config.Rank_Uuid_Shooting_HOF, Config.Record_Shooting_HOF, 1);
        });
    }
}
