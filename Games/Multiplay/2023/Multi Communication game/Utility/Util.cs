
using System.Collections.Generic;
using UnityEngine;

 public static class Util 
{
    static public string GetStr(ExitGames.Client.Photon.Hashtable table, string key)
    {
        string customValue = "";
        if (table.TryGetValue(key, out object value))
        {
            customValue = value as string;
        }
        return customValue;
    }


    static public void SaveData(string key , string getValue)
    {
        PlayerPrefs.SetString(key, AESUtil.EncryptString(getValue));
    }

    static public string LoadData(string key)
    {
        if(PlayerPrefs.GetString(key, "").Equals(""))
        {
            return "";
        }

        if (!PlayerPrefs.GetString(key, "").Equals(""))
        {
            return AESUtil.DecryptString(PlayerPrefs.GetString(key, ""));
        }
        else
        {
            return "";
        }
        
    }

    static public Dictionary<int, List<string>> jsonToDictionary(string json)
    {
        JSONRoom.RoomUser serializableData = JsonUtility.FromJson<JSONRoom.RoomUser>(json);

        Dictionary<int, List<string>> data = new Dictionary<int, List<string>>();
        foreach (var kvp in serializableData.items)
        {
            data.Add(kvp.key, kvp.value);
        }

        foreach (var kvp in data)
        {
            Debug.Log("Key: " + kvp.Key + ", Values: " + string.Join(", ", kvp.Value));
        }
        return data;
    }
    static public string dictionaryToJson(Dictionary<int, List<string>> dic)
    {
        JSONRoom.RoomUser serializableData = new JSONRoom.RoomUser();
        serializableData.items = new List<JSONRoom.UserKey>();

        foreach (var kvp in dic)
        {
            JSONRoom.UserKey serializableKvp = new JSONRoom.UserKey();
            serializableKvp.key = kvp.Key;
            serializableKvp.value = kvp.Value;
            serializableData.items.Add(serializableKvp);
        }

        string json = JsonUtility.ToJson(serializableData, true);
        return json;
    }
}
