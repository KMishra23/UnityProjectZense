using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPelletHit : MonoBehaviour
{
    public RigidbodyPlayer pl;
    public LayerMask pLayer;

    private void Update()
    {
        if(Physics.OverlapSphere(transform.position, 0.3f, pLayer).Length > 0)
        {
            pl.hitPellet();
            Destroy(gameObject);
        }
    }
}
