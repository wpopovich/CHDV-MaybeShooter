using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{
    public List<Transform> waypointList;
    public float timeToWait = 10;
    public float speed = 2;
    public float rotationSpeed = 10;
    public float timeForAlarm;

    private IEnumerator<Transform> waypointEnumerator;
    private Transform currentWaypoint;
    private bool arrived;
    private bool shouldMove;
    private float waitingTime;
    private Color calm = Color.green;
    private Color alerted = Color.yellow;
    private Color detected = Color.red;
    private bool playerDetected;

    private float alarmTimer;


    // Start is called before the first frame update
    protected override void Start()
    {
        InitEnemy();

        if (waypointList.Count <= 1) {
            Debug.LogError("Moving enemy requires at least 2 waypoints!");
            isDead = true;
            return;
        }

        waypointEnumerator = waypointList.GetEnumerator();
        Transform initialWaypoint = NextWaypoint();
        transform.position = initialWaypoint.position;
        alarmTimer = 0;
        playerDetected = false;
        visionLight.color = calm;

        arrived = true;

    }

    // Update is called once per frame
    protected override void Update()
    {
        if (isDead)
            return;

        KillEnemy(killed);

        if (arrived) {
            waitingTime += Time.deltaTime;
        }

        if (arrived && waitingTime > timeToWait) {
            shouldMove = true;
            arrived = false;
            waitingTime = 0;
        }

        if (shouldMove) {
            Move();
        }

        if (transform.position == currentWaypoint.position) {
            arrived = true;
            shouldMove = false;
            NextWaypoint();
        }

        LookAtWaypoint();
        Animate();
        LookForPlayer();
        DetectPlayer();
        UpdateLightColor();
    }

    Transform NextWaypoint()
    {
        bool hasNext = waypointEnumerator.MoveNext();
        if (hasNext) {
            return currentWaypoint = waypointEnumerator.Current;
        } else {
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

    void DetectPlayer()
    {
        if (playerInVisionCone) {
            alarmTimer += Time.deltaTime;
            if (alarmTimer > timeForAlarm)
                alarmTimer = timeForAlarm;
        } else {
            alarmTimer = Mathf.Max(0, alarmTimer - Time.deltaTime);
        }

        if (alarmTimer >= timeForAlarm) {
            Debug.Log("PLayerDetected");
            playerDetected = true;
            LevelManager.GetInstance().Detected();
        }
    }

    void UpdateLightColor()
    {
        if (playerInVisionCone) {
            visionLight.color = Color.Lerp(visionLight.color, detected, Time.deltaTime) ;
        } else {
            visionLight.color = Color.Lerp(visionLight.color, calm, Time.deltaTime);
        }
    }

    protected override void OnKill()
    {
        base.OnKill();

        if (playerDetected)
            LevelManager.GetInstance().StopAlarm();

        
    }
}
