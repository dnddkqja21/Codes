using UnityEngine;

public class ToStandby : MonoBehaviour
{
    [Header("���Ĺ��� �˾�")]
    public StanbyUI standby;
    [Header("x�� �� ���ȭ��")]
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
            // �帥 �ð��� �ʱ�ȭ
            elapseTime = 0;            

            // Ű �Է� ������  
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
