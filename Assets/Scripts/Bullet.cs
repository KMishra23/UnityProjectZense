using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 shootDirection;
    [SerializeField] float bulletSpeed = 10f;
    public void setup(Vector3 shootDirection)
    {
        this.shootDirection = shootDirection;
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        transform.position += shootDirection * bulletSpeed * Time.deltaTime;
    }
}
