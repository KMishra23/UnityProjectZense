using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public Vector3 worldPosition;
    void Update() 
    {
        Plane plane = new Plane(new Vector3(-27f,-1f,0f), new Vector3(-227,-1f,0f), new Vector3(-27f,-50f,0f));
        float Distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray, out Distance))
        {
            if (worldPosition != ray.GetPoint(Distance))
            {
                worldPosition = ray.GetPoint(Distance);
            }
        }
    }

    public Vector3 CrosshairPos()
    {
        return worldPosition;
    }
}
