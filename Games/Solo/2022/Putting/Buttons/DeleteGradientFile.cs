using System.Collections.Generic;
using UnityEngine;

public class DeleteGradientFile : MonoBehaviour
{
    [Header("기울기 파일의 부모 트랜스폼")]
    public Transform content;

    List<Transform> deleteList = new List<Transform>();
    public void Delete()
    {
        // 클리어 안 하면 삭제된 리스트에 접근 오류 발생
        deleteList.Clear();

        for (int i = 0; i < content.childCount; i++)
        {
            deleteList.Add(content.GetChild(i));
            Destroy(deleteList[i].gameObject);
        }

    }
}
