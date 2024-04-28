using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpitter : EnemyBase
{
    public override void InitializeStates()
    {
        var idleState = new EnemyIdleState(MonsterData.IAMaxIdleTime, Animation, IsPlayerNear);
        CustomAddEnemyState(idleState, true);
    }
}
