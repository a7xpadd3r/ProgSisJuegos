using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionControls : MonoBehaviour
{
    public PlayerController playerRef;
    public List<GameObject> controlStuff;

    private void Start()
    {
        playerRef.LightDisable(false);
    }

    private void LateUpdate()
    {
        foreach (GameObject item in controlStuff)
        {
            item.SetActive(playerRef.IsPlayerEnabled);
        }
    }
}
