using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangeScene : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(PhotonManagerLobby.Instance.ChangeSceneToInterior);
    }
}
