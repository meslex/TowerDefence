using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WaypointMovement : MonoBehaviour
{
    public event Action ReachedDestination;

    [SerializeField] private Waypoint currentWaypoint = default;
    [SerializeField] private float switchDistance = default;
    [Range(0f, 10f)]
    [SerializeField] private float speed = default;
    [SerializeField] private float steeringForce = default;
    [SerializeField] private float turnSmoothing = default;

    private Vector3 destination;
    private Vector3 currentVelocity;
    private bool moving;
    private float switchDistSqr { get { return switchDistance * switchDistance; } }

    public void SetWaypoint(Waypoint waypoint)
    {
        currentWaypoint = waypoint;
        destination = currentWaypoint.GetPosition();
        moving = true;
    }

    public void StartMovement()
    {
        moving = true;
    }

    public void Stop()
    {
        moving = false;
    }

    private void Update()
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
            destination = currentWaypoint.GetPosition();
        }


    }

    private void Move()
    {
        Vector3 desiredVelocity = (destination - transform.position).normalized;
        Vector3 steering = (desiredVelocity - currentVelocity) * steeringForce * Time.deltaTime;

        currentVelocity += steering;

        transform.position += currentVelocity.normalized * speed * Time.deltaTime;

        Quaternion targetRotation = Quaternion.LookRotation(currentVelocity);

        float deltaAngle = Quaternion.Angle(targetRotation, transform.rotation);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSmoothing);
    }
}
