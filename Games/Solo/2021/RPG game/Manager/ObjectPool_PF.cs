using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 재사용 가능한 몬스터의 경우 오브젝트 풀에서 관리.

public class ObjectPool_PF : MonoBehaviour
{
    public ResourceManager_PF rM = null;


    // 리스트로 관리 몬스터
    List<Monster_PF> poolList = new List<Monster_PF>();   // 재활용 가능한 몬스터를 풀에 저장
    List<Monster_PF> monsterList = new List<Monster_PF>();    // 씬에 소환된 몬스터 리스트    

    // 풀 오브젝트 , 씬 오브젝트
    List<GameObject> poolObject = new List<GameObject>(); // 재활용 가능 오브젝트를 풀에 저장
    List<GameObject> sceneObject = new List<GameObject>();    // 씬에 출력된 오브젝트

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

        if(tmpObj != null)  // 풀에 있다면
        {
            tmpObj.gameObject.SetActive(true);  // 활성화
            tmpObj.transform.position = Vector3.zero;
            sceneObject.Add(tmpObj);    // 씬 오브젝트에 추가
            poolObject.Remove(tmpObj);  // 풀 오브젝트에서 제거
            return tmpObj;
        }

        else
        {
            // 풀에 없다면 생성
            GameObject tmpObj2 = rM.GetObjectPool(_name);   // 리소스 매니저에서 이름으로 찾기
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
        // 풀리스트에서 이름으로 검색한 게임오브젝트 검색
        Monster_PF findObj = poolList.Find(o => (o.gameObject.name.Equals(_name)));

        if (findObj != null) // 풀리스트에 있다면
        {
            findObj.gameObject.SetActive(true); // 활성화
            monsterList.Add(findObj);   // 몬스터리스트에 저장
            poolList.Remove(findObj);   // 풀리스트에서 제거
        }

        else // 풀리스트에 없다면 리소스 매니저를 통해 새로운 몬스터 생성
        {
            GameObject _obj = rM.GetMonsterRC(_name);   // 리소스 매니저에서 로드된 리소스를 저장하는 곳에서 이름으로 비교해봄
            if (_obj != null)
            {
                GameObject obj = Instantiate<GameObject>(_obj); // 화면에 출력
                Monster_PF tmp = obj.AddComponent<Monster_PF>();  // 몬스터 스크립트 추가
                tmp.transform.position = new Vector3(Random.Range(-10, 10), 2, Random.Range(-10, 10));
                tmp.POOLING = this; // 생성 시 스크립트를 알고있는 것
                tmp.name = _name;
                monsterList.Add(tmp);   // 몬스터리스트에 보관
            }
        }
    }


    public void AddPoolList(Monster_PF monster)
    {
        monster.gameObject.SetActive(false);    // 비활성화 시킨 후
        monsterList.Remove(monster);    // 몬스터 리스트에서 제거
        poolList.Add(monster);  // 풀리스트에 저장
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
