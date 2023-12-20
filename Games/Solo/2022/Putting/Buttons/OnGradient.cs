using UnityEngine;

public class OnGradient : MonoBehaviour
{
    [Header("¹öÆ° ¼Â")]
    public GameObject[] buttonSets;    

    //bool isOn;

    public void OnSetGradient()
    {
        //isOn = !isOn;
        for (int i = 0; i < buttonSets.Length; i++)
        {
            buttonSets[i].SetActive(true);
        }
    }  
    
    public void OffSetGradient()
    {
        for (int i = 0; i < buttonSets.Length; i++)
        {
            buttonSets[i].SetActive(false);
        }
    }
}
