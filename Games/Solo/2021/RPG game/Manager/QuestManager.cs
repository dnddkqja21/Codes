using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    // ������Ʈ
    public TextMeshProUGUI questName;

    public TextMeshProUGUI questIng;

    public InventoryUI inventory;   // �κ��丮    

    public ItemDataBase itemDB; // ������ DB


    // ����
    public int questID; // ���� ���� ����Ʈ

    public int questOrder;  // ����Ʈ ����  

    public bool isClear;   // ����Ʈ Ŭ���� ����
        

    // �����̳�
    Dictionary<int, QuestData> questList;   // ����Ʈ id�� ����Ʈ ������
    

    // ������Ʈ
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
    {   // ����Ʈ id�� 10�� ����
        questList.Add(10, new QuestData("���ʱ��� �߰�", new int[] { 6000, 4000 }));
        questList.Add(20, new QuestData("����԰� �λ�", new int[] { 1000 }));
        questList.Add(30, new QuestData("���� ���ϱ�", new int[] { 1000 }));
        questList.Add(40, new QuestData("���ǰ� �λ�", new int[] { 1000, 2000 }));
        questList.Add(50, new QuestData("��� ���ϱ�", new int[] { 2000 }));
        questList.Add(60, new QuestData("���� Ž��", new int[] { 2000 }));
        questList.Add(70, new QuestData("�� ���ϱ�", new int[] { 2000 }));
        questList.Add(80, new QuestData("�˷��� �λ�", new int[] { 2000 }));
        questList.Add(90, new QuestData("���� Ž��", new int[] { 3000 }));
        questList.Add(100, new QuestData("��ö ���ϱ�", new int[] { 3000 }));
        questList.Add(110, new QuestData("��ö �����ֱ�", new int[] { 3000 }));
        questList.Add(120, new QuestData("�Ƶ��� �λ�", new int[] { 2000 }));
        questList.Add(130, new QuestData("���� Ž��", new int[] { 5000 }));
        questList.Add(140, new QuestData("�ֹ��� ���ϱ�", new int[] { 5000 }));
        questList.Add(150, new QuestData("�˷����� �����û", new int[] { 5000 }));
        questList.Add(160, new QuestData("���� ����", new int[] { 3000 }));
        questList.Add(170, new QuestData("������", new int[] { 3000 }));
    }
    void Update()
    {
        CountQuestItem();
    }

    public int GetQuestTalkIndex(int _id)   // ����Ʈ ��ȣ + ����Ʈ ���� = ����Ʈ id
    {
        return questID + questOrder;
    }

    public void CheckQuest(int _id) // ������ �°� ��ȭ�� ������ ��쿡�� ����Ʈ ������ ������Ŵ
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

    void NextQuest()    // ���ο� ����Ʈ�� ���� �ε����� 10�� ����, ����Ʈ ������ �ٽ� 0���� �ʱ�ȭ
    {
        questID += 10;
        questOrder = 0;
    }

    void CountQuestItem()
    {
        switch(questID)
        {
            case 30:
                // ����Ʈ �α� �����ӿ� ������ ����Ʈ ������ ���� (1 / 3) ǥ�� 
                questIng.text = "[ ���� " + inventory.FindItemCount("����") + " / 3�� ]";
                break;

            case 40:
                questIng.text = "[ ���ǰ� �λ��ϱ� ]";
                break;

            case 50:
                questIng.text = "[ ��� " + inventory.FindItemCount("���") + " / 3�� ]";
                break;

            case 70:
                questIng.text = "[ �� " + inventory.FindItemCount("��") + " / 4�� ]";
                break;

            case 100:
                questIng.text = "[ ��ö " + inventory.FindItemCount("��ö") + " / 5�� ]";
                break;

            case 140:
                questIng.text = "[ �ֹ��� " + inventory.FindItemCount("�ֹ���") + " / 3�� ]";
                break;

            case 170:
                questIng.text = "[ �巡���� óġ�Ͻÿ�! ]";
                break;


        }
    }

    void ControlQuestItem() // ����Ʈ ��ȣ�� ���� ����Ʈ ������ ����
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
                int a = inventory.FindItem("����", 3);
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

                int b = inventory.FindItem("���", 3);
                if (b != -1)
                {
                    questFrame.SetActive(false);
                    //Debug.Log("��� ã��Ϸ�.");
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
                int c = inventory.FindItem("��", 4);
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
                int d = inventory.FindItem("��ö", 5);
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
                int e = inventory.FindItem("��ö", 5);
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
                int f = inventory.FindItem("�ֹ���", 3);
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
                int g = inventory.FindItem("�ֹ���", 3);
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
