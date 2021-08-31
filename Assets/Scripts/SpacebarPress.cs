using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacebarPress : MonoBehaviour
{
    public event EventHandler onSpacePress;
    void Start()
    {
        onSpacePress += Test_Space_Down;
    }

    void Test_Space_Down(object sender, EventArgs e)
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onSpacePress?.Invoke(this, EventArgs.Empty);//the invoke checks if the event has any subscribers
        }   
    }
}
