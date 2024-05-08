using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCreateEnemy : ICommand
{
    private EnemyBase _prefab;
    private Transform _transform;

    private EnemyBase _instance;

    public CommandCreateEnemy(EnemyBase prefab)
    {
        _prefab = prefab;
    }

    public CommandCreateEnemy(EnemyBase prefab, Transform positionAndRotation)
    {
        _prefab = prefab;
        _transform = positionAndRotation;
    }

    public void Execute()
    {
        _instance = Object.Instantiate(_prefab, _transform.position, _transform.rotation);
    }

    public void Undo()
    {
        if (_instance != null ) Object.Destroy(_instance);
    }
}
