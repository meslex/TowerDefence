using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WaypointMovement : MonoBehaviour
{
    #region Fields
    public event Action ReachedDestination;

    [SerializeField] private Waypoint currentWaypoint = default;
    [SerializeField] private float switchDistance = default;
    [Range(0f, 10f)]
    [SerializeField] private float speed = default;
    [SerializeField] private float steeringForce = default;
    [SerializeField] private float turnSmoothing = default;

    private Vector3 destination;
    private Vector3 upVector;
    //private Vector3 gravity = Vector3.down;

    private Vector3 currentVelocity;
    private bool moving;
    private float switchDistSqr { get { return switchDistance * switchDistance; } }
    private Transform myTransform;
    private Vector3 impulse;

    public float Speed { get { return speed; } set{ speed = value; } }
    public int WaypointsSwitched { get; private set; }
    public bool IsMoving { get { return moving; } }
    public Vector3 Velocity { get { return currentVelocity.normalized * speed; } }
    #endregion

    private void OnEnable()
    {
        WaypointsSwitched = 0;
        //currentVelocity = transform.forward;
    }

    private void Start()
    {
        myTransform = transform;
    }

    public void SetWaypoint(Waypoint waypoint)
    {
        currentWaypoint = waypoint;
        destination = currentWaypoint.GetPosition();
        upVector = currentWaypoint.transform.up;
        moving = true;
    }

    public void StartMovement()
    {
        if (currentWaypoint == null)
            moving = false;
        else
            moving = true;
        
    }

    public void Stop()
    {
        moving = false;
    }

    private void FixedUpdate()
    {
        if (moving)
        {

            float dist = Vector3.SqrMagnitude(transform.position - destination);

            if (dist < switchDistSqr)
            {
                SwitchWaypoint();
            }
            Move();
        }
    }

    private void SwitchWaypoint()
    {
        bool shouldBranch = false;

        //checks if current waypoint has branches and decides whether it should use that branch 
        if (currentWaypoint.branches != null && currentWaypoint.branches.Count > 0)
        {
            shouldBranch = UnityEngine.Random.Range(0f, 1f) <= currentWaypoint.branchRation ? true : false;
        }

        if (shouldBranch)
        {
            //picks random branch
            currentWaypoint = currentWaypoint.branches[UnityEngine.Random.Range(0, currentWaypoint.branches.Count - 1)];
        }
        else
        {
            currentWaypoint = currentWaypoint.nextWaypoint;
        }

        if (currentWaypoint == null)
        {
            moving = false;
            ReachedDestination?.Invoke();
        }
        else
        {
            WaypointsSwitched++;
            destination = currentWaypoint.GetPosition();
            upVector = currentWaypoint.transform.up;
        }


    }

    private void Move()
    {
        Vector3 desiredVelocity = (destination - myTransform.position).normalized;
        Vector3 steering = (desiredVelocity - currentVelocity) * steeringForce * Time.fixedDeltaTime;

        currentVelocity += steering ;

        myTransform.position += currentVelocity.normalized * speed * Time.fixedDeltaTime;


        myTransform.position += impulse /10;

        impulse -= impulse / 10; 

        Quaternion targetRotation = Quaternion.LookRotation(currentVelocity, upVector);

        //float deltaAngle = Quaternion.Angle(targetRotation, myTransform.rotation);

        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRotation, turnSmoothing * Time.fixedDeltaTime);
        if(impulse.x < 0)
            impulse = Vector3.zero;
    }

    /*private void Move()
    {
        Vector3 desiredVelocity = (destination - myTransform.position).normalized;
        Vector3 steering = (desiredVelocity - currentVelocity) * steeringForce * Time.deltaTime;

        currentVelocity += steering + impulse + Vector3.down;

        myTransform.position += currentVelocity.normalized * speed * Time.deltaTime;

        Quaternion targetRotation = Quaternion.LookRotation(currentVelocity, upVector);

        //float deltaAngle = Quaternion.Angle(targetRotation, myTransform.rotation);

        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
        impulse = Vector3.zero;
    }*/

    public void AddImpulse(Vector3 impulse)
    {
        this.impulse = impulse;
        //GetComponent<Rigidbody>().velocity = impulse;
    }
}
