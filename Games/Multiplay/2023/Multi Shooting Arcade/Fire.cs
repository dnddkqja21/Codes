using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Transform firePos;
    public GameObject bulletPrefab;

    ParticleSystem muzzleFlash;

    PhotonView pv;

    bool isMouseClick => Input.GetMouseButtonDown(0);

    void Start()
    {
        pv = GetComponent<PhotonView>();    
        muzzleFlash = firePos.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if(pv.IsMine && isMouseClick)
        {
            // 네트워크 유저의 고유 번호를 넘긴다.
            FireBullet(pv.Owner.ActorNumber);
            // RPC로 원격지에 있는 함수 호출
            pv.RPC("FireBullet", RpcTarget.Others, pv.Owner.ActorNumber);

            RoomNumUp();
        }
    }

    [PunRPC]
    void FireBullet(int actorNum)
    {
        // 총구 화염 효과 실행 중 아니라면 머즐플래쉬 실행
        if(!muzzleFlash.isPlaying)
        {
            muzzleFlash.Play(true);
        }

        GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
        bullet.GetComponent<Bullet>().actorNumber = actorNum;
    }

    
    void RoomNumUp()
    {
        //GameManager.Instance.roomNum++;
        
        //int currentNum = (int)PhotonNetwork.LocalPlayer.CustomProperties["number"];
        //int newNum = currentNum++;

        //// 로컬 플레이어 업데이트
        //ExitGames.Client.Photon.Hashtable updateNum = new ExitGames.Client.Photon.Hashtable();
        //updateNum.Add("number", newNum);
        //PhotonNetwork.LocalPlayer.SetCustomProperties(updateNum);

        int currentNum = GameManager.Instance.GetNumber();
        int newNum = currentNum + 1;

        GameManager.Instance.SetNumber(newNum);

        // 리모트 플레이어 업데이트
        pv.RPC("UpdateRoomNum", RpcTarget.Others, newNum);

        Debug.Log("방 번호 : " + GameManager.Instance.GetNumber());
        GameManager.Instance.roomNumText.text = "Room Num : " + GameManager.Instance.GetNumber();
    }

    [PunRPC]
    void UpdateRoomNum(int newNum)
    {
        GameManager.Instance.SetNumber(newNum);
    }    
}
