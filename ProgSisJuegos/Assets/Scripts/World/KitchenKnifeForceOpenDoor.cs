using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenKnifeForceOpenDoor : MonoBehaviour
{
    public DoorBase door;

    private void OnTriggerEnter(Collider other)
    {
        door.UnlockLockDoor(false);
        door.Interact();
        door.UnlockLockDoor(true);
        Destroy(this.gameObject);
    }
}
