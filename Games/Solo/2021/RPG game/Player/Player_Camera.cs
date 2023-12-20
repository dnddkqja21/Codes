using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
    // ���� ��ġ�� �ʱ�ȭ
    public Vector3 oldPos = Vector3.zero;   

    public GameManager_PF gameManager;   // ���� �Ŵ����� �÷��̾ �˰������Ƿ� ī�޶� ���Ӹ޴����� �˰������� �ȴ�.

    float rotateSpeed = 300f;

    public Player_PF player;

    // ī�޶� �� �� �ƿ�
    float zoomSpeed = 3f;
    float zoomMax = 5f;
    float zoomMin = 15f;
   
    void Start()
    {       
        // ���� ��ġ�� ���� ������Ʈ ��ġ�� ����
        // ���� �ÿ��� ���� ��ġ�� ĳ���� ���� ��ġ�� ����.         
        if(player != null)
        {
            oldPos = player.transform.position;
        }

    }    

    void LateUpdate()
    {
        CameraZoom(); 

        if (player != null)
        {
            Vector3 delta = player.transform.position - oldPos;

            transform.position += delta;

            oldPos = player.transform.position;

            float x1 = Input.GetAxis("Mouse X");

            if (Input.GetMouseButton(1))
            {
                transform.RotateAround(player.transform.position, Vector3.up, x1 * Time.deltaTime * rotateSpeed);  
            }
        }        
    }

    void CameraZoom()
    {
        float zoomDir = Input.GetAxis("Mouse ScrollWheel");

        // ī�޶��� y�� ���ξƿ� �Ѱ�ġ ����
        if (transform.position.y <= zoomMax && zoomDir > 0)
            return;
        if (transform.position.y >= zoomMin && zoomDir < 0)
            return;

        transform.position += transform.forward * zoomDir * zoomSpeed;
    }
}
