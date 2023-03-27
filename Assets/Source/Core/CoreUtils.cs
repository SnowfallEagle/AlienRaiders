using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoreUtils
{
    public static Vector3 ScreenToWorldPosition(Vector3 Position)
    {
        return Camera.main.ScreenToWorldPoint(Position);
    }
}
