using Cinemachine; 
using Photon.Pun; 

// 시네머신 카메라가 로컬 플레이어를 추적
public class CameraSetup : MonoBehaviourPun 
{
    void Start() {
        if(photonView.IsMine)
        {
            CinemachineVirtualCamera camera = FindObjectOfType<CinemachineVirtualCamera>();
            camera.Follow = transform;
            camera.LookAt = transform;
        }
    }
}