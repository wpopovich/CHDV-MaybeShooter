using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjective : MonoBehaviour
{
    public ObjectiveScript objective;
    public bool completed = false;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null) {
            Debug.LogError(gameObject.name + " doesn't have an associated audioSource for completion sounds");
        }
    }

    public void CompleteObjective()
    {
        if (isCompleted())
            return;

        Debug.Log(objective.completedText);
        PlaySound();
        completed = true;
    }

    void PlaySound()
    {
        if (objective.audioClip != null && audioSource != null) {
            audioSource.clip = objective.audioClip;
            audioSource.Play();
        }
    }

    public bool isCompleted()
    {
        return completed;
    }

    public string GetObjectiveDescription()
    {
        return objective.GetObjectiveText();
    }
}
