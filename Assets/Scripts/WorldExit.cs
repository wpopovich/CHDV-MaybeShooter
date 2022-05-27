using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldExit : MonoBehaviour
{
    Animator animator;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void  OpenDoors()
    {
        animator.SetTrigger("Open");
        audioSource.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            SceneLoader.GetInstance().LoadNextScene();
        }
    }
}
