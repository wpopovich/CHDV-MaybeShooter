using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public EnemyType enemyType;
    public List<Transform> waypointList;
    
    public bool killed = false;

    public float timeToWait = 10;
    public float speed = 2;
    public float rotationSpeed = 10;

    private Animator animator;

    private bool isDead;
    private IEnumerator<Transform> waypointEnumerator;
    private Transform currentWaypoint;
    private bool arrived;
    private bool shouldMove;
    private float waitingTime;

    // Start is called before the first frame update
    void Start()
    {
        ValidateEnemy(enemyType);
        InitEnemy(enemyType);

        animator = GetComponentInChildren<Animator>();        
    }

    void ValidateEnemy(EnemyType enemyType)
    {
        switch(enemyType) {
            case EnemyType.Patrolling:
                if (waypointList.Count <= 1) {
                    Debug.LogError("Moving enemy requires at least 2 waypoints!");
                    isDead = true;
                    return;
                }
                break;
            default:
                break;
        }
    }

    void InitEnemy(EnemyType enemyType)
    {
        switch(enemyType) {
            case EnemyType.Patrolling:
                waypointEnumerator = waypointList.GetEnumerator();
                Transform initialWaypoint = NextWaypoint();
                transform.position = initialWaypoint.position;
                arrived = true;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        if (killed) {
            AnimateDeath();
            isDead = true;
            killed = false;
        }

        switch(enemyType) {
            case EnemyType.Patrolling:
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
                break;
            default:
                break;
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);
    }

    void LookAtWaypoint()
    {
        //transform.LookAt(currentWaypoint);
        //Quaternion rot = Quaternion.LookRotation(currentWaypoint.position, Vector3.up);
        Quaternion rot = Quaternion.LookRotation(currentWaypoint.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
    }

    Transform NextWaypoint() {
        bool hasNext = waypointEnumerator.MoveNext();
        if (hasNext) {
            return currentWaypoint = waypointEnumerator.Current;
        } else {
            waypointEnumerator.Reset();
            return NextWaypoint();
        }
    }

    void Animate()
    {
        animator.SetBool("isWalking", shouldMove);      
    }

    void AnimateDeath()
    {
        animator.SetTrigger("Die");
    }

    public enum EnemyType
    {
        Patrolling,
        Static
    }

    public void Kill()
    {

    }
}
