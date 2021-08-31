using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform bullet;
    public Transform bullet_spawn;

    void Start()
    {
        onMouseClicl onMouseLeftDown = GetComponent<onMouseClicl>();
        onMouseLeftDown.onLeftMouseClick += OnMouseLeftDown_onLeftMouseClick;
    }

    private void OnMouseLeftDown_onLeftMouseClick(object sender, System.EventArgs e)
    {
        Transform bullet_clone = Instantiate(bullet, bullet_spawn.position, Quaternion.identity);

        Vector3 shootDirection = bullet_spawn.position.normalized;
        bullet_clone.GetComponent<Bullet>().setup(shootDirection);
    }

}
