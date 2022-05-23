using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audSource;
    public AudioClip music;

    private void Start()
    {
        StartCoroutine(PlaySound(music));
    }

    public IEnumerator PlaySound(AudioClip sound)
    {
        audSource.clip = sound;

        yield return new WaitForSeconds(2);

        audSource.Play();
    }
}
