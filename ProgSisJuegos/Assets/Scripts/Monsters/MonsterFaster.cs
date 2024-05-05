using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFaster : EnemyBase
{
    public override void InitializeStates()
    {
        var idleState = new EnemyIdleState(this);
        var persuitState = new EnemyChaseState(transform, this);
        var meleeState = new EnemyAttackState(transform, this);
        var damagedState = new EnemyDamagedState(AnimationState, MonsterData.MaxStunTime);
        var deathState = new EnemyDeathState(AnimationState);

        _enemyStates.Add(idleState);
        _enemyStates.Add(persuitState);
        _enemyStates.Add(meleeState);
        _enemyStates.Add(damagedState);
        _enemyStates.Add(deathState);

        SetDefaultStateAndSubscribeToStates(idleState);
    }
}
