using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public float viewRange;
    public float rotationSpeed;
    public float scaleFactor;
    public GameObject robotHead;
    public AudioSource audioSource;
    public LineRenderer lineRenderer;
    public Transform firingPosition;
    public float firingRange;
    

    private int health = 4;
    private bool isDead = false;
    private bool isDying = false;
    // Start is called before the first frame update
    void Start()
    {
        InteractableObjective.onComplete += Damage;
        lineRenderer.startWidth = .2f;
        lineRenderer.endWidth = .2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        Dying();
        LookAtPlayer();
        Fire();
    }

    void Dying()
    {
        if (!isDying || isDead)
            return;

        robotHead.transform.localScale = Vector3.Lerp(robotHead.transform.localScale, Vector3.zero, Time.deltaTime);
        if (robotHead.transform.localScale.x <= 0.001) {
            robotHead.SetActive(false);
        }

        if (!robotHead.activeSelf)
            isDead = true;
    }   

    void LookAtPlayer()
    {
        Player player = LevelManager.GetInstance().Player();
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= viewRange) {
            Vector3 direction = player.transform.position - transform.position;
            Quaternion lookAtRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void Damage(InteractableObjective objective)
    {
        health -= 1;
        if (health <= 0) {
            isDying = true;
            audioSource.Play();
        }
    }

    void Fire()
    {
        Player player = LevelManager.GetInstance().Player();
        Vector3 targetPosition = firingPosition.position + firingPosition.forward * firingRange + firingPosition.up * -1;
        lineRenderer.SetPosition(0, firingPosition.position);
        lineRenderer.SetPosition(1, new Vector3(targetPosition.x, player.transform.position.y, targetPosition.z));
        Vector3 direction = firingPosition.position - targetPosition;
        //Ray ray = new Ray(firingPosition.position, direction);
        //RaycastHit hit;
        //Physics.Raycast(ray, out hit, firingRange);

        //if (hit.transform != null) {
        //    Debug.Log(hit.transform.name);
        //}
    }
}
