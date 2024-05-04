using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyStateBase
{
    private float _cooldown;
    private float _currentCooldown;

    // References
    private Func<bool> _isAttackExecuted;
    private Func<Animator> _getAnimator;
    private Func<Vector3> _playerDirection;
    private Transform _selfTransform;
    private EnemyBase _controller;

    public EnemyAttackState(Transform selfTransform, EnemyBase baseScript)
    {
        _controller = baseScript;
        _cooldown = _controller.MonsterData.AttackCooldown;
        _getAnimator = _controller.AnimationState;
        _playerDirection = _controller.RequestingDirectionToPlayer;
        _isAttackExecuted = _controller.ExecMeleeAttack;

        _selfTransform= selfTransform;
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
                _getAnimator().SetTrigger("MeleeAttack");

            _currentCooldown = _cooldown;
        }

        else
            _currentCooldown -= deltaTime;

        if (!_controller.PlayerNearAndLoS)
            OnStateChangePetitionHandler(EnemyStates.Idle);

        if (!_controller.IsOnMeleeAttackRange() && _controller.MonsterData.IACanMove)
            OnStateChangePetitionHandler(EnemyStates.Persuit);
    }

    public override void OnExitState()
    {
    }
}
