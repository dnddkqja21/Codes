using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    // 컴포넌트
    public TextMeshProUGUI questName;

    public TextMeshProUGUI questIng;

    public InventoryUI inventory;   // 인벤토리    

    public ItemDataBase itemDB; // 아이템 DB


    // 변수
    public int questID; // 진행 중인 퀘스트

    public int questOrder;  // 퀘스트 순서  

    public bool isClear;   // 퀘스트 클리어 여부
        

    // 컨테이너
    Dictionary<int, QuestData> questList;   // 퀘스트 id와 퀘스트 데이터
    

    // 오브젝트
    public GameObject questFrame;
    public GameObject sword;
    public GameObject bow;
    public GameObject wand;

    private void Awake()
    {
        questList = new Dictionary<int, QuestData>();
        SetData();
    }


    void SetData()
    {   // 퀘스트 id는 10씩 증가
        questList.Add(10, new QuestData("전초기지 발견", new int[] { 6000, 4000 }));
        questList.Add(20, new QuestData("선장님과 인사", new int[] { 1000 }));
        questList.Add(30, new QuestData("나무 구하기", new int[] { 1000 }));
        questList.Add(40, new QuestData("막꽁과 인사", new int[] { 1000, 2000 }));
        questList.Add(50, new QuestData("등껍질 구하기", new int[] { 2000 }));
        questList.Add(60, new QuestData("서쪽 탐사", new int[] { 2000 }));
        questList.Add(70, new QuestData("뼈 구하기", new int[] { 2000 }));
        questList.Add(80, new QuestData("알랭과 인사", new int[] { 2000 }));
        questList.Add(90, new QuestData("북쪽 탐사", new int[] { 3000 }));
        questList.Add(100, new QuestData("강철 구하기", new int[] { 3000 }));
        questList.Add(110, new QuestData("강철 전해주기", new int[] { 3000 }));
        questList.Add(120, new QuestData("아딩과 인사", new int[] { 2000 }));
        questList.Add(130, new QuestData("동쪽 탐사", new int[] { 5000 }));
        questList.Add(140, new QuestData("주문서 구하기", new int[] { 5000 }));
        questList.Add(150, new QuestData("알랭에게 도움요청", new int[] { 5000 }));
        questList.Add(160, new QuestData("악의 원흉", new int[] { 3000 }));
        questList.Add(170, new QuestData("마무리", new int[] { 3000 }));
    }
    void Update()
    {
        CountQuestItem();
    }

    public int GetQuestTalkIndex(int _id)   // 퀘스트 번호 + 퀘스트 순서 = 퀘스트 id
    {
        return questID + questOrder;
    }

    public void CheckQuest(int _id) // 순서에 맞게 대화를 진행할 경우에만 퀘스트 순서를 증가시킴
    {
        ControlQuestItem();

        if (_id == questList[questID].npcID[questOrder] && isClear == true)
        {            
            questOrder++;
        }

        if(questOrder == questList[questID].npcID.Length)
        {
            isClear = false;
            NextQuest();
        }
    }

    void NextQuest()    // 새로운 퀘스트를 위해 인덱스를 10씩 증가, 퀘스트 순서는 다시 0으로 초기화
    {
        questID += 10;
        questOrder = 0;
    }

    void CountQuestItem()
    {
        switch(questID)
        {
            case 30:
                // 퀘스트 로그 프레임에 나오는 퀘스트 아이템 갯수 (1 / 3) 표시 
                questIng.text = "[ 나무 " + inventory.FindItemCount("나무") + " / 3개 ]";
                break;

            case 40:
                questIng.text = "[ 막꽁과 인사하기 ]";
                break;

            case 50:
                questIng.text = "[ 등껍질 " + inventory.FindItemCount("등껍질") + " / 3개 ]";
                break;

            case 70:
                questIng.text = "[ 뼈 " + inventory.FindItemCount("뼈") + " / 4개 ]";
                break;

            case 100:
                questIng.text = "[ 강철 " + inventory.FindItemCount("강철") + " / 5개 ]";
                break;

            case 140:
                questIng.text = "[ 주문서 " + inventory.FindItemCount("주문서") + " / 3개 ]";
                break;

            case 170:
                questIng.text = "[ 드래곤을 처치하시오! ]";
                break;


        }
    }

    void ControlQuestItem() // 퀘스트 번호에 따른 퀘스트 아이템 구분
    {
        switch(questID)
        {
            case 10:
                isClear = true;
                break;

            case 20:
                questFrame.SetActive(true);
                questName.text = "< " + questList[questID + 10].questName + " >";   
                isClear = true;
                break;

            case 30:                
                int a = inventory.FindItem("나무", 3);
                if (a != -1)
                {
                    questFrame.SetActive(false);
                    isClear = true;
                    inventory.slots[a].ClearSlot();
                    inventory.AddSlotItem(itemDB.item[3], 1);
                    inventory.AddSlotItem(itemDB.item[12], 5);
                }
                break;

            case 40:
                questFrame.SetActive(true);
                questName.text = "< " + questList[questID+10].questName + " >";
                isClear = true;
                break;

            case 50:                

                int b = inventory.FindItem("등껍질", 3);
                if (b != -1)
                {
                    questFrame.SetActive(false);
                    //Debug.Log("등껍질 찾기완료.");
                    isClear = true;
                    inventory.slots[b].ClearSlot();
                    inventory.AddSlotItem(itemDB.item[1], 1);

                    GameObject tmp = Instantiate(sword);
                    tmp.transform.position = new Vector3(2f, 0.5f, 8f);
                }
                break;

            case 60:
                questFrame.SetActive(true);
                questName.text = "< " + questList[questID + 10].questName + " >";
                isClear = true;
                break;

            case 70:
                int c = inventory.FindItem("뼈", 4);
                if (c != -1)
                {
                    questFrame.SetActive(false);
                    isClear = true;
                    inventory.slots[c].ClearSlot();
                    inventory.AddSlotItem(itemDB.item[0], 1);
                }
                break;

            case 80:                
                isClear = true;                
                break;

            case 90:
                questFrame.SetActive(true);
                questName.text = "< " + questList[questID + 10].questName + " >";
                isClear = true;
                inventory.AddSlotItem(itemDB.item[5], 5);
                break;

            case 100:                
                int d = inventory.FindItem("강철", 5);
                if (d != -1)
                {
                    questFrame.SetActive(false);
                    isClear = true;
                }
                break;

            case 110:
                isClear = true;                 
                break;

            case 120:
                    isClear = true;

                    GameObject tmp2 = Instantiate(bow);
                    tmp2.transform.position = new Vector3(2f, 0.5f, 8f);

                    GameObject tmp3 = Instantiate(wand);
                    tmp3.transform.position = new Vector3(1f, 0.5f, 8f);
                /*
                int e = inventory.FindItem("강철", 5);
                if (e != -1)
                {
                    inventory.slots[e].ClearSlot();
                    inventory.AddSlotItem(itemDB.item[2], 1);
                }
                */
                break;

            case 130:
                questFrame.SetActive(true);
                questName.text = "< " + questList[questID + 10].questName + " >";
                isClear = true;
                break;

            case 140:
                int f = inventory.FindItem("주문서", 3);
                if (f != -1)
                {
                    questFrame.SetActive(false);
                    isClear = true;                    
                }                
                break;

            case 150:
                isClear = true;
                break;

            case 160:
                questFrame.SetActive(true);
                questName.text = "< " + questList[questID].questName + " >";
                int g = inventory.FindItem("주문서", 3);
                if (g != -1)
                {
                    isClear = true;
                    inventory.slots[g].ClearSlot();                    
                }
                break;

            case 170:
                
                break;
        }
    }
}
