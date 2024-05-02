using System;
using System.Diagnostics;
using System.Drawing.Printing;
using UnityEngine;

public class EnemyIdleState : EnemyStateBase
{
    private float _idleTimer;

    // References
    private Func<Animator> _getAnimator;

    // Conditions
    EnemyBase _controller;

    public EnemyIdleState() 
    { 
        //Empty state                              
    }

    public EnemyIdleState(Func<Animator> getAnimation, EnemyBase baseScript)
    {
        _getAnimator = getAnimation;
        _controller = baseScript;
    }

    public override void OnEnterState()
    {
        _idleTimer = 0;
        _getAnimator()?.SetBool("Walking", false);
    }

    public override void OnExecute(float deltaTime, float turnSpeed = 1)
    {
        _idleTimer += deltaTime;

        if (_controller.PlayerNearAndLoS)
        {
            // Chase if not on range
            if (_controller.MonsterData.IACanMove && !_controller.IsOnMeleeAttackRange() && !_controller.IsOnRangedAttackRange())
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
