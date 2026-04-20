using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseUtil
{
    public static Camera cameramain=Camera.main;
    public static Vector3 GetMouseWorldPos(float zvalue=0)
    {
        Plane plane=new(cameramain.transform.forward,new Vector3(0,0,zvalue));
        Ray ray=cameramain.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray,out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;

    }
}
