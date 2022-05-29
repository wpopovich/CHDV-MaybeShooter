using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour
{
    public GameObject particles;
    public GameObject lights;
    public AudioSource generatorSoundSource;
    public AudioSource generatorStartingUpSource;

    public void TurnOnGenerator()
    {
        particles.SetActive(true);
        lights.SetActive(true);

        generatorSoundSource.Play();
        generatorStartingUpSource.Play();
    }
}
