using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class NightmareFinalTeleport : MonoBehaviour, IInteractable
{
    private string commandIdOne = "bossbattlefirst";
    private string commandIdTwo = "bossbattlesecond";
    public PlayerController thePlayer;
    public GameObject cageArena;
    public GameObject disableNightmare;

    public Transform arenaDestination;

    public Transform boss1;
    public Transform boss2;

    private GameManager _manager;

    private void Start()
    {
        // Get gm
        _manager = FindObjectOfType<GameManager>();
        if (_manager == null)
        {
            Debug.LogError("invalid manager!");
            return;
        }

        // create boss
        ICommand bossOne = new CommandCreateEnemy(_manager.CreateMonster(MonsterType.Faster.ToString()), boss1);
        _manager.EventsCommandsQueue.EnqueueOnDemandCommand(commandIdOne, bossOne);

        ICommand BossTwo = new CommandCreateEnemy(_manager.CreateMonster(MonsterType.Faster.ToString()), boss2);
        _manager.EventsCommandsQueue.EnqueueOnDemandCommand(commandIdTwo, BossTwo);
    }

    public void Interact()
    {
        // Clear all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
            Destroy(enemy);

        cageArena.SetActive(true);
        thePlayer.gameObject.SetActive(false);

        _manager.EventsCommandsQueue.ExecuteOnDemandCommand(commandIdOne);
        _manager.EventsCommandsQueue.ExecuteOnDemandCommand(commandIdTwo);

        thePlayer.transform.position = arenaDestination.position;
        thePlayer.transform.rotation = arenaDestination.rotation;

        disableNightmare.SetActive(false);
    }
}
