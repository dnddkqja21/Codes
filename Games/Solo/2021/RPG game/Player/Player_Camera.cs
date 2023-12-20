using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    // 이전 위치를 초기화
    public Vector3 oldPos = Vector3.zero;   

    public GameManager_PF gameManager;   // 게임 매니저는 플레이어를 알고있으므로 카메라가 게임메니저를 알고있으면 된다.

    float rotateSpeed = 300f;

    public Player_PF player;

    // 카메라 줌 인 아웃
    float zoomSpeed = 3f;
    float zoomMax = 5f;
    float zoomMin = 15f;
   
    void Start()
    {       
        // 이전 위치에 현재 오브젝트 위치를 대입
        // 시작 시에는 이전 위치와 캐릭터 현재 위치가 같다.         
        if(player != null)
        {
            oldPos = player.transform.position;
        }

    }    

    void LateUpdate()
    {
        CameraZoom(); 

        if (player != null)
        {
            Vector3 delta = player.transform.position - oldPos;

            transform.position += delta;

            oldPos = player.transform.position;

            float x1 = Input.GetAxis("Mouse X");

            if (Input.GetMouseButton(1))
            {
                transform.RotateAround(player.transform.position, Vector3.up, x1 * Time.deltaTime * rotateSpeed);  
            }
        }        
    }

    void CameraZoom()
    {
        float zoomDir = Input.GetAxis("Mouse ScrollWheel");

        // 카메라의 y로 줌인아웃 한계치 설정
        if (transform.position.y <= zoomMax && zoomDir > 0)
            return;
        if (transform.position.y >= zoomMin && zoomDir < 0)
            return;

        transform.position += transform.forward * zoomDir * zoomSpeed;
    }
}
