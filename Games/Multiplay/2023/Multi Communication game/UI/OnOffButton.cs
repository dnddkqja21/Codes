using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버튼 OnOff제어
/// </summary>

public class OnOffButton : MonoBehaviour
{
    // 시작 시 비활성화
    bool inactiveOnAwake = false;
    // 시작 시 활성화 
    bool activeOnAwake = true;


    // 시작 시 기본 비활성화 상태인 오브젝트 (현재 전체 메뉴 애니메이션으로 바꿈)
    public void ActiveSettingWindow()
    {
        SoundManager.Instance.PlaySFX(SFX.MenuClick);
        inactiveOnAwake = !inactiveOnAwake;
        UIManager.Instance.settingWindow.SetActive(inactiveOnAwake);
    }    

    public void ActiveEmoticon()
    {
        SoundManager.Instance.PlaySFX(SFX.MenuClick);
        inactiveOnAwake = !inactiveOnAwake;
        UIManager.Instance.emoticon.SetActive(inactiveOnAwake);

    }

    // 시작 시 기본 활성화 상태인 오브젝트

    // 현재 애니메이션으로 대체 됨
    public void ActiveChatWindow()
    {
        SoundManager.Instance.PlaySFX(SFX.MenuClick);
        activeOnAwake = !activeOnAwake;
        UIManager.Instance.chatWindow.SetActive(activeOnAwake);
    }

    public void ActiveBubbleChat()
    {
        SoundManager.Instance.PlaySFX(SFX.MenuClick);
        activeOnAwake = !activeOnAwake;
        PhotonManagerWorld.Instance.player.GetComponent<PlayerAttributes>().bubbleAccept = activeOnAwake;
        Debug.Log("버블챗" + activeOnAwake);
    }

    public void ActiveMinimap()
    {
        SoundManager.Instance.PlaySFX(SFX.MenuClick);
        activeOnAwake = !activeOnAwake;
        UIManager.Instance.minimap.SetActive(activeOnAwake);
    }

    // 초기화
    public void InitActive()
    {        
        inactiveOnAwake = !inactiveOnAwake;
    }
}
