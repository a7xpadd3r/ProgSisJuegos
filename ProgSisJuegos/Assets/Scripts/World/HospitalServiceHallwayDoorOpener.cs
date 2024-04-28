using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalServiceHallwayDoorOpener : MonoBehaviour
{
    public List<DestructibleBase> destructibleWoods;

    private DoorBase _door;

    void Start()
    {
        _door = GetComponent<DoorBase>();
        _door.OnInteracted += OpenDoor;
    }

    void OpenDoor()
    {
        for (int i = 0; i < destructibleWoods.Count; i++)
            if (destructibleWoods[i] != null) return;

        _door.UnlockLockDoor(false);
        _door.OnInteracted -= OpenDoor;
    }

}
