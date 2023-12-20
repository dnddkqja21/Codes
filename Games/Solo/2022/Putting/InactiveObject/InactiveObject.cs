using UnityEngine;

public class InactiveObject : MonoBehaviour
{   
    [Header("비활성화 시간")]
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
