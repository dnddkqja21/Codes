using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAlpha : MonoBehaviour
{
    Transform player;
    List<GameObject> Alphalist = new List<GameObject>();

    void Start()
    {
        player = PhotonManagerWorld.Instance.player.transform;
    }

    void Update()
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 dir = player.transform.position - origin;

        RaycastHit [] hits = Physics.RaycastAll(origin, dir.normalized);

        for(int i = 0; i < hits.Length; i++)
        {
            //Debug.Log(hits[i].collider.gameObject);

            // 광선과 교차한 게임오브젝트를 리스트에 보관
            // 단, 리스트에 이미 보관되어 있다면 추가 하지 않는다.
            GameObject findObj = Alphalist.Find(o => (o.Equals(hits[i].collider.gameObject)));
            if (findObj == null)
            {
                Alphalist.Add(hits[i].collider.gameObject);
                Color newColor = hits[i].collider.gameObject.GetComponent<MeshRenderer>().material.color;
                newColor.a = 0.2f;
                hits[i].collider.gameObject.GetComponent<MeshRenderer>().material.color = newColor;
            }
        }
    }
}
