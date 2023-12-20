using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam_Player_New : MonoBehaviour
{
    public Cam_Player_Controller_New player;

    float oldX;
    float oldY;
    float oldZ;

    float deltaX;
    float deltaY;
    float deltaZ;

    void Start()
    {
        oldX = player.transform.position.x;
        oldY = player.transform.position.y;
        oldZ = player.transform.position.z;
    }

    private void Update()
    {
        deltaX = player.transform.position.x - oldX;
        deltaY = player.transform.position.x - oldY;
        deltaZ = player.transform.position.x - oldZ;

        oldX = player.transform.position.x;
        oldY = player.transform.position.y;
        oldZ = player.transform.position.z;
    }
    void LateUpdate()
    {
        
        if (deltaX == 0 || deltaY == 0 || deltaZ == 0)
        {
            transform.position = transform.position;
        }
    }
}
