using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndShow : MonoBehaviour
{
    [SerializeField]
    Animator ani;

    bool isHide;

    // 토글
    public void OnHideAndShow()
    {
        SoundManager.Instance.PlaySFX(SFX.MenuClick);

        isHide = !isHide;
        ani.SetBool("IsHide", isHide);
    }

    // 종료 패널, 전체 메뉴 패널
    
    // 좌측 보임
    public void OnShow()
    {
        SoundManager.Instance.PlaySFX(SFX.MenuClick);

        ani.SetBool("IsShow", true);
    }

    // 우측 숨김
    public void OnHide()
    {
        SoundManager.Instance.PlaySFX(SFX.MenuClick);

        ani.SetBool("IsShow", false);
    }
}
