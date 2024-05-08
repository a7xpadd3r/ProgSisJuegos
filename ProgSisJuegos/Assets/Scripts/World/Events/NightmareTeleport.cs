using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareTeleport : MonoBehaviour, IInteractable
{
    public GameObject elevatorSequence;
    public PlayerController thePlayer;

    public void Interact()
    {
        thePlayer.gameObject.SetActive(false);
        elevatorSequence.SetActive(true);
    }
}
