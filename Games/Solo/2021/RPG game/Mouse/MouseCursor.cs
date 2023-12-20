using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] Texture2D cursorNormal;
    void Start()
    {
        Cursor.SetCursor(cursorNormal, Vector2.zero, CursorMode.ForceSoftware);
    }    
}
