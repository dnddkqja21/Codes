using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Conversation : MonoBehaviour
{
    public ChatEffect description;

    public Animator textPanel;

    public LinesManager linesManager;

    public QuestManager questManager;

    public bool isTalk;

    public int linesIndex;    

    public void Talk(Transform _npc)
    {        
        isTalk = true;
        NPC_ID npcID = _npc.gameObject.GetComponent<NPC_ID>();  // ���޹��� npc�� ������ ������
        StartTalking(npcID.id, npcID.isNPC);

        textPanel.SetBool("isShow", isTalk);
    }

    void StartTalking(int _id, bool _isNPC)
    {
        int questTalkIndex = 0;
        string linesData = "";

        if (description.isChatting)
        {
            description.SetMessage("");
            return;
        }

        else
        {
            questTalkIndex = questManager.GetQuestTalkIndex(_id);

            // ���̵� + ����Ʈ��ũ �ε����� �޴´�.
            linesData = linesManager.GetTalk(_id + questTalkIndex, linesIndex);
        }

        if(linesData == null)
        {
            isTalk = false;
            linesIndex = 0; // ��ȭ�� ������ �ٽ� 0���� �ʱ�ȭ
            questManager.CheckQuest(_id);  // ��ȭ�� ������ ����Ʈ ���� ++
            return;
        }

        if(_isNPC)
        {
            description.SetMessage(linesData);
        }

        else
        {
            description.SetMessage(linesData);
        }

        isTalk = true;

        linesIndex++;   // ��� �ε��� ����
    }
}
