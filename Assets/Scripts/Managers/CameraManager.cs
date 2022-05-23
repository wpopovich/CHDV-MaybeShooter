using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject MainCamera;
    public GameObject ControlsCamera;
    public GameObject CinematicCamera;
    public GameObject mainMenu;

    private void Start()
    {
        StartCoroutine(DoCinematic());
    }

    IEnumerator DoCinematic()
    {
        CinematicCamera.SetActive(true);

        yield return new WaitForSeconds(2.5f);

        MainCamera.SetActive(true);
        CinematicCamera.SetActive(false);

        yield return new WaitForSeconds(2f);

        mainMenu.SetActive(true);
    }

    public void ChangeToControlsCamera()
    {
        MainCamera.SetActive(false);
        ControlsCamera.SetActive(true);
    }

    public void ChangeToMainCamera()
    {
        MainCamera.SetActive(true);
        ControlsCamera.SetActive(false);
    }
}
