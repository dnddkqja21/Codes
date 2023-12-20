using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoomStone : MonoBehaviour
{
    public GameObject warning;

    public GameObject dragon;

    public GameObject invincibleArea;

    ActionController player;

    GameObject area;

    [SerializeField]
    LayerMask layerMask;

    bool isCreated = false;    

    void Start()
    {
        dragon = GameObject.Find("Dragon");
        area = Instantiate(invincibleArea);
        area.transform.position = transform.position;
        player = FindObjectOfType<ActionController>();

        Invoke("LookDragon", 1.4f);
        Invoke("SetArea", 3f);        
    }

    
    void Update()
    {
        RaycastHit hitInfo;

        if(Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity, layerMask))
        {
            if (isCreated == false)
            {
                GameObject tmp = Instantiate(warning);

                tmp.transform.position = hitInfo.point + new Vector3(0, 0.3f, 0);

                isCreated = true;
            }
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MonstersAttack")
        {
            Destroy(gameObject, 0.5f);
        }

        if(other.tag == "Player")
        {
            Destroy(gameObject, 0.5f);
        }
    }

    void LookDragon()
    {
        transform.LookAt(dragon.transform);
        player.PlayClips(10);
    }   
    
    void SetArea()
    {
        area.transform.position = transform.position + transform.forward * -2f;
    }
}
