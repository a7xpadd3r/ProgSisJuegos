using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryMonsters : AbstractFactory<EnemyBase>
{
    private Dictionary<string, EnemyBase> _availableMonsters = new Dictionary<string, EnemyBase>();
    public FactoryMonsters(List<MonsterDatabase> monstersList) 
    {
        foreach (MonsterDatabase monster in monstersList)
            _availableMonsters.Add(monster.EnemyType.ToString(), monster.MonsterPrefab);
    }

    public override EnemyBase CreateProduct(string productCode)
    {
        if (_availableMonsters.TryGetValue(productCode, out EnemyBase enemy))
            return enemy;

        Debug.LogError($"Enemy '{productCode}' not found.");
        return null;
    }
}
