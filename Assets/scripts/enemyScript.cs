
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    public Transform waypoint1;  
    public Transform waypoint2; 
    public float speed = 2.0f;   
    public float rotationSpeed = 5.0f; 

    private Transform targetWaypoint; 
    private bool movingToWaypoint1 = true; 

    void Start()
    {
        // Start the ghost movement towards the first waypoint
        targetWaypoint = waypoint1;
    }

    void Update()
    {
        MoveTowardsTarget();
    }


    private void MoveTowardsTarget()
    {
        Vector3 direction = targetWaypoint.position - transform.position;

        RotateTowardsTarget(direction);

        transform.position += direction.normalized * (speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            SwitchTargetWaypoint();
        }
    }


    private void RotateTowardsTarget(Vector3 direction)
    {
        if (direction != Vector3.zero) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void SwitchTargetWaypoint()
    {

        if (movingToWaypoint1)
        {
            targetWaypoint = waypoint2;
        }
        else
        {
            targetWaypoint = waypoint1;
        }
        movingToWaypoint1 = !movingToWaypoint1;
    }
}