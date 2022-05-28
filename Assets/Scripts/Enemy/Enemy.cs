using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{       
    public bool killed = false;
    public float visionRange = 10;
    public float visionAngle = 15;
    public Light visionLight;

    protected Animator animator;
    protected bool isDead;
    protected AudioSource audioSource;
    protected bool playerInVisionCone;

    public Transform eyesPosition;

    protected virtual void Start()
    {

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

    protected void ActivateAlarm()
    {
        LevelManager.GetInstance().PlayAlarm();
    }

    protected void DeactivateAlarm()
    {
        LevelManager.GetInstance().StopAlarm();
    }
}
