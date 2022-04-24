using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{       
    public bool killed = false;
    

    protected Animator animator;
    protected bool isDead;
    protected AudioSource audioSource;

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
}
