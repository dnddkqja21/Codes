using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¾Û Á¾·á
/// </summary>

public class QuitApp : MonoBehaviour
{
    public void OnApplicationQuit()
    {
        SoundManager.Instance.PlaySFX(SFX.OpenDoor);

        Application.Quit();
    }
}
