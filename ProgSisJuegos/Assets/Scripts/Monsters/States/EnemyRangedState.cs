using System;
using UnityEngine;

public class EnemyRangedState : EnemyStateBase
{
    private float _cooldown;
    private Func<bool> _isAttackExecuted;
    private Func<Vector3> _playerDirection;
    private Func<Animator> _getAnimator;

    private Transform _selfTransform;
    private float _currentCooldown;
    private EnemyBase _controller;

    public EnemyRangedState(Transform selfTransform, EnemyBase baseScript)
    {
        _controller = baseScript;

        _cooldown = _controller.MonsterData.RangedAttackCooldown;
        _isAttackExecuted = _controller.ExecRangedAttack;
        _playerDirection = _controller.RequestingDirectionToPlayer;
        _getAnimator = _controller.AnimationState;
        _selfTransform = selfTransform;
    }

    public override void OnEnterState()
    {
        if (!_controller.PlayerNearAndLoS)
            OnStateChangePetitionHandler(EnemyStates.Idle);
    }

    public override void OnExecute(float deltaTime, float turnSpeed)
    { 
        _selfTransform.right = Vector3.LerpUnclamped(_selfTransform.right, _playerDirection(), deltaTime * turnSpeed);

        if (_currentCooldown <= 0)
        {
            bool attack = _isAttackExecuted();

            if (attack && _getAnimator() != null)
                _getAnimator().SetTrigger("RangedAttack");

            _currentCooldown = _cooldown;
        }

        else
            _currentCooldown -= deltaTime;

        if (!_controller.PlayerNearAndLoS)
            OnStateChangePetitionHandler(EnemyStates.Idle);
    }

    public override void OnExitState()
    {
    }
}
