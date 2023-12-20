using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//<summary>
// 미로 생성기
//</summary>

public class MazeSpawnManager : MonoBehaviour 
{
    static MazeSpawnManager instance = null;
    public static MazeSpawnManager Instance { get { return instance; } }

    public enum MazeGenerationAlgorithm
	{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

    [Header("메이즈 속성")]
    [SerializeField] 
    MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
    [SerializeField] 
    bool FullRandom = false;
	[SerializeField]
    int RandomSeed = 12345;
	[SerializeField]
    int Rows = 5;
	[SerializeField]
    int Columns = 5;
	[SerializeField]
    float CellWidth = 5;
	[SerializeField]
    float CellHeight = 5;
	[SerializeField]
    bool AddGaps = true;
	BasicMazeGenerator mMazeGenerator = null;

    [Header("오브젝트풀")]
    [SerializeField]
    GameObject[] prefabs;
    List<GameObject>[] pools;
    Transform objectPool;
    List<DissolveObject> dissolveObjects = new List<DissolveObject>();
    Vector3 mazePos = new Vector3 (12.97f, 0, -5.04f);
    int poolSize = 10;
	int coinCount;
	public float clearTime;

    void Awake()
    {
        if (instance == null)
            instance = this;

        // 프리팹 종류 수에 따라 배열 크기 설정
        pools = new List<GameObject>[prefabs.Length];
        objectPool = new GameObject("ObjectPool").transform;        
        objectPool.transform.parent = transform;

        for (int i = 0; i < prefabs.Length; i++)
        {
            pools[i] = new List<GameObject>();
            for (int j = 0; j < poolSize; j++)
            {
                GameObject obj = Instantiate(prefabs[i]);
                obj.SetActive(false);
                obj.transform.SetParent(objectPool);
                pools[i].Add(obj);

                // 생성, 소멸 이펙트 스크립트
                if (obj.GetComponent<DissolveObject>() != null)
                {
                    dissolveObjects.Add(obj.GetComponent<DissolveObject>());
                }
            }
        }
    }

    public void CreateMaze()
    {
        SoundManager.Instance.PlaySFX(SFX.MiniGameOn);

        if (!FullRandom)
        {
            Random.seed = RandomSeed;
        }

        switch (Algorithm)
        {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveTree:
                mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RandomTree:
                mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.OldestTree:
                mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveDivision:
                mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                break;
        }

        mMazeGenerator.GenerateMaze();
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
                GameObject tmp;                
                if (cell.WallRight)
                {
                    tmp = GetFromPool(0, new Vector3(x + CellWidth / 2, 0, z), Quaternion.Euler(0, 90, 0));
                    tmp.transform.parent = objectPool.transform;
                }
                if (cell.WallFront)
                {
                    tmp = GetFromPool(0, new Vector3(x, 0, z + CellHeight / 2), Quaternion.Euler(0, 0, 0));
                    tmp.transform.parent = objectPool.transform;
                }
                if (cell.WallLeft)
                {
                    tmp = GetFromPool(0, new Vector3(x - CellWidth / 2, 0, z), Quaternion.Euler(0, 270, 0));
                    tmp.transform.parent = objectPool.transform;
                }
                if (cell.WallBack)
                {
                    tmp = GetFromPool(0, new Vector3(x, 0, z - CellHeight / 2), Quaternion.Euler(0, 180, 0));
                    tmp.transform.parent = objectPool.transform;
                }
                if (cell.IsGoal)
                {
                    tmp = GetFromPool(1, new Vector3(x, 0.5f, z), Quaternion.Euler(0, 0, 0));
                    tmp.transform.parent = objectPool.transform;
                    coinCount++;
                }
            }
        }
        
        for (int row = 0; row < Rows + 1; row++)
        {
            for (int column = 0; column < Columns + 1; column++)
            {
                float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                GameObject tmp = GetFromPool(2, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity);
                tmp.transform.parent = objectPool.transform;
            }
        }
        objectPool.transform.position = mazePos;
        StartCoroutine(CalculateTime());
    }

    // 풀에서 가져옴
    public GameObject GetFromPool(int prefabIndex, Vector3 position, Quaternion rotation)
    {
        if (prefabIndex >= 0 && prefabIndex < prefabs.Length)
        {
            foreach (GameObject obj in pools[prefabIndex])
            {
                if (!obj.activeSelf)
                {
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;
                    obj.SetActive(true);
                    return obj;
                }
            }

            // 풀에서 사용 가능한 오브젝트가 없으면 새로 생성
            GameObject newObj = Instantiate(prefabs[prefabIndex], position, rotation);
            newObj.transform.SetParent(objectPool);
            pools[prefabIndex].Add(newObj);
            
            if (newObj.GetComponent<DissolveObject>() != null)
            {
                dissolveObjects.Add(newObj.GetComponent<DissolveObject>());
            }
            return newObj;
        }

        return null; // 잘못된 프리팹 인덱스인 경우 null 반환
    }

    // 풀로 반환
    IEnumerator ReturnToPool()
    {
        SoundManager.Instance.PlaySFX(SFX.MiniGameOff);

        foreach (var item in dissolveObjects)
        {
            item.DoDissolve();
        }

        yield return new WaitForSeconds(0.3f);
        string text = LocalizationManager.Instance.MazePoint();
        PopupManager.Instance.ShowTwoButtnPopup(false, text, CreateMaze);

        yield return new WaitForSeconds(1.7f);
        objectPool.transform.position = Vector3.zero;
    }

    public void CountCoin()
	{
        SoundManager.Instance.PlaySFX(SFX.Coin);
        coinCount--;
		if(coinCount == 0) 
		{
            StartCoroutine(ReturnToPool());
            TryInsertRank();
        }
	}

    async void TryInsertRank()
    {
        await Task.Run(() =>
        {
            RankManager.Instance.InsertGameRecord(Config.Rank_Uuid_Maze, Config.Record_Maze, clearTime);
            RankManager.Instance.InsertGameRecord(Config.Rank_Uuid_Maze_HOF, Config.Record_Maze_HOF, clearTime);
        });
    }

    IEnumerator CalculateTime()
	{
		clearTime = 0;
		while(coinCount > 0)
		{
			clearTime += Time.deltaTime;
			yield return null;
		}
	}
}
