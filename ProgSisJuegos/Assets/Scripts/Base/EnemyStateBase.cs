using System;

public abstract class EnemyStateBase : StateBase
{
    public event Action<EnemyStates> OnStateChangePetition;

    protected void OnStateChangePetitionHandler(EnemyStates newState)
    {
        OnStateChangePetition?.Invoke(newState);
    }
}
