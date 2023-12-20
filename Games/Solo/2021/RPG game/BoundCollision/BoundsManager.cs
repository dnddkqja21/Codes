using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsManager : MonoBehaviour
{
    public static BoundsManager boundsManager = null;

    public List<Bounds> boundList = new List<Bounds>();

    public List<BoundsCollision> boundBodyList = new List<BoundsCollision>();

    public GameObject button;

    Vector3 offSet = new Vector3(0, 1f, 0);

    private void Awake()
    {
        if (boundsManager == null)
            boundsManager = this;

        BoundsCollision[] bds = FindObjectsOfType<BoundsCollision>();

        for (int i = 0; i < bds.Length; i++)
        {
            boundList.Add(bds[i].BD);
        }

        for (int i = 0; i < bds.Length; i++)
        {
            boundBodyList.Add(bds[i]);
        }
    }

    private void Start()
    {
        button.SetActive(false);        
    }

    private void Update()
    {
        button.transform.position = Camera.main.WorldToScreenPoint(transform.position + offSet);

        for (int i = 0; i < boundBodyList.Count; i++)
        {
            int tmp = boundBodyList[i].GetNum();
            if (tmp == 1)
                button.SetActive(true);
            else
                button.SetActive(false);
        }
    }
}
