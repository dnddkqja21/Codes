using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndShow : MonoBehaviour
{
    [SerializeField]
    Animator ani;

    bool isHide;

    // ���
    public void OnHideAndShow()
    {
        SoundManager.Instance.PlaySFX(SFX.MenuClick);

        isHide = !isHide;
        ani.SetBool("IsHide", isHide);
    }

    // ���� �г�, ��ü �޴� �г�
    
    // ���� ����
    public void OnShow()
    {
        SoundManager.Instance.PlaySFX(SFX.MenuClick);

        ani.SetBool("IsShow", true);
    }

    // ���� ����
    public void OnHide()
    {
        SoundManager.Instance.PlaySFX(SFX.MenuClick);

        ani.SetBool("IsShow", false);
    }
}
