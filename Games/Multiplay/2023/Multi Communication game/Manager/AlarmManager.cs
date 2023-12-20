using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlarmManager : MonoBehaviour
{
    static AlarmManager instance = null;
    public static AlarmManager Instance { get { return instance; } }

    // 알람 리스트 관리
    List<Dictionary<string, object>> alarmList = new List<Dictionary<string, object>>();

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        AlarmListApi();
    }

    public void AlarmListApi()
    {
        Dictionary<string, object> requestData = new Dictionary<string, object>();
        requestData.Add("userSeq", UserData.Instance.avatarData.userSeq);

        StartCoroutine(HttpNetwork.Instance.PostSendNetwork(requestData, URL_CONFIG.ALARM_LIST, (data) =>
        {
            Debug.Log("gusanaglkyo == call");
            if (data != null)
            {
                // 기존 리스트 제거 후 리스트에 추가
                alarmList.Clear();
                alarmList = (List<Dictionary<string, object>>)data;

                // 알람 종 스프라이트 변경
                bool isCheck = true;
                foreach (Dictionary<string, object> alarmData in alarmList)
                {
                    if ((string)alarmData["checkYn"] == "N")
                    {
                        isCheck = false;
                        break;
                    }
                }
                UIManager.Instance.alarmButton.image.sprite = isCheck ? UIManager.Instance.alarmSprites[0] : UIManager.Instance.alarmSprites[1];

                // 이전 알람 삭제
                List<Transform> oldObj = new List<Transform>();
                int childCount = UIManager.Instance.contentAlarm.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    oldObj.Add(UIManager.Instance.contentAlarm.GetChild(i));
                }
                for (int i = 0; i < oldObj.Count; i++)
                {
                    Destroy(oldObj[i].gameObject);
                }
                oldObj.Clear();

                // 알림이 하나도 없다면 알림없음 이미지 활성화
                if(alarmList.Count == 0)
                {
                    UIManager.Instance.emptyAlarmImage.SetActive(true);
                }

                foreach (Dictionary<string, object> alarmData in alarmList)
                {
                    // 알람 생성            
                    GameObject alarmObj = Instantiate(UIManager.Instance.alarmPrefab,
                                                        UIManager.Instance.contentAlarm);

                    // 생성 시 알람 obj색상 및 이미지 변경 (읽음 - 투명으로)
                    if ((string)alarmData["checkYn"] == "Y")
                    {
                        Debug.Log("읽었음? : " + (string)alarmData["checkYn"]);
                        Color alpha = alarmObj.GetComponent<Image>().color;
                        alpha.a = 0.35f;
                        alarmObj.GetComponent<Image>().color = alpha;

                        alarmObj.transform.GetChild(0).GetComponent<Image>().sprite =
                                                UIManager.Instance.alarmCheckSprites[1];
                    }
                    else
                    {
                        alarmObj.transform.GetChild(0).GetComponent<Image>().sprite =
                                                UIManager.Instance.alarmCheckSprites[0];
                    }

                    TextMeshProUGUI[] texts = alarmObj.GetComponentsInChildren<TextMeshProUGUI>();

                    if(alarmData["alarmTypeNm"] != null)
                    {
                        texts[0].text = (string)alarmData["alarmTypeNm"];
                        if (texts[0].text.Length > 9)
                        {
                            texts[0].text = texts[0].text.Substring(0, 8) + "...";
                        }
                    }
                    if(alarmData["msg"] != null)
                    {
                        texts[1].text = (string)alarmData["msg"];
                        if (texts[1].text.Length > 9)
                        {
                            texts[1].text = texts[1].text.Substring(0, 8) + "...";
                        }
                    }                    

                    // 알람 오브젝트의 버튼 눌렀을 시 행위
                    alarmObj.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        UIManager.Instance.bgDetails.GetComponent<HideAndShow>().OnHideAndShow();

                        // 버튼을 누를 때도 알파처리, 이미지 변경해야 함.
                        Color alpha = alarmObj.GetComponent<Image>().color;
                        alpha.a = 0.35f;
                        alarmObj.GetComponent<Image>().color = alpha;
                        alarmObj.transform.GetChild(0).GetComponent<Image>().sprite =
                                                UIManager.Instance.alarmCheckSprites[1];

                        // 체크 api 호출 (알람 상태를 '읽었다(Y)'로 바꿔줘)
                        Dictionary<string, object> requestData = new Dictionary<string, object>();
                        requestData.Add("alarmSeq", alarmData["alarmSeq"]);
                        requestData.Add("userSeq", UserData.Instance.avatarData.userSeq);
                        requestData.Add("checkYn", "Y");
                        StartCoroutine(HttpNetwork.Instance.PostSendNetwork(requestData, URL_CONFIG.ALARM_CHECK, (data) =>
                        {
                        }));

                        if(alarmData["alarmTypeNm"] != null)
                        {
                            UIManager.Instance.detailsTitle.text = (string)alarmData["alarmTypeNm"];
                        }
                        if(alarmData["msg"] != null)
                        {
                            UIManager.Instance.detailsDesc.text = (string)alarmData["msg"];
                        }
                    });
                }
            }
        }));
    }
}
