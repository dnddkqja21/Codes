using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 카메라 줌
/// PC, 모바일 버전 분기
/// </summary>

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    MoveJoystick moveJoystick;
    
    // 시네머신 카메라 적용
    CinemachineVirtualCamera virtualCamera;
    Cinemachine3rdPersonFollow followCamera;

    // 시네머신에 맞게 감도 조절 
    float wheel;
    float wheelSpeed = 5f;

    const float MIN_ZOOM = -12f;
    const float MAX_ZOOM = -0.8f;

    public bool isZooming;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        // 바디 타입에 따라 제대로 가져와야 한다.
        followCamera = virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    void Update()
    {
        ZoomInput();
    }

    void LateUpdate()
    {
        // 전처리기에 의해 에디터일 시 줌, 안드로이드일 시 모바일 줌
#if UNITY_EDITOR_WIN || UNITY_STANDALONE || UNITY_EDITOR_OSX
        Zoom();

#elif UNITY_ANDROID || UNITY_IOS // 스위칭한 플랫폼에 따라 자동완성 유무 결정 
        MobileZoom();        
#endif
    }
        
    void MobileZoom()
    {
        if (Input.touchCount == 2 && !moveJoystick.isMoving) //손가락 2개가 눌렸을 때, 플레이어가 이동 중이지 않을 때
        {
            isZooming = true;
            //Debug.Log("터치 2개 입력 됨");
            Touch touchOne = Input.GetTouch(0); // 첫 번째 터치
            Touch touchTwo = Input.GetTouch(1); // 두 번째 터치

            // 각 터치에 대한 이전 위치 값을 각각 저장함
            // deltaPosition : 마지막 프레임에서 발생한 터치 위치와 현재 프레임에서 발생한 터치 위치의 차이 (이동 방향 추적)
            // 처음 터치한 위치에서 deltaPosition을 뺀다
            Vector2 touchOnePrev = touchOne.position - touchOne.deltaPosition; 
            Vector2 touchTwoPrev = touchTwo.position - touchTwo.deltaPosition;

            // 각 프레임에서 터치 사이의 벡터 거리 구함
            // magnitude : 두 점간의 거리 비교(벡터)
            float prevTouchDelta = (touchOnePrev - touchTwoPrev).magnitude; // 현재 터치 델타
            float touchDelta = (touchOne.position - touchTwo.position).magnitude;   // 첫 터치 델타

            // 거리 차이 구함. 현재 터치 델타에서 이전 델타를 빼준다 (손가락을 벌린 상태 : 줌아웃 상태)
            float deltaDiff =  prevTouchDelta - touchDelta;
            
            // 거리 차이가 양수 또는 음수냐에 따라 값을 정해준다. 1 : -1을 하게 되면 감도가 너무 높아서 1/10을 해주었다.
            deltaDiff = deltaDiff > 0 ? 0.1f : -0.1f;

            wheel += deltaDiff;

            wheel = Mathf.Clamp(wheel, MIN_ZOOM, MAX_ZOOM);

            // 시네머신의 디스턴스는 반대방향이다.
            followCamera.CameraDistance = -wheel * wheelSpeed;
        }
        else
        {
            isZooming = false;
        }
    }

    void ZoomInput()
    {
        #region 이전코드
        /*
        // 스크롤 입력에 - 곱하여 반대 방향 계산
        wheel = -Input.GetAxis("Mouse ScrollWheel");
        cameraDir = playerCamera.localRotation * Vector3.forward;
        dis = Vector3.Distance(player.transform.position, playerCamera.position);
        newDis = dis + cameraDir.magnitude * wheel * wheelSpeed;
        */
        #endregion
        wheel += Input.GetAxis("Mouse ScrollWheel");
    }

    void Zoom()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        // 줌 기능이 카메라의 위치를 업데이트 시키기 때문에 휠 입력이 있을 때만 작동해야 한다.
        if (wheel != 0)
        {
            #region 이전코드
            /*
            if (newDis < maxZoom)
            {
                newDis = maxZoom;
            }
            else if (newDis > minZoom)
            {
                newDis = minZoom;
            }
            
            // newDis를 카메라의 포지션에 영향을 주도록 하여야 한다.
            playerCamera.position = player.transform.position - cameraDir.normalized * newDis;
            */

            //Debug.Log(wheel);
            #endregion  

            // 프로젝트에 따라 다른 로직으로 작동해야 함.            
            wheel = Mathf.Clamp(wheel, MIN_ZOOM, MAX_ZOOM);
            followCamera.CameraDistance = -wheel * wheelSpeed;
            //playerCamera.transform.localPosition = new Vector3(0, 0, wheel) * wheelSpeed;
        }
    }
}
