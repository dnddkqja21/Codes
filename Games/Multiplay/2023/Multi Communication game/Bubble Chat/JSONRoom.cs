using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONRoom : MonoBehaviour
{
    [System.Serializable]
    public class RoomUser
    {
        public List<UserKey> items;
    }

    [System.Serializable]
    public class UserKey
    {
        public int key;
        public List<string> value;
    }
}
