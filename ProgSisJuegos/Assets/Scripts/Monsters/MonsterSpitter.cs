using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpitter : EnemyBase
{
    public override void InitializeStates()
    {
        var idleState = new EnemyIdleState(MonsterData.IAMaxIdleTime, AnimationState, IsPlayerNear, MonsterData.IACanPatrol, MonsterData.IACanMove, IsOnMeleeAttackRange, IsOnRangedAttackRange);
        var rangedAttackState = new EnemyRangedState(transform, RequestingDirectionToPlayer, AnimationState, MonsterData.RangedAttackCooldown, IsOnRangedAttackRange, ExecRangedAttack);
        
        _enemyStates.Add(idleState);
        _enemyStates.Add(rangedAttackState);

        SetDefaultStateAndSubscribeToStates(idleState);
    }
}
