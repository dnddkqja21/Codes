using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCollision : MonoBehaviour
{
    MeshRenderer mRenderer;

    public Bounds BD
    {
        get { return mRenderer.bounds; }
    }    

    void Awake()
    {
        mRenderer = GetComponent<MeshRenderer>();
    }

    public BoundsCollision other;

    void Start()
    {       
        if(gameObject.tag == "Player")
        {
            for (int i = 0; i < BoundsManager.boundsManager.boundBodyList.Count; i++)
            {
                if (BoundsManager.boundsManager.boundBodyList[i].gameObject.tag == "NPC")
                {
                    other = BoundsManager.boundsManager.boundBodyList[i];
                    Debug.Log(other.transform.name);
                }
            }  
        }
        if(gameObject.tag == "NPC")
        {
            for (int i = 0; i < BoundsManager.boundsManager.boundBodyList.Count; i++)
            {
                if (BoundsManager.boundsManager.boundBodyList[i].gameObject.tag == "Player")
                {
                    other = BoundsManager.boundsManager.boundBodyList[i];
                    Debug.Log(other.transform.name);
                }
            }
        }        
    }

    public int GetNum()
    {
        if (BD.Intersects(other.BD))
        {
            return 1;
        }
        return -1;
    }
}
