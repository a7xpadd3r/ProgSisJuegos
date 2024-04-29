using System;
using UnityEngine;

public class EnemyRangedState : EnemyStateBase
{
    private float _cooldown;
    private Func<bool> _isOnRange;
    private Func<bool> _isAttackExecuted;
    private Func<Vector3> _playerDirection;
    private Func<Animator> _getAnimator;

    private Transform _selfTransform;
    private float _currentCooldown;

    public EnemyRangedState() 
    { 
        // Empty state
    }

    public EnemyRangedState(Transform selfTransform, Func<Vector3> playerDirection, Func<Animator> animation, float cooldown, Func<bool> isOnRange, Func<bool> executedAttack)
    {
        _selfTransform = selfTransform;
        _playerDirection = playerDirection;
        _getAnimator = animation;

        _cooldown = cooldown;
        _isOnRange = isOnRange;
        _isAttackExecuted = executedAttack;
    }

    public override void OnEnterState()
    {
        if (!_isOnRange())
            OnStateChangePetitionHandler(EnemyStates.Idle);
    }

    public override void OnExecute(float deltaTime, float turnSpeed)
    {
        Debug.Log("performing ranged attack?");
        // Look at the player
        _selfTransform.right = Vector3.LerpUnclamped(_selfTransform.right, _playerDirection(), deltaTime * turnSpeed);

        if (_currentCooldown <= 0)
        {
            bool attack = _isAttackExecuted();

            if (attack && _getAnimator() != null)
                _getAnimator().SetTrigger("Attack");

            _currentCooldown = _cooldown;
        }

        else if (_currentCooldown > 0)
            _currentCooldown -= deltaTime;

        if (!_isOnRange())
            OnStateChangePetitionHandler(EnemyStates.Idle);
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting ranged state");
    }
}
