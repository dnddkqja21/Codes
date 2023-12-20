using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int actorNumber;
    public Dictionary<string , object> GetUserData()
    {
        Dictionary<string, object> userData = new Dictionary<string , object>();
        userData.Add("like", "1");
        userData.Add("grade", "브론즈");
        userData.Add("psitnNm", UserData.Instance.avatarData.psitnNm);
        userData.Add("userNm", UserData.Instance.avatarData.userNm);
        return userData;
    }
}
