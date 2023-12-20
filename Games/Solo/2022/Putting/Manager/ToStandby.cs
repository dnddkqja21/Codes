using UnityEngine;

public class ToStandby : MonoBehaviour
{
    [Header("스탠바이 팝업")]
    public StanbyUI standby;
    [Header("x초 후 대기화면")]
    public float turnLogoTime = 10f;

    float elapseTime = 0;
    bool isInput = false;
   
    void Update()
    {
        LastInput();
        ElapseTime();
        TurnStandby();
    }

    private void LastInput()
    {
        if (Input.anyKeyDown)
        {
            // 흐른 시간을 초기화
            elapseTime = 0;            

            // 키 입력 감지됨  
            isInput = true;
        }
    }

    void ElapseTime()
    {
        if(isInput)
        {
            elapseTime += Time.deltaTime;
            //Debug.Log(elapseTime);
        }
    }

    void TurnStandby()
    {
        if(elapseTime >= turnLogoTime && isInput)
        {
            standby.Initialize();
            isInput = false;
        }
    }
}
