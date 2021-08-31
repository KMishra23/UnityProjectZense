using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onMouseClicl : MonoBehaviour
{
    public event EventHandler onLeftMouseClick;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            onLeftMouseClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
