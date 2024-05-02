using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpitter : EnemyBase
{
    public override void InitializeStates()
    {
        var idleState = new EnemyIdleState(AnimationState, this);
        var rangedAttackState = new EnemyRangedState(transform, RequestingDirectionToPlayer, AnimationState, MonsterData.RangedAttackCooldown, this, ExecRangedAttack);
        var damagedState = new EnemyDamagedState(AnimationState, MonsterData.MaxStunTime);
        var deathState = new EnemyDeathState(AnimationState);

        _enemyStates.Add(idleState);
        _enemyStates.Add(rangedAttackState);
        _enemyStates.Add(damagedState);
        _enemyStates.Add(deathState);

        SetDefaultStateAndSubscribeToStates(idleState);
    }
}
