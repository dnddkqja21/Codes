using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager_PF : MonoBehaviour
{
    // ������Ʈ Ǯ ����� ������Ʈ�� ������
    List<GameObject> loadedObjectPool = new List<GameObject>(); // �ε�� ���Ҹ��� ����Ʈ�� ������
    
    GameObject player;   // ���� ĳ���͸� ������

    private void Awake()
    {
        GameObject[] dropItem = Resources.LoadAll<GameObject>("ObjectPool");
        for (int i = 0; i < dropItem.Length; i++)
        {
            loadedObjectPool.Add(dropItem[i]);
        }
        

        player = Resources.Load<GameObject>("PLAYER_PF");
       
        
    }
  
    public GameObject GetObjectPool(string _name)    // �ܺο��� �����ϰ��ִ� ���ҽ��� ������ �� �ֵ��� �ϴ� �Լ�.
    {
        return loadedObjectPool.Find(o => (o.name.Equals(_name)));
    }   

    public GameObject GetPlayerRC() // �ܺο��� �÷��̾� ĳ���Ϳ� ������ �� �ִ� �Լ�.
    {
        return player;
    }

    public GameObject GetMonsterRC(string _name)
    {
        return null;
    }

}
