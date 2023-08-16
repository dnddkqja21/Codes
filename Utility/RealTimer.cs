using UnityEngine;
using TMPro;

public class RealTimer : MonoBehaviour
{
    [Header("Ÿ�̸�")]
    public TextMeshProUGUI textTimer;

    public float elapseTime = 0;

    private static RealTimer instance = null;
    public static RealTimer Instance { get { return instance; } }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {        
        ReverseTime();
    }    

    private void ReverseTime()
    {
        float chargedTime = GameOption.Instance.chargedTime;

        if(chargedTime - elapseTime <= 0)
        {
            textTimer.text = "00:00";
            return;
        }

        elapseTime += Time.deltaTime;
        // ������ ���� �����ϸ� �� �� , �����ؾ� ��
        string m = Mathf.FloorToInt((chargedTime - elapseTime) / 60).ToString();
        string s = Mathf.FloorToInt((chargedTime - elapseTime) % 60).ToString();

        RedColor();

        if (int.Parse(m) < 10)
        {
            textTimer.text = "0" + m + ":" + s;

            if (int.Parse(s) < 10)
            {
                textTimer.text = "0" + m + ":" + "0" + s;
            }
        }
        else if (int.Parse(m) >= 10)
        {
            textTimer.text = m + ":" + s;

            if (int.Parse(s) < 10)
            {
                textTimer.text = m + ":" + "0" + s;
            }
        }        
    }
    // 5�� ������ �� �ؽ�Ʈ �Ӱ� ����
    void RedColor()
    {
        float chargedTime = GameOption.Instance.chargedTime;

        if (Mathf.FloorToInt((chargedTime - elapseTime) / 60) < 5f)
        {
            textTimer.color = Color.red;
        }
    }
}
