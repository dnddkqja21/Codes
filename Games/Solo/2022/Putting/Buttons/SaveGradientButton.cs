using UnityEngine;

public class SaveGradientButton : MonoBehaviour
{    
    public void SaveButton()
    {
        var manager = SaveRecord.Instance;
        if (manager.caution.enabled == true)
        {            
            return;
        }
        manager.Save();        
    }
}
