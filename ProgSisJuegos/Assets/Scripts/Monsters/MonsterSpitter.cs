public class MonsterSpitter : EnemyBase
{
    public override void InitializeStates()
    {
        var idleState = new EnemyIdleState(this);
        var rangedAttackState = new EnemyRangedState(transform, this);
        var damagedState = new EnemyDamagedState(AnimationState, MonsterData.MaxStunTime);
        var deathState = new EnemyDeathState(AnimationState);

        _enemyStates.Add(idleState);
        _enemyStates.Add(rangedAttackState);
        _enemyStates.Add(damagedState);
        _enemyStates.Add(deathState);

        SetDefaultStateAndSubscribeToStates(idleState);
    }
}
