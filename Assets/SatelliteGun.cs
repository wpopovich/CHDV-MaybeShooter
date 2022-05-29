using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteGun : MonoBehaviour
{
    public ParticleSystem particles;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        InteractableObjective.onComplete += Fire;

        var main = particles.main;
        main.duration = audioSource.clip.length;
    }

    void Fire(InteractableObjective objective)
    {
        particles.Play();
        audioSource.Play();
    }
}
