using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPellet : MonoBehaviour
{
    public RigidbodyPlayer pl;
    public LayerMask pLayer;

    private void Update()
    {
        if (Physics.OverlapSphere(transform.position, 1f, pLayer).Length > 0)
        {
            pl.hitJumpPellet();
            Destroy(gameObject);
        }
    }
}
