using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    public string questName;    // ����Ʈ �̸�

    public int[] npcID; // ���Ǿ� id

    public QuestData(string _name, int[] _npc)  // ������
    {
        questName = _name;
        npcID = _npc;
    }
}
