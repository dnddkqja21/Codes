using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam_0419 : MonoBehaviour
{
    public Transform camArm;
    public Transform cam;

    public Transform player;
    public float camSpeed;
    float mouseX;
    float mouseY;
    float wheel;

    float minZoom = -4f;
    float maxZoom = -20f;
    
    void LateUpdate()
    {
        Move();
        Zoom();
        FollowPlayer();
    }

    void Move()
    {
        if(Input.GetMouseButton(1))
        {
            mouseX += Input.GetAxis("Mouse X");
            mouseY += Input.GetAxis("Mouse Y") * -1;

            camArm.rotation = Quaternion.Euler(new Vector3(camArm.rotation.x + mouseY, camArm.rotation.y + mouseX, 0) * camSpeed);
        }
    }

    void Zoom()
    {
        wheel += Input.GetAxis("Mouse ScrollWheel");
        if(wheel >= minZoom)
        {
            wheel = minZoom;
        }

        if(wheel <= maxZoom)
        {
            wheel = maxZoom;
        }
        cam.localPosition = new Vector3(0, 0, wheel) * 2f;
    }

    void FollowPlayer()
    {
        camArm.position = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);
    }
}
