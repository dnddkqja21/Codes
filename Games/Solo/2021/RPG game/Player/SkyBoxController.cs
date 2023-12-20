using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkyBoxController : MonoBehaviour
{    
    public GameObject skyBoxDay;

    public GameObject skyBoxNight;

    public LightController lightController;

    public Image dragonBg;

    public GameObject warning;

    bool isActive = false;

    [SerializeField]
    LayerMask layerMask;

    void Start()
    {
        
    }

    
    void Update()
    {
        RaycastHit hitinfo;
        if(Physics.Raycast(transform.position, Vector3.down, out hitinfo, Mathf.Infinity, layerMask))
        {
            //Debug.Log(hitinfo.transform.tag);
            if(hitinfo.transform.tag == "Sea" || hitinfo.transform.tag == "Orc" || hitinfo.transform.tag == "BaseCamp")
            {
                skyBoxDay.SetActive(true);
                skyBoxNight.SetActive(false);
                lightController.DayLight();
            }
            else if(hitinfo.transform.tag == "Skel" || hitinfo.transform.tag == "Mage")
            {
                skyBoxDay.SetActive(false);
                skyBoxNight.SetActive(true);
                lightController.NightLight();
            }
            else
            {
                dragonBg.enabled = true;
                skyBoxDay.SetActive(false);
                skyBoxNight.SetActive(true);
                lightController.DragonLava();

                if(isActive == false)
                {
                    warning.SetActive(true);
                    isActive = true;
                }
            }
        }

        Debug.DrawRay(transform.position, Vector3.down, Color.red, Mathf.Infinity);
    }
}
