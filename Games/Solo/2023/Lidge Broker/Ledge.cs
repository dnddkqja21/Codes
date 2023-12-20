using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ledge : MonoBehaviour
{
    public Block[] blocks;

    [Header("블록 갯수")]
    public int blockCount;
    [Header("블록 사이즈")]
    public float blockSize;
    [Header("현재 블록")]
    public int nowBlock;

    void Start()
    {
        blocks = GetComponentsInChildren<Block>();        
    }

    public void Alignment()
    {
        blockCount = blocks.Length;

        if(blockCount == 0 ) { return; }

        blockSize = blocks[0].GetComponentInChildren<BoxCollider>().transform.localScale.z;

        for (int i = 0; i < blockCount; i++)
        {
            blocks[i].transform.Translate(0, 0, -i * blockSize);
            blocks[i].Init();
        }

        
    }

    IEnumerator Move()
    {
        float nextPos = transform.position.z + 2;

        while (transform.position.z < nextPos)
        {
            yield return null;
            transform.Translate(0, 0, Time.deltaTime * 20f);
        }
        transform.position = Vector3.forward * nextPos;        
    }

    // 에디터에서 실행할 수 있음, 스크립트의 아래 화살표 눌러 Move 클릭 (파라미터 없을 때만 가능)
    //[ContextMenu("Move")]
    // 버튼에 매개변수 받는 법
    public void Select(int selectType)
    {
        bool result =  blocks[nowBlock].Check(selectType);

        if(result)
        {
            GameManager.success();
            // 블록 카운트로 나눠주어 블록카운트를 넘지 않도록 한다.
            nowBlock = (nowBlock + 1) % blockCount;
            StartCoroutine(Move());
        }
        else
        {
            GameManager.failure();
        }
    }
}
