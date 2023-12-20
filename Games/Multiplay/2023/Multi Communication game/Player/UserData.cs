using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 유저 데이터
/// </summary>
public class UserData 
{
    static UserData instance = null;
    public static UserData Instance { get { if (instance == null) { instance = new UserData(); } return instance; } }

    public AvatarData avatarData = new AvatarData();
}

public class AvatarData
{
    Dictionary<string, object> loginUserDic = new Dictionary<string, object>();
    // test
    public Dictionary<string, object> GetLoginUser()
    {
        return loginUserDic;
    }
    public void SetFromDictionary(Dictionary<string, object> data)
    {
        loginUserDic = data;
        userSeq = (string)data["userSeq"];
        userEmail = (string)data["userEmail"];
        userNm = (string)data["userNm"];
        userNcnm = (string)data["userNcnm"];
        userAuthor = (string)data["userAuthor"];
        userAuthorReqst = (string)data["userAuthorReqst"];
        useAt = (string)data["useAt"];
        psitnNm = (string)data["psitnNm"];
        userInnb = (string)data["userInnb"];
        mbtlnum = (string)data["mbtlnum"];
        brthdy = (string)data["brthdy"];
        sexdstn = (string)data["sexdstn"];
        atnlcSbjectNm = (string)data["atnlcSbjectNm"];
        zip = (string)data["zip"];
        mainAdres = (string)data["mainAdres"];
        detailAdres = (string)data["detailAdres"];
        profsrHist = (string)data["profsrHist"];
        proofImageCn = (string)data["proofImageCn"];
        proflImageCn = (string)data["proflImageCn"];
        proflColor = (string)data["proflColor"];
        lang = (string)data["lang"];
        qestnCode = (string)data["qestnCode"];
        qestnRspns = (string)data["qestnRspns"];
        appOs = (string)data["appOs"];
        deviceToken = (string)data["deviceToken"];
        registId = (string)data["registId"];
        registDt = (string)data["registDt"];
        updtId = (string)data["updtId"];
        updtDt = (string)data["updtDt"];
        accessToken = (string)data["accessToken"];
        gradeNm = (string)data["gradeNm"];
    }

    public void ClearUserData()
    {
        loginUserDic = null;
        userSeq = string.Empty;
        userEmail = string.Empty;
        userNm = string.Empty;
        userNcnm = string.Empty;
        userAuthor = string.Empty;
        userAuthorReqst = string.Empty;
        useAt = string.Empty;
        psitnNm = string.Empty;
        userInnb = string.Empty;
        mbtlnum = string.Empty;
        brthdy = string.Empty;
        sexdstn = string.Empty;
        atnlcSbjectNm = string.Empty;
        zip = string.Empty;
        mainAdres = string.Empty;
        detailAdres = string.Empty;
        profsrHist = string.Empty;
        proofImageCn = string.Empty;
        proflImageCn = string.Empty;
        proflColor = string.Empty;
        lang = string.Empty;
        qestnCode = string.Empty;
        qestnRspns = string.Empty;
        appOs = string.Empty;
        deviceToken = string.Empty;
        registId = string.Empty;
        registDt = string.Empty;
        updtId = string.Empty;
        updtDt = string.Empty;
        accessToken = string.Empty;
        gradeNm = string.Empty;
    }

    public string userSeq;
    public string userEmail;
    public string userNm;
    public string userNcnm;
    public string userAuthor;
    public string userAuthorReqst;
    public string useAt;
    public string psitnNm;
    public string userInnb;
    public string mbtlnum;
    public string brthdy;
    public string sexdstn;
    public string atnlcSbjectNm;
    public string zip;
    public string mainAdres;
    public string detailAdres;
    public string profsrHist;
    public string proofImageCn;
    public string proflImageCn;
    public string proflColor;
    public string lang;
    public string qestnCode;
    public string qestnRspns;
    public string appOs;
    public string deviceToken;
    public string registId;
    public string registDt;
    public string updtId;
    public string updtDt;
    public string accessToken;    
    public string gradeNm;
    // test
    public string bubbleAccept = "true";
}
