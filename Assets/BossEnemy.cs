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
    public AudioClip laserFireSound;
    public AudioClip deathSound;
    public float firingRange;
    public float firingRate;
    public float fireDuration;
    public ParticleSystem propulsion;
    public Light visionLight;

    private int health = 4;
    private bool isDead = false;
    private bool isDying = false;
    public float firingRateTimer = 0;    
    public float fireTimer;
    bool firing = false;
    // Start is called before the first frame update

    void Start()
    {
        InteractableObjective.onComplete += Damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        Dying();
        LookAtPlayer();
        if (!isDying)
            Fire();
    }

    void Dying()
    {
        if (!isDying || isDead)
            return;

        propulsion.Stop();
        propulsion.gameObject.SetActive(false);
        visionLight.gameObject.SetActive(false);
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
            audioSource.clip = deathSound;
            audioSource.Play();
        }
    }

    void Fire()
    {
        lineRenderer.enabled = firing;
        if (firing) {
            
            fireTimer += Time.deltaTime;
            Player player = LevelManager.GetInstance().Player();
            Vector3 targetPosition = firingPosition.position + firingPosition.forward * firingRange + firingPosition.up * -3;
            lineRenderer.SetPosition(0, firingPosition.position);
            
            Vector3 direction = firingPosition.position - targetPosition;
            Ray ray = new Ray(firingPosition.position, direction * -1);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, firingRange);
            Debug.DrawRay(firingPosition.position, direction * -1, Color.red, 10);
            if (hit.transform != null) {
                lineRenderer.SetPosition(1, hit.point);
                if (hit.transform.CompareTag("Player")) {
                    player.Kill(UIManager.GameOverReason.Boss);
                }
            } else {
                lineRenderer.SetPosition(1, new Vector3(targetPosition.x, player.transform.position.y, targetPosition.z));
            }
        } else {
            firingRateTimer += Time.deltaTime;
        }

        if (fireTimer >= fireDuration) {
            fireTimer = 0;
            firingRateTimer = 0;
            firing = false;
        }

        if (firingRateTimer >= firingRate) {
            firing = true;
            audioSource.clip = laserFireSound;
            audioSource.Play();
        }
            
    }

    private void OnDestroy()
    {
        InteractableObjective.onComplete -= Damage;
    }
}
