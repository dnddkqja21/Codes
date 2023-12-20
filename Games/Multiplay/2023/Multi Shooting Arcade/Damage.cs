
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviourPunCallbacks
{
    Renderer[] renderers;

    int initHP = 100;
    public int currHP = 100;

    Animator animator;
    CharacterController controller;

    readonly int hashDie = Animator.StringToHash("Die");
    readonly int hashRespawn = Animator.StringToHash("Respawn");

    GameManager gameManager;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        currHP = initHP;

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (currHP > 0 && collision.collider.CompareTag("BULLET"))
        {
            currHP -= 50;
            if(currHP <= 0)
            {
                // 자신의 포톤뷰일 때만 메시지 출력
                if(photonView.IsMine)
                {
                    // 총알의 고유 번호 호출
                    var actorNum = collision.collider.GetComponent<Bullet>().actorNumber;
                    // 고유 번호로 플레이어 추출
                    Player lastShootPlayer = PhotonNetwork.CurrentRoom.GetPlayer(actorNum);

                    // 메시지 출력 문자열 포맷
                    string msg = string.Format("\n<color=#00ff00>{0}</color> is killed by <color=#ff0000>{1}</color>",
                                                photonView.Owner.NickName, lastShootPlayer.NickName); 

                    photonView.RPC("KillMessage", RpcTarget.AllBufferedViaServer, msg);
                }
                StartCoroutine(PlayerDie());
            }
        }

    }

    [PunRPC]
    void KillMessage(string msg)
    {
        gameManager.messageList.text += msg;
    }

    IEnumerator PlayerDie()
    {
        controller.enabled = false;
        animator.SetBool(hashRespawn, false);
        animator.SetTrigger(hashDie);

        yield return new WaitForSeconds(3f);

        animator.SetBool(hashRespawn, true);

        // 플레이어 투명
        SetPlayerVisible(false);

        yield return new WaitForSeconds(1.5f);

        // 생성 위치 재조정
        Transform[] points = GameObject.Find("Spawn").GetComponentsInChildren<Transform>();
        int index = Random.Range(1, points.Length);
        transform.position = points[index].position;

        currHP = initHP;
        SetPlayerVisible(true);
        controller.enabled = true;
    }

    void SetPlayerVisible(bool isVisible)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = isVisible;
        }
    }
}
