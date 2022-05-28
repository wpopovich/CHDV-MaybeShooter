using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{       
    public bool killed = false;
    public float visionRange = 10;
    public float visionAngle = 15;
    public Light visionLight;
    public Transform eyesPosition;
    public float timeForAlarm = 3;

    protected Animator animator;
    protected bool isDead;
    protected AudioSource audioSource;
    protected bool playerInVisionCone;

    protected Color calm = Color.green;
    protected Color alerted = Color.yellow;
    protected Color detected = Color.red;
    protected bool playerDetected;

    protected float alarmTimer;


    protected virtual void Start()
    {
        InitEnemy();
        alarmTimer = 0;
        playerDetected = false;
        visionLight.color = calm;
    }

    protected virtual void Update()
    {

    }

    protected void InitEnemy()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected void KillEnemy(bool shouldKill)
    {
        if (shouldKill) {
            AnimateDeath();
            isDead = true;
            killed = false;
            List<BoxCollider> colliderList = new List<BoxCollider>();
            colliderList.AddRange(GetComponents<BoxCollider>());
            colliderList.ForEach(c => c.enabled = false);
            OnKill();
        }
    }

    protected void AnimateDeath()
    {
        animator.SetTrigger("Die");
    }

    public void Kill()
    {
        killed = true;
        FindObjectOfType<AudioManager>().Play("EnemyDeath");
    }

    protected virtual void OnKill()
    {
        visionLight.gameObject.SetActive(false);

        if (playerDetected)
            LevelManager.GetInstance().StopAlarm();
    }

    protected void LookForPlayer()
    {
        playerInVisionCone = false;
        LevelManager manager = LevelManager.GetInstance();
        if (manager == null)
            return; 

        Player player = manager.Player();
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= visionRange) {
            Vector3 direction = transform.position - player.transform.position;;
            if (Vector3.Angle(-1 * transform.forward, direction) <= visionAngle) {
                RaycastHit hit;
                Ray ray = new Ray(eyesPosition.position, -1 * direction);
                Physics.Raycast(ray, out hit, visionRange);
                if (hit.transform != null && hit.transform.CompareTag("Player")) {
                    playerInVisionCone = true;
                }
            }
        }
    }

    protected void SearchForPlayer()
    {
        LookForPlayer();
        DetectPlayer();
        UpdateLightColor();
    }

    protected void DetectPlayer()
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
    protected void UpdateLightColor()
    {
        if (playerInVisionCone) {
            visionLight.color = Color.Lerp(visionLight.color, detected, Time.deltaTime);
        } else {
            visionLight.color = Color.Lerp(visionLight.color, calm, Time.deltaTime);
        }
    }

    protected void ActivateAlarm()
    {
        LevelManager.GetInstance().PlayAlarm();
    }

    protected void DeactivateAlarm()
    {
        LevelManager.GetInstance().StopAlarm();
    }
}
