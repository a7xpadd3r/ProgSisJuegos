using UnityEngine;

public class MonsterWheelchair : EnemyBase
{
    [SerializeField] private bool _patrolEnabled = false;
    public override void InitializeStates()
    {
        var idleState = new EnemyIdleState(this);
        var persuitState = new EnemyChaseState(transform, this, true, 0.35f);
        var meleeState = new EnemyAttackState(transform, this);
        var damagedState = new EnemyDamagedState(AnimationState, MonsterData.MaxStunTime);
        var deathState = new EnemyDeathState(AnimationState);

        _enemyStates.Add(idleState);
        _enemyStates.Add(persuitState);
        _enemyStates.Add(meleeState);
        _enemyStates.Add(damagedState);
        _enemyStates.Add(deathState);

        if (_patrolEnabled)
        {
            var patrolState = new EnemyPatrolState();
            _enemyStates.Add(patrolState);
            print("patrol enabled - TODO patrol wheelchair");
        }

        SetDefaultStateAndSubscribeToStates(idleState);
    }
}
