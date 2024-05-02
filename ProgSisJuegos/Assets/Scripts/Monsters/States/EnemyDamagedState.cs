using System;
using UnityEngine;

public class EnemyDamagedState : EnemyStateBase
{
    private float _currentStunTime;
    private float _maxStunTime;

    Func<Animator> _animation;

    public EnemyDamagedState()
    {
        // Empty state
    }
    public EnemyDamagedState(Func<Animator> animation, float maxStunTime)
    {
        _animation = animation;
        _maxStunTime = maxStunTime;
    }

    public override void OnEnterState()
    {
        _currentStunTime = _maxStunTime;
        _animation().SetTrigger("Damaged");
    }

    public override void OnExecute(float deltaTime, float turnSpeed = 1)
    {
        _currentStunTime -= deltaTime;

        Debug.Log(_currentStunTime);

        if (_currentStunTime <= 0)
            OnStateChangePetitionHandler(EnemyStates.Idle);
    }

    public override void OnExitState()
    {
        _currentStunTime = _maxStunTime;
    }
}
