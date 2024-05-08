using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPistol : MonoBehaviour
{
    private ItemBase _itemScript;
    public GameObject controls;

    void Start()
    {
        _itemScript = GetComponent<ItemBase>();
        _itemScript.OnInteracted += ShowControls;
    }

    private void ShowControls()
    {
        controls.SetActive(true);
    }
}
