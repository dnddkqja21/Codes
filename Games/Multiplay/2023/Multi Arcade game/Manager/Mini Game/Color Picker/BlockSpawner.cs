using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField]
    Block blockPrefab;
    GridLayoutGroup gridLayout;

    void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
    }

    public List<Block> SpawnBlocks(int blockCount)
    {
        List<Block> blockList = new List<Block>(blockCount * blockCount);

        // 블록은 2x2, 3x3, 5x5...로 배치된다. 블록의 개수에 따라 셀 갯수를 지정.
        int cellSize = 300 - 50 * (blockCount - 2);
        gridLayout.cellSize = new Vector2(cellSize, cellSize);
        // 가로에 배치되는 갯수
        gridLayout.constraintCount = blockCount;

        // N x N만큼 생성
        for (int i = 0; i < blockCount; i++)
        {
            for (int j = 0; j < blockCount; j++)
            {
                Block block = Instantiate(blockPrefab, gridLayout.transform);
                blockList.Add(block);
            }
        }
        return blockList;
    }
}
