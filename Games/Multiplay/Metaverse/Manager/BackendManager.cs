using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Threading.Tasks;

public class BackendManager : MonoBehaviour
{
    void Start()
    {
        var backend = Backend.Initialize(true);
        if(backend.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ���� " + backend);
        }
        else
        {
            Debug.Log("�ʱ�ȭ ���� " + backend);
        }
    }

    async void TEST()
    {
        await Task.Run(() =>
        {

        });
    }
}
