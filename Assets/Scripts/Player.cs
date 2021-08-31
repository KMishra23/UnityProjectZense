using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpVelocity = 5f;

    private bool isGrounded;
    private bool jumpKeyPressed;
    private float horizontalInput;
    private bool doubleJump;

    private float forwardInput;

    private Rigidbody bodyPlayer;

    public Transform groundCheck;
    private void Start()
    {
        bodyPlayer = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            jumpKeyPressed = true;
        }
        if(Physics.OverlapSphere(groundCheck.position, 0.1f).Length < 1)
        {
            isGrounded = true;
            doubleJump = true;
        }
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if(jumpKeyPressed && (isGrounded || doubleJump))
        {
            isGrounded = false;
            bodyPlayer.AddForce(Vector3.up * jumpVelocity, ForceMode.VelocityChange);
            jumpKeyPressed = false;
            if(!isGrounded)
            {
                doubleJump = false;
            }
        }
        bodyPlayer.velocity = new Vector3(forwardInput * speed, bodyPlayer.velocity.y, horizontalInput * speed);
    }
    */

    
    public CharacterController controller;
    public float speed = 10f;
    public float gravity = -10f;

    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    bool doubleJump;
    public float jumpVelocity = 10f;    

    void Start()
    {
        velocity.y = 0;
        doubleJump = true;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded)
        {
            doubleJump = true;
        }
    
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -4f;
        }

        if((doubleJump || isGrounded) && Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpVelocity;
            if(!isGrounded)
            {
                doubleJump = false;
            }
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
