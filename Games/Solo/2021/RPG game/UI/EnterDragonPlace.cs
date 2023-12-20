using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnterDragonPlace : MonoBehaviour
{
    Cam_Player_Controller_New playerBox;

    public GameObject loading;

    public NavMeshAgent player;

    Vector3 dragonPlace = new Vector3(0, 0, 209f);

    private void Start()
    {
        playerBox = FindObjectOfType<Cam_Player_Controller_New>();
    }

    public void EnterDragon()
    {
        player.enabled = false;
        playerBox.transform.position = dragonPlace;
        player.enabled = true;
        loading.SetActive(true);
    }
}
