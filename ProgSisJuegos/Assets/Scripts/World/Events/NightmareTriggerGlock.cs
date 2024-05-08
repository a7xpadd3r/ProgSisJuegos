using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareTriggerGlock : MonoBehaviour
{
    public GameObject blockade;
    public GameObject backBlockade;

    private ItemBase _itemScript;

    private void Start()
    {
        _itemScript= GetComponent<ItemBase>();
        _itemScript.OnInteracted += OnGlockGrabbed;
    }

    private void OnGlockGrabbed()
    {
        _itemScript.OnInteracted -= OnGlockGrabbed;
        backBlockade.SetActive(true);
        blockade.SetActive(false);
    }
}
