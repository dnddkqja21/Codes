using Photon.Pun;
using UnityEngine;

// 총알을 충전하는 아이템
public class AmmoPack : MonoBehaviourPun, IItem 
{
    public int ammo = 30; // 충전할 총알 수

    public void Use(GameObject target) 
    {        
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();
        
        if (playerShooter != null && playerShooter.gun != null)
        {
            // 총의 남은 탄환 수를 ammo 만큼 더하기, 모든 클라이언트에서 실행
            playerShooter.gun.photonView.RPC("AddAmmo", RpcTarget.All, ammo);
        }
        // 포톤 파괴
        PhotonNetwork.Destroy(gameObject);
    }
}