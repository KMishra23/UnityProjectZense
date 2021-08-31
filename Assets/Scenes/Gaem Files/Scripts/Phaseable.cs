using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phaseable : MonoBehaviour
{
    public GameObject wall;
    public RigidbodyPlayer rbp;
    public Transform player;
    [SerializeField] private LayerMask playerMask;

    void Update()
    {
        if (rbp.DashQuery())
        {
            if(Physics.OverlapBox(transform.position, new Vector3(1 , 2.5f , 3), Quaternion.identity, playerMask).Length > 0)
            {
                Destroy(wall.GetComponent<MeshCollider>());
            }
        }
        else
        {
            if(wall.GetComponent<MeshCollider>() == null)
            {
                wall.AddComponent<MeshCollider>();
            }
        }
    }
}
