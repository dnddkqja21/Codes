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
        NPC_ID npcID = _npc.gameObject.GetComponent<NPC_ID>();  // 전달받은 npc의 정보를 가져옴
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

            // 아이디 + 퀘스트토크 인덱스를 받는다.
            linesData = linesManager.GetTalk(_id + questTalkIndex, linesIndex);
        }

        if(linesData == null)
        {
            isTalk = false;
            linesIndex = 0; // 대화가 끝나면 다시 0으로 초기화
            questManager.CheckQuest(_id);  // 대화가 끝나면 퀘스트 순서 ++
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

        linesIndex++;   // 대사 인덱스 증가
    }
}
