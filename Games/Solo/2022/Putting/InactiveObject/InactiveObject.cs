using UnityEngine;

public class InactiveObject : MonoBehaviour
{   
    [Header("��Ȱ��ȭ �ð�")]
    public float inactiveTime = 3f;

    private void OnEnable()
    {
        Invoke("Inactive", inactiveTime);
    }

    void Inactive()
    {
        gameObject.SetActive(false);
    }
}
