using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public RigidbodyPlayer rbp;
    public Rigidbody player;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float jumpStrength = 100f;
    [SerializeField] private AudioSource soundPad;

    private float coolDown;

    private void Start()
    {
        coolDown = 0;
    }

    void Update()
    {
        if(Physics.OverlapBox(transform.position, new Vector3(1,2.5f,1), Quaternion.identity, playerMask).Length > 0 && Time.time - coolDown > 0.3f)
        {
            if (rbp.GroundedQuery())
            {
                soundPad.Play();
                coolDown = Time.time;
                player.velocity = new Vector3(player.velocity.x, jumpStrength, 0);
            }
        }
    }
}
