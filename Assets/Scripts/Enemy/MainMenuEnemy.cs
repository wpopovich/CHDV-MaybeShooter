using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEnemy : Enemy
{
    public List<Transform> waypointList;
    public float timeToWait = 10;
    public float speed = 2;
    public float rotationSpeed = 10;

    private IEnumerator<Transform> waypointEnumerator;
    private Transform currentWaypoint;
    private bool arrived;
    private bool shouldMove;
    private float waitingTime;


    void Start()
    {
        InitEnemy();

        if (waypointList.Count <= 1)
        {
            Debug.LogError("Moving enemy requires at least 2 waypoints!");
            isDead = true;
            return;
        }

        waypointEnumerator = waypointList.GetEnumerator();
        Transform initialWaypoint = NextWaypoint();
        transform.position = initialWaypoint.position;
    }

    void Update()
    {
        if (arrived)
        {
            waitingTime += Time.deltaTime;
        }

        if (arrived && waitingTime > timeToWait)
        {
            shouldMove = true;
            arrived = false;
            waitingTime = 0;
        }

        if (shouldMove)
        {
            Move();
        }

        if (transform.position == currentWaypoint.position)
        {
            arrived = true;
            shouldMove = false;
            NextWaypoint();
        }

        LookAtWaypoint();
        Animate();
    }

    Transform NextWaypoint()
    {
        bool hasNext = waypointEnumerator.MoveNext();
        if (hasNext)
        {
            return currentWaypoint = waypointEnumerator.Current;
        }
        else
        {
            waypointEnumerator.Reset();
            return NextWaypoint();
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);
    }

    void LookAtWaypoint()
    {
        Quaternion rot = Quaternion.LookRotation(currentWaypoint.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
    }
    void Animate()
    {
        animator.SetBool("isWalking", shouldMove);
    }
}
