using UnityEngine;

public class TranningStart : MonoBehaviour
{
    public void OnStart()
    {
        GameOption.Instance.isTranning = true;
        
    }
}
