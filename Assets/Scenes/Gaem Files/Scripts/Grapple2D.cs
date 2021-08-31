using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple2D : MonoBehaviour
{
    [SerializeField] private bool grappleTypeSwitch;

    public Crosshair crossHair;

    private Ray grappleRay;
    private RaycastHit grapplePointRaycastHit;
    private bool grape;
    private SpringJoint grappleRope;
    private HingeJoint fixedGrappleRope;
    [SerializeField] private float grappleSpringForce = 10f;
    [SerializeField] private float grappleSpringDamper = 0.2f;
    [SerializeField] private float ToleranceSpring = 0.025f;
    [SerializeField] private float grappleRange = 20f;

    private LineRenderer lr;

    public GameObject player;

    public LayerMask objects;

    public RigidbodyPlayer rbp;

    [SerializeField] AudioSource soundGrapple;

    private void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        //for springy grapple
        
            if (Input.GetKeyDown(KeyCode.Mouse0) && !grape)
            {
                grappleRay.origin = Camera.main.transform.position;//ray starts from camera position
                grappleRay.direction = crossHair.CrosshairPos() - Camera.main.transform.position;//direction from camera towards mouse
                if (Physics.Raycast(grappleRay, out grapplePointRaycastHit, grappleRange, objects))//only grapple if some body is hit
                {
                    grape = true;
                    StartGrappleSpringy();
                    soundGrapple.Play();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && grape)
            {
                grape = false;
                EndGrappleSpringy();
            }
            if (grape)
            {
                DrawRopeSpringy();
            }
        
        /*else//for rope grapple(fixed length of grapple rope
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !grape)
            {
                grappleRay.origin = Camera.main.transform.position;//ray starts from camera position
                grappleRay.direction = crossHair.CrosshairPos() - Camera.main.transform.position;//direction from camera towards mouse
                if (Physics.Raycast(grappleRay, out grapplePointRaycastHit, grappleRange, objects))//only grapple if some body is hit
                {
                    grape = true;
                    StartGrappleFixed();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && grape)
            {
                grape = false;
                EndGrappleFixed();
            }
            if (grape)
            {
                DrawRopeFixed();
            }
        }*/
    }
    void StartGrappleSpringy()
    {
        player.AddComponent<SpringJoint>();
        grappleRope = player.GetComponent<SpringJoint>();//create a springjoint for playerbody
        grappleRope.autoConfigureConnectedAnchor = false;
        grappleRope.anchor = transform.position;//player anchor
        grappleRope.connectedAnchor = new Vector3(grapplePointRaycastHit.point.x, grapplePointRaycastHit.point.y, 0);//grapple point anchor
        grappleRope.connectedBody = grapplePointRaycastHit.rigidbody;//connect spring to rigidbody of hit object
        grappleRope.spring = grappleSpringForce;
        grappleRope.damper = grappleSpringDamper;
        grappleRope.tolerance = ToleranceSpring;
        grappleRope.massScale = 4.5f;
        float distanceFromGrapple = Vector3.Distance(transform.position, grapplePointRaycastHit.point);
        grappleRope.minDistance = distanceFromGrapple * 0.6f;
        grappleRope.maxDistance = distanceFromGrapple * 0.6f;
        grappleRope.enableCollision = true;

        lr.positionCount = 2;
    }
    void EndGrappleSpringy()
    {
        Destroy(player.GetComponent<SpringJoint>());
        lr.positionCount = 0;
    }
    void DrawRopeSpringy()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, new Vector3(grapplePointRaycastHit.point.x, grapplePointRaycastHit.point.y, 0));
    }

    void StartGrappleFixed()
    {
        player.AddComponent<HingeJoint>().axis = new Vector3(0,0,1);
        fixedGrappleRope = player.GetComponent<HingeJoint>();
        fixedGrappleRope.connectedBody = grapplePointRaycastHit.rigidbody;
        fixedGrappleRope.anchor = new Vector3(grapplePointRaycastHit.transform.position.x - player.GetComponent<Transform>().position.x, grapplePointRaycastHit.transform.position.y- player.GetComponent<Transform>().position.y, 0);

        lr.positionCount = 2;
    }
    void EndGrappleFixed()
    {
        Destroy(player.GetComponent<HingeJoint>());
        lr.positionCount = 0;
    }
    void DrawRopeFixed()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, new Vector3(grapplePointRaycastHit.point.x, grapplePointRaycastHit.point.y, 0));
    }
}
   
