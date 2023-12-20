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
            // ��Ʈ��ũ ������ ���� ��ȣ�� �ѱ��.
            FireBullet(pv.Owner.ActorNumber);
            // RPC�� �������� �ִ� �Լ� ȣ��
            pv.RPC("FireBullet", RpcTarget.Others, pv.Owner.ActorNumber);

            RoomNumUp();
        }
    }

    [PunRPC]
    void FireBullet(int actorNum)
    {
        // �ѱ� ȭ�� ȿ�� ���� �� �ƴ϶�� �����÷��� ����
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

        //// ���� �÷��̾� ������Ʈ
        //ExitGames.Client.Photon.Hashtable updateNum = new ExitGames.Client.Photon.Hashtable();
        //updateNum.Add("number", newNum);
        //PhotonNetwork.LocalPlayer.SetCustomProperties(updateNum);

        int currentNum = GameManager.Instance.GetNumber();
        int newNum = currentNum + 1;

        GameManager.Instance.SetNumber(newNum);

        // ����Ʈ �÷��̾� ������Ʈ
        pv.RPC("UpdateRoomNum", RpcTarget.Others, newNum);

        Debug.Log("�� ��ȣ : " + GameManager.Instance.GetNumber());
        GameManager.Instance.roomNumText.text = "Room Num : " + GameManager.Instance.GetNumber();
    }

    [PunRPC]
    void UpdateRoomNum(int newNum)
    {
        GameManager.Instance.SetNumber(newNum);
    }    
}
