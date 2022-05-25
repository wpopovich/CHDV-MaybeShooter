using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    //public AudioSource audSource;
    //public AudioClip music;

    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    private void Start()
    {
        StartCoroutine(PlayMainMenuMusic());
    }
    
    public IEnumerator PlayMainMenuMusic()
    {
        yield return new WaitForSeconds(2);

        Play("MainMenuMusic");
    }

    public void PlayPointerEnterSound()
    {
        Play("PointerEnter");
    }

    public void PlayPointerClickSound()
    {
        Play("PointerClick");
    }
}
