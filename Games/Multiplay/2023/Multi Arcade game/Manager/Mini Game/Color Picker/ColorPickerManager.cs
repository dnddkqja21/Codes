using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ColorPickerManager : MonoBehaviour
{
    static ColorPickerManager instance = null;
    public static ColorPickerManager Instance {  get { return instance; } }    

    [Header("설정")]
    [SerializeField]
    BlockSpawner blockSpawner;
    [SerializeField]
    Color[] colorPallett;    
    [SerializeField]
    [Range(2, 5)]
    int blockCount = 2;

    [SerializeField]
    TextMeshProUGUI scoreText;
    float difficultly = 50;
    int score = 0;

    List<Block> blockList = new List<Block>();
    // 다른 색상(정답) 관리
    Color otherOneColor;
    int otherBlockIndex;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        blockList = blockSpawner.SpawnBlocks(blockCount);
        for (int i = 0; i < blockList.Count; i++)
        {
            blockList[i].SetUp(this);
        }
        //InitGame();
    }

    public void InitGame()
    {
        SoundManager.Instance.PlaySFX(SFX.MiniGameOn);

        foreach (var block in blockList)
        {
            block.gameObject.SetActive(true);
        }
        difficultly = 50;
        SetColor();
        score = 0;
        scoreText.text = "Score : " + score;
    }

    void GameOver()
    {
        SoundManager.Instance.PlaySFX(SFX.MiniGameOff);

        foreach (var block in blockList)
        {
            block.gameObject.SetActive(false);
        }
        score = 0;
        scoreText.text = "Score : " + score;
        difficultly = 0;
    }

    void SetColor()
    {
        // 정답을 맞출 때마다 색상이 다른 블록들과 점점 비슷해진다.
        difficultly *= 0.92f;

        // 다른 블록들의 색상
        Color currentColor = colorPallett[Random.Range(0, colorPallett.Length)];

        // 정답 블록의 색상
        float diff = (1.0f / 255.0f) * difficultly;
        otherOneColor = new Color(currentColor.r - diff, currentColor.g - diff, currentColor.b - diff);

        otherBlockIndex = Random.Range(0, blockList.Count);
        //Debug.Log(otherBlockIndex);

        // 블록 색상 지정
        for (int i = 0; i < blockList.Count; i++)
        {
            if (i == otherBlockIndex)
            {
                blockList[i].color = otherOneColor;
            }
            else
            {
                blockList[i].color = currentColor;
            }
        }
    }

    public void CheckBlock(Color color)
    {
        if (blockList[otherBlockIndex].color == color)
        {
            SoundManager.Instance.PlaySFX(SFX.Success);
            SetColor();
            score += 10;
            scoreText.text = "Score : " + score;
        }
        else
        {
            SoundManager.Instance.PlaySFX(SFX.Fail);
            // 점수 저장, 팝업 재실행 또는 취소 묻기
            string text = LocalizationManager.Instance.ColorPoint(score);
            PopupManager.Instance.ShowTwoButtnPopup(false, text, InitGame, null, () => UIManagerWorld.Instance.colorPicker.SetBool("isShow", false));
            TryInsertRank(score);
            GameOver();
        }
    }

    async void TryInsertRank(int score)
    {
        await Task.Run(() =>
        {
            RankManager.Instance.InsertGameRecord(Config.Rank_Uuid_Color, Config.Record_Color, score);
            RankManager.Instance.InsertGameRecord(Config.Rank_Uuid_Color_HOF, Config.Record_Color_HOF, score);
        });
    }
}
