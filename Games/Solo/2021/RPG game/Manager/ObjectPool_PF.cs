using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ������ ������ ��� ������Ʈ Ǯ���� ����.

public class ObjectPool_PF : MonoBehaviour
{
    public ResourceManager_PF rM = null;


    // ����Ʈ�� ���� ����
    List<Monster_PF> poolList = new List<Monster_PF>();   // ��Ȱ�� ������ ���͸� Ǯ�� ����
    List<Monster_PF> monsterList = new List<Monster_PF>();    // ���� ��ȯ�� ���� ����Ʈ    

    // Ǯ ������Ʈ , �� ������Ʈ
    List<GameObject> poolObject = new List<GameObject>(); // ��Ȱ�� ���� ������Ʈ�� Ǯ�� ����
    List<GameObject> sceneObject = new List<GameObject>();    // ���� ��µ� ������Ʈ

    public static ObjectPool_PF objectPoolInstance;

    private void Start()
    {
        objectPoolInstance = this;
    }

    public void CreateMob()
    {
        string str = string.Empty;
        int randomNum = Random.Range(0, 4); 

        switch (randomNum)
        {
            case 0:
                str = "EvilMage";
                break;
            case 1:
                str = "Turtle";
                break;
            case 2:
                str = "Orc";
                break;
            case 3:
                str = "Skeleton";
                break;
        }
        CreateMonster(str);
    }

    public GameObject CreateObject(string _name)
    {
        GameObject tmpObj = poolObject.Find(o => (o.gameObject.name.Equals(_name)));

        if(tmpObj != null)  // Ǯ�� �ִٸ�
        {
            tmpObj.gameObject.SetActive(true);  // Ȱ��ȭ
            tmpObj.transform.position = Vector3.zero;
            sceneObject.Add(tmpObj);    // �� ������Ʈ�� �߰�
            poolObject.Remove(tmpObj);  // Ǯ ������Ʈ���� ����
            return tmpObj;
        }

        else
        {
            // Ǯ�� ���ٸ� ����
            GameObject tmpObj2 = rM.GetObjectPool(_name);   // ���ҽ� �Ŵ������� �̸����� ã��
            if(tmpObj2 != null)
            {
                GameObject tmpObj3 = Instantiate(tmpObj2);
                tmpObj3.SetActive(true);
                tmpObj3.transform.position = Vector3.zero;
                tmpObj3.name = _name;
                sceneObject.Add(tmpObj3);
                return tmpObj3;
            }
        }
        return null;
    }

    void CreateMonster(string _name)
    {
        // Ǯ����Ʈ���� �̸����� �˻��� ���ӿ�����Ʈ �˻�
        Monster_PF findObj = poolList.Find(o => (o.gameObject.name.Equals(_name)));

        if (findObj != null) // Ǯ����Ʈ�� �ִٸ�
        {
            findObj.gameObject.SetActive(true); // Ȱ��ȭ
            monsterList.Add(findObj);   // ���͸���Ʈ�� ����
            poolList.Remove(findObj);   // Ǯ����Ʈ���� ����
        }

        else // Ǯ����Ʈ�� ���ٸ� ���ҽ� �Ŵ����� ���� ���ο� ���� ����
        {
            GameObject _obj = rM.GetMonsterRC(_name);   // ���ҽ� �Ŵ������� �ε�� ���ҽ��� �����ϴ� ������ �̸����� ���غ�
            if (_obj != null)
            {
                GameObject obj = Instantiate<GameObject>(_obj); // ȭ�鿡 ���
                Monster_PF tmp = obj.AddComponent<Monster_PF>();  // ���� ��ũ��Ʈ �߰�
                tmp.transform.position = new Vector3(Random.Range(-10, 10), 2, Random.Range(-10, 10));
                tmp.POOLING = this; // ���� �� ��ũ��Ʈ�� �˰��ִ� ��
                tmp.name = _name;
                monsterList.Add(tmp);   // ���͸���Ʈ�� ����
            }
        }
    }


    public void AddPoolList(Monster_PF monster)
    {
        monster.gameObject.SetActive(false);    // ��Ȱ��ȭ ��Ų ��
        monsterList.Remove(monster);    // ���� ����Ʈ���� ����
        poolList.Add(monster);  // Ǯ����Ʈ�� ����
    }

    public void AddPoolObject(GameObject _obj)
    {
        if(_obj.tag == "Arrow" || _obj.tag == "MagicArrow")
        {
            Rigidbody tmp = _obj.GetComponent<Rigidbody>();
            tmp.velocity = Vector3.zero;
        }
        _obj.gameObject.SetActive(false);
        sceneObject.Remove(_obj);
        poolObject.Add(_obj);
    }
}
