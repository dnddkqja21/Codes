using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    public string questName;    // 퀘스트 이름

    public int[] npcID; // 엔피씨 id

    public QuestData(string _name, int[] _npc)  // 생성자
    {
        questName = _name;
        npcID = _npc;
    }
}
