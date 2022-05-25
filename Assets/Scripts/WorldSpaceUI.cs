using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    GameObject interactIcon;
    public

    void ShowInteractIcon()
    {
        interactIcon.SetActive(true);
    }

    void FollowPlayer()
    {
        //Vector3 direccion = player.transform.position - transform.position;
        //transform.position += (direccion * speed * Time.deltaTime);
    }
}
