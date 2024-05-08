using System;
using UnityEngine;

public class EnemyIdleState : EnemyStateBase
{
    private float _idleTimer;

    // References
    private Func<Animator> _getAnimator;
    private EnemyBase _controller;

    public EnemyIdleState(EnemyBase baseScript)
    {
        _controller = baseScript;
        _getAnimator = _controller.AnimationState;
    }

    public override void OnEnterState()
    {
        if (!_controller.MonsterData.IACanIdle)
            OnStateChangePetitionHandler(EnemyStates.Persuit);

        _idleTimer = 0;
        _getAnimator()?.SetBool("Walking", false);
    }

    public override void OnExecute(float deltaTime, float turnSpeed = 1)
    {
        _idleTimer += deltaTime;

        // If monster can't idle, then try persuit
        if (!_controller.MonsterData.IACanIdle)
        {
            OnStateChangePetitionHandler(EnemyStates.Persuit);
            return;
        }

        if (_controller.PlayerNearAndLoS)
        {
            // Chase if not on range
            if (!_controller.IsOnMeleeAttackRange() && !_controller.IsOnRangedAttackRange() && _controller.MonsterData.IACanMove)
                OnStateChangePetitionHandler(EnemyStates.Persuit);

            // Do melee attack if not on ranged one
            if (_controller.IsOnMeleeAttackRange() && !_controller.IsOnRangedAttackRange())
                OnStateChangePetitionHandler(EnemyStates.Attack);

            // Do ranged attack if not on melee one
            if (!_controller.IsOnMeleeAttackRange() && _controller.IsOnRangedAttackRange())
                OnStateChangePetitionHandler(EnemyStates.RangedAttack);

            // TODO - random poss between attacks
            return;
        }

        if (_idleTimer >= _controller.MonsterData.IAMaxIdleTime) 
        {          
            if (_controller.MonsterData.IACanPatrol)
                OnStateChangePetitionHandler(EnemyStates.Patrol);
            else
                _idleTimer = 0;
        }
    }

    public override void OnExitState()
    {
    }
}
