using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_ID : MonoBehaviour
{
    public Texture2D cursorNormal;
    public Texture2D cursorNpc;

    public int id;
    public bool isNPC;

    private void OnMouseEnter()
    {
        Cursor.SetCursor(cursorNpc, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.ForceSoftware);
    }
}
