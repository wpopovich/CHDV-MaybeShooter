using UnityEngine;

public class InteractableObjective : MonoBehaviour
{
    public ObjectiveScript objective;
    public bool completed = false;
    private AudioSource audioSource;
    public GameObject progressBar;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
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
        if (progressBar != null) // En caso de que el objetivo no necesite de una ProgressBar
        {
            ResetProgressBar();
        }

        if (objective.name == "GeneratorObjective")
        {
            FindObjectOfType<GeneratorScript>().TurnOnGenerator();
        }
    }

    public void ChargeProgressBar()
    {
        progressBar.SetActive(true);
    }

    public void ResetProgressBar()
    {
        progressBar.SetActive(false);
    }

    void PlaySound()
    {
        if (objective.audioClip != null && audioSource != null)
        {
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
