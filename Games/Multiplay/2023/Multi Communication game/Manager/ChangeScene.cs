using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 씬 체인지
/// 포톤 씬 로드를 하였을 때 페이드인 아웃이 되지 않기 때문에 따로 분리해야 할 것.
/// 멀티에선 룸을 떠나고 입장하는 개념이기 때문에 함수들 수정 필요!
/// </summary>

public class ChangeScene : MonoBehaviourPunCallbacks
{
    [Header("페이드 인 이미지")]   
    [SerializeField] 
    Image background;
    [Header("페이드 인 대기시간")] 
    [SerializeField] 
    float waitTime = 0.5f;
    [Header("페이드 인 속도")]
    [SerializeField]
    float fadeInSpeed = 0.65f;

    // 트리거 충돌 시 코루틴
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        // 로컬 플레이어일 때 UI띄우기
        if (!other.transform.GetComponent<PhotonView>().IsMine)
            return;

        // 게스트 입장 불가. (웹2개, 마이룸, 행정지원실만 입장가능)
        if(other.transform.GetComponent<PlayerAttributes>().isGuest)
        {
            string objName = gameObject.name;
            if (objName.Equals(EnumToData.Instance.BuildingNameToString(BuildingName.ClassRoom)) ||
                objName.Equals(EnumToData.Instance.BuildingNameToString(BuildingName.StudyRoom)) ||
                objName.Equals(EnumToData.Instance.BuildingNameToString(BuildingName.VODCenter)) ||
                objName.Equals(EnumToData.Instance.BuildingNameToString(BuildingName.SeminarRoom)) ||
                objName.Equals(EnumToData.Instance.BuildingNameToString(BuildingName.ProfessorOffice)) ||
                objName.Equals(EnumToData.Instance.BuildingNameToString(BuildingName.ExperienceCenter)))
            {
                Locale curLang = LocalizationSettings.SelectedLocale;
                string text = LocalizationSettings.StringDatabase.GetLocalizedString("Table 01", "게스트 제한", curLang);
                PopupManager.Instance.ShowOneButtnPopup(text);
                return;
            }
        }

        // 입장 UI띄우기
        UIManager.Instance.changeScenePopup.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        UIManager.Instance.changeScenePopup.SetActive(false);
    }

    // 씬 전환 효과 코루틴 (포톤과 동시에 못함, 따로 해야 함)
    IEnumerator LoadSceneAndFadeIn(SceneName sceneName)
    {
        Color temp = background.color;

        temp.a = 0;
        background.color = temp;

        yield return new WaitForSeconds(waitTime);

        while (background.color.a < 1)
        {
            temp.a += Time.deltaTime * fadeInSpeed;
            background.color = temp;
            yield return null;
        }
        temp.a = 1;
        background.color = temp;

        LoadScene(sceneName);
    }

    // 로드 씬
    void LoadScene(SceneName sceneName)
    {        
        SceneManager.LoadScene(EnumToData.Instance.SceneNameToString(sceneName));
    }    
}
