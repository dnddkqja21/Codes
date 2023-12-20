using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Vector3 STICKDIR
    {
        get { return stickDir.normalized; }
    }

    public bool ISCLICK
    {
        get { return isClick; }
    }

    public bool ISMOVING
    {
        get { return isMoving; }
    }

    public Image stick;

    Vector3 center;

    float radius;

    Vector3 stickDir;

    //public Animator ani;

    public Cam_Player_Controller_New player;

    bool isClick = false;

    bool isMoving = false;

    public enum JoystickType { Move, Rotate}

    public JoystickType joystickType;

    public float rotSpeed = 0.2f;

    Image BG;

    void Start()
    {
        center = transform.position;
        radius = GetComponent<RectTransform>().sizeDelta.x * 0.5f;
        stickDir = Vector3.zero;
        BG = GetComponent<Image>();        
    }

    private void Update()
    {
        if(isClick)
        {
            InputControlVector();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        stick.transform.position = center;
        stickDir = Vector3.zero;

        //ani.SetTrigger("toIdle");
        //ani.SetBool("isWalk", false);
        isClick = false;
        isMoving = false;
        switch(joystickType)
        {
            case JoystickType.Move:
                player.Move(Vector2.zero);
                break;

            case JoystickType.Rotate:
                break;
        }
        Color tmp = BG.color;
        tmp.a = 0.5f;
        BG.color = tmp;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        stick.transform.position = eventData.position;
        Vector3 endPos = eventData.position;
        stickDir = endPos - center;

        //ani.SetTrigger("toWalk");
        //ani.SetBool("isWalk", true);
        isClick = true;
        isMoving = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 endPos = eventData.position;
        stickDir = endPos - center;
        float dis = Vector3.Distance(center, endPos);
        if(dis <= radius)
        {
            stick.transform.position = center + stickDir.normalized * dis;
        }
        else
        {
            stick.transform.position = center + stickDir.normalized * radius;
        }
        isClick = true;
        isMoving = true;

        Color tmp = BG.color;
        tmp.a = 0.9f;
        BG.color = tmp;
    }

    void InputControlVector()
    {
        switch(joystickType)
        {
            case JoystickType.Move :
                player.Move(STICKDIR);
                break;

            case JoystickType.Rotate:
                player.LookAround(STICKDIR * rotSpeed);
                break;
        }        
    }
}
