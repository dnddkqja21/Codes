using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC_Name : MonoBehaviour
{
    public GameObject npcName;
    
    Vector3 offSet = new Vector3(0, 2f, 0);

    TextMeshProUGUI nameText;

    List<GameObject> npcNameList = new List<GameObject>();

    void Start()
    {
        Transform parents = GameObject.Find("Canvas2").transform;
        nameText = npcName.GetComponent<TextMeshProUGUI>();
        nameText.text = gameObject.name;

        GameObject tmp = Instantiate(npcName, transform.position, Quaternion.identity, parents);
        npcNameList.Add(tmp);
    }

    
    void Update()
    {
        if (npcNameList[0] != null)
        {
            npcNameList[0].transform.position = Camera.main.WorldToScreenPoint(transform.position + offSet);        
        }
    }
}
