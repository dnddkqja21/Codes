using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManagerInterior : MonoBehaviour
{
    void Start()
    {
        PhotonManagerLobby.Instance.ConnectToPhoton();
    }    
}
