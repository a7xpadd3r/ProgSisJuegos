using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareTriggerBlockade : MonoBehaviour
{
    public string commandId = "changeme";
    public GameObject backBlockade;
    public Transform enemySpawnPosition;
    public MonsterType monsterToSpawn = MonsterType.Wheelchair;
    public bool spawnEnemy = true;


    private GameManager _manager;

    private void Start()
    {
        if (spawnEnemy)
        {
            _manager = FindObjectOfType<GameManager>();
            ICommand enemySpawnCommand = new CommandCreateEnemy(_manager.CreateMonster(monsterToSpawn.ToString()), enemySpawnPosition);
            _manager?.EventsCommandsQueue.EnqueueOnDemandCommand(commandId, enemySpawnCommand);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (spawnEnemy)
            _manager.EventsCommandsQueue.ExecuteOnDemandCommand(commandId);

        backBlockade.SetActive(true);        
        Destroy(gameObject);
    }
}
