using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static Vector3 ReturnMousePosition2D()
    {
        Vector3 mousePosition2D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(mousePosition2D.x, mousePosition2D.y, 0);
    }
}
