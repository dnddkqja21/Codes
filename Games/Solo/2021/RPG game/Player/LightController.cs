using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    Light lightControl;

    public Color day;

    public Color night;

    public Color Dragon;
    void Start()
    {
        lightControl = GetComponent<Light>();
    }

    
    void Update()
    {
        
    }

    public void DayLight()
    {
        lightControl.color = day;
    }

    public void NightLight()
    {
        lightControl.color = night;
    }

    public void DragonLava()
    {
        lightControl.color = Dragon;
    }
}
