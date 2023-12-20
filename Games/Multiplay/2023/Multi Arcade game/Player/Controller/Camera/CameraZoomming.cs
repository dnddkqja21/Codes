using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 플랫폼에 따른 카메라 줌
/// </summary>

public class CameraZoomming : MonoBehaviour
{    
    PlayerMoveOnMobile moveJoystick;

    // 시네머신 카메라 적용
    CinemachineVirtualCamera virtualCamera;
    Cinemachine3rdPersonFollow followCamera;

    // 시네머신에 맞게 감도 조절 
    float wheel;
    float wheelSpeed = 5f;

    const float MIN_ZOOM = -6f;
    const float MAX_ZOOM = -0.6f;

    public bool isZoomming;

    void Start()
    {
        moveJoystick = UIManagerWorld.Instance.moveJoystick;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        // 시네머신 속성 중 바디 타입에 따라 정확하게 가져와야 한다.
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

#elif UNITY_ANDROID || UNITY_IOS // 스위칭한 플랫폼에 따라 줌 방식 결정 
        MobileZoom();        
#endif
    }

    void ZoomInput()
    {        
        wheel += Input.GetAxis("Mouse ScrollWheel");
    }

    void Zoom()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        // 줌 기능이 카메라의 위치를 업데이트 시키기 때문에 휠 입력이 있을 때만 작동해야 한다.
        if (wheel != 0)
        {        
            wheel = Mathf.Clamp(wheel, MIN_ZOOM, MAX_ZOOM);
            followCamera.CameraDistance = -wheel * wheelSpeed;
        }
    }

    void MobileZoom()
    {
        if (Input.touchCount == 2 && !moveJoystick.isMoving) //손가락 2개가 눌렸을 때, 플레이어가 이동 중이지 않을 때
        {
            isZoomming = true;
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
            float deltaDiff = prevTouchDelta - touchDelta;

            // 거리 차이가 양수 또는 음수냐에 따라 값을 정해준다. 1 : -1을 하게 되면 감도가 너무 높아서 1/10을 해주었다.
            deltaDiff = deltaDiff > 0 ? 0.1f : -0.1f;

            wheel += deltaDiff;

            wheel = Mathf.Clamp(wheel, MIN_ZOOM, MAX_ZOOM);

            // 시네머신의 디스턴스는 반대방향이다.
            followCamera.CameraDistance = -wheel * wheelSpeed;
        }
        else
        {
            isZoomming = false;
        }
    }
}
