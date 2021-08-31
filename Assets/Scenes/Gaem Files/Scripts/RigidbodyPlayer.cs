using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpVelocity = 10f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float playerAcceleration = 5f;
    [SerializeField] private float playerDeceleration = 10f;
    [SerializeField] private float dashSlowDelay = 0.5f;
    [SerializeField] private float dashTimeSlowDown = 0.2f;

    private bool isGrounded;
    private float horizontalInput;
    private bool jumpKeyPressed;
    private bool leftKeyPressed;
    private bool rightKeyPressed;
    private bool canJump;

    private bool controllerToggle;//toggle between rigidbody and character controller

    private Rigidbody playerRigid;

    public Transform groundCheck;

    private Vector3 mousePos;

    public Crosshair crossHair;

    private bool dashing;
    private bool canDash;
    private float dashStartTime;
    private Vector3 oldVelocity;

    private float dashSlowTime;

    private bool timeSwitch;
    private bool slowed;

    public LayerMask ground;

    [SerializeField] private AudioSource soundDash;

    private void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        controllerToggle = true;
        canDash = true;
        slowed = false;
    }

    void Update()
    {
        if (!canDash)//dash is enabled after 1s cooldown and only after the player has touched ground after dashing
        {
            if (isGrounded && (Time.time - dashStartTime > 1)) canDash = true; 
        }
        if (Input.GetKeyDown(KeyCode.E)  && canDash && !dashing)//to dash
        {
            dashSlowTime = Time.time;
        }
        else if(Input.GetKey(KeyCode.E) && canDash && !dashing)
        {
            if(Time.time - dashSlowTime > dashSlowDelay && !slowed)//if e is held for more than some time, slow down time
            {
                dashSlowTime = Time.time;
                oldVelocity = playerRigid.velocity;
                playerRigid.velocity = new Vector3(oldVelocity.x * 0.1f, oldVelocity.y * 0.1f, 0);
                Time.timeScale = dashTimeSlowDown;//slow down time for player
                Time.fixedDeltaTime *= dashTimeSlowDown;
                slowed = true;
            }
            else if (slowed && Time.time - dashSlowTime > 0.25)//autodash after holding dash for some time
            {
                slowed = false;
                playerRigid.velocity = oldVelocity;
                Time.timeScale = 1f;
                Time.fixedDeltaTime /= dashTimeSlowDown;
                StartDash();
                canDash = false;
            }
        }
        else if(Input.GetKeyUp(KeyCode.E) && canDash && !dashing)
        {
            if (slowed)
            {
                slowed = false;
                playerRigid.velocity = oldVelocity;
                Time.fixedDeltaTime /= dashTimeSlowDown;
                Time.timeScale = 1f;
            }
            StartDash();
            canDash = false;
        }
        else if (dashing && Time.time - dashStartTime > 0.2)//if player is dashing and greater than a 1s has elapsed, end dash
        {
            EndDash();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            controllerToggle = !controllerToggle;
            if (controllerToggle)//deletes character controller
            {
                Destroy(GetComponent<CharacterController>());
                gameObject.AddComponent<Rigidbody>();
                playerRigid = GetComponent<Rigidbody>();
                playerRigid.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionZ;
            }
            else//deletes rigidbody
            {
                Destroy(GetComponent<Rigidbody>());
                gameObject.AddComponent<CharacterController>();
            }
        }
        if (controllerToggle)//if rigidbody movement
        {
            if (Input.GetButtonDown("Jump"))
            {
                jumpKeyPressed = true;
            }
            if (Physics.OverlapSphere(groundCheck.position, 0.01f, ground).Length > 0)
            {
                isGrounded = true;
                canJump = true;
            }
            else isGrounded = false;
            leftKeyPressed = false;
            rightKeyPressed = false;
            if (Input.GetKey(KeyCode.A))
            {
                leftKeyPressed = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                rightKeyPressed = true;
            }
        }
    }

    void FixedUpdate()
    {
        if (controllerToggle && !dashing)
        {
            if (jumpKeyPressed && canJump)
            {
                playerRigid.velocity = new Vector3(playerRigid.velocity.x, jumpVelocity, 0f);
                jumpKeyPressed = false;
                isGrounded = false;
                canJump = false;
            }
            jumpKeyPressed = false;
            if (leftKeyPressed)//wants to move to the left
            {
                if (playerRigid.velocity.x > 0)//if player has not reached max speed to the left and is moving to the right
                {
                    playerRigid.AddForce(-1 * playerDeceleration, 0, 0, ForceMode.Acceleration);
                }
                else if (playerRigid.velocity.x > -1 * speed)//if player has not reached max speed to the left and is moving to the left
                {
                    playerRigid.AddForce(-1 * playerAcceleration, 0, 0, ForceMode.Acceleration);
                }
            }
            else if (rightKeyPressed)//wants to move to the right
            {
                if (playerRigid.velocity.x < 0)//if player has not reached max speed to the right and is moving left
                {
                    playerRigid.AddForce(1 * playerDeceleration, 0, 0, ForceMode.Acceleration);
                }
                else if(playerRigid.velocity.x < speed)//if player has not reached max speed and is moving to the right
                {
                    playerRigid.AddForce(1 * playerAcceleration, 0, 0, ForceMode.Acceleration);
                }
            }
            else if(playerRigid.velocity.x != 0)//no key pressed so decelerate the player 
            {
                if(playerRigid.velocity.x < -0.2)//moving to left
                {
                    playerRigid.AddForce(1 * playerDeceleration, 0, 0, ForceMode.Acceleration);
                }
                else if (playerRigid.velocity.x > 0.2)//moving to right
                {
                    playerRigid.AddForce(-1 * playerDeceleration, 0, 0, ForceMode.Acceleration);
                }
                else { 
                    playerRigid.velocity = new Vector3(0, playerRigid.velocity.y, 0);
                }
            }
        }
    }

    void StartDash()
    {
        soundDash.Play();
        dashing = !dashing;
        //Debug.Log(crossHair.CrosshairPos());
        mousePos = crossHair.CrosshairPos();//obtain mouse pos
        oldVelocity = playerRigid.velocity;//store velocity of player right before dash
        Vector3 dashPoint = (mousePos - transform.position).normalized;//get the dash direction

        playerRigid.useGravity = false;//disable gravity
        playerRigid.velocity = new Vector3(dashPoint.x * dashSpeed, dashPoint.y * dashSpeed, 0);//assign dash velocity

        dashStartTime = Time.time;//start tracking time
    }
    
    void EndDash()
    {
        playerRigid.useGravity = true;//renable gravity
        playerRigid.velocity = new Vector3(oldVelocity.x, 0, 0);//reassign velocity before dash
        dashing = !dashing;//disable dash state
    }
    
    public void hitPellet()
    {
        canDash = true;
    }

    public void hitJumpPellet()
    {
        canJump = true;
    }

    public void EnableDash()
    {
        canDash = true;
    }
    public void DisableDash()
    {
        canDash = false;
    }
    public bool DashQuery()
    {
        return dashing;
    }
    public bool GroundedQuery()
    {
        return isGrounded;
    }
}
