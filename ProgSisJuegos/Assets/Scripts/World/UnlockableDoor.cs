using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableDoor : MonoBehaviour, ICommand
{
    [SerializeField] private string _doorId = "changeme";
    private DoorBase _door;
    private GameManager _managerScript;

    private void Start()
    {
        _door = GetComponent<DoorBase>();

        GameObject manager = GameObject.FindGameObjectWithTag("GameController");
        _managerScript = manager.GetComponent<GameManager>();
        
        if (_managerScript != null)
            _managerScript.EventsCommandsQueue.EnqueueOnDemandCommand(_doorId, this);

        _door.OnInteracted += OnDoorInteracted;
    }

    public void Execute()
    {
        if (_door.IsLocked)
            _door.UnlockLockDoor(false);
    }

    private void OnDoorInteracted()
    {
        if (_managerScript.OnPlayerHasKey(_doorId))
        {
            _managerScript.EventsCommandsQueue.ExecuteOnDemandCommand(_doorId);
            _door.OnInteracted -= OnDoorInteracted;
        }
    }
}
