using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager_PF : MonoBehaviour
{
    // 오브젝트 풀 경로의 오브젝트들 보관함
    List<GameObject> loadedObjectPool = new List<GameObject>(); // 로드된 리소르를 리스트에 보관함
    
    GameObject player;   // 나의 캐릭터를 보관함

    private void Awake()
    {
        GameObject[] dropItem = Resources.LoadAll<GameObject>("ObjectPool");
        for (int i = 0; i < dropItem.Length; i++)
        {
            loadedObjectPool.Add(dropItem[i]);
        }
        

        player = Resources.Load<GameObject>("PLAYER_PF");
       
        
    }
  
    public GameObject GetObjectPool(string _name)    // 외부에서 보관하고있는 리소스에 접근할 수 있도록 하는 함수.
    {
        return loadedObjectPool.Find(o => (o.name.Equals(_name)));
    }   

    public GameObject GetPlayerRC() // 외부에서 플레이어 캐릭터에 접근할 수 있는 함수.
    {
        return player;
    }

    public GameObject GetMonsterRC(string _name)
    {
        return null;
    }

}
