using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    // 6.1 넣으면 실패 안 했을 경우 해 뜨는 것까지 볼 수 있음
    public float changeSpeed;
    void LateUpdate()
    {
        if(GameManager.isGameOver) { return; }

        // 로테이트 함수의 x축값을 증가시키면 라이트의 위치가 회전하며 낮과 밤의 효과를 연출할 수 있다.
        transform.Rotate(Vector3.right * Time.deltaTime * changeSpeed);
    }
}
