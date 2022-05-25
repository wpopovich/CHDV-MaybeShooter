using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{       
    public bool killed = false;
    public float visionRange = 10;
    public float visionAngle = 15;

    protected Animator animator;
    protected bool isDead;
    protected AudioSource audioSource;
    protected bool playerInVisionCone;

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
        audioSource.Play();
    }

    protected virtual void OnKill()
    {

    }

    protected void LookForPlayer()
    {
        playerInVisionCone = false;
        LevelManager manager = LevelManager.GetInstance();
        Player player = manager.Player();
        if (Vector3.Distance(player.transform.position, gameObject.transform.position) <= visionRange) {
            Debug.DrawLine(gameObject.transform.position, player.transform.position, Color.red);
            Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward, Color.black);
            Vector3 direction = transform.position - player.transform.position;;
            if (Vector3.Angle(-1 * transform.forward, direction) <= visionAngle) {
                playerInVisionCone = true;
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
