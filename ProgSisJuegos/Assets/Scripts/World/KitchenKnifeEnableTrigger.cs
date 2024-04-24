using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenKnifeEnableTrigger : MonoBehaviour
{
    public Light bathroomLight;
    public GameObject forceOpenDoorObject;

    private void OnTriggerEnter(Collider other)
    {
        bathroomLight.intensity = 2;
        forceOpenDoorObject.SetActive(true);
        Destroy(this.gameObject);
    }
}
