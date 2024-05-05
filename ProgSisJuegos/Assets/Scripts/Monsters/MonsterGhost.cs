
public class MonsterGhost : EnemyBase
{
    public override void InitializeStates()
    {
        var idleState = new EnemyIdleState(this);
        var persuitState = new EnemyChaseState(transform, this);
        var meleeState = new EnemyAttackState(transform, this);
        var deathState = new EnemyDeathState(AnimationState);

        _enemyStates.Add(idleState);
        _enemyStates.Add(persuitState);
        _enemyStates.Add(meleeState);
        _enemyStates.Add(deathState);

        SetDefaultStateAndSubscribeToStates(idleState);
    }
}
