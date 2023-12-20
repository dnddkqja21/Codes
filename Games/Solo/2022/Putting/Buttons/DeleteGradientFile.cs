using System.Collections.Generic;
using UnityEngine;

public class DeleteGradientFile : MonoBehaviour
{
    [Header("���� ������ �θ� Ʈ������")]
    public Transform content;

    List<Transform> deleteList = new List<Transform>();
    public void Delete()
    {
        // Ŭ���� �� �ϸ� ������ ����Ʈ�� ���� ���� �߻�
        deleteList.Clear();

        for (int i = 0; i < content.childCount; i++)
        {
            deleteList.Add(content.GetChild(i));
            Destroy(deleteList[i].gameObject);
        }

    }
}
