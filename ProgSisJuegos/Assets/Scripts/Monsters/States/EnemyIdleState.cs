using System;
using System.Diagnostics;
using System.Drawing.Printing;
using UnityEngine;

public class EnemyIdleState : EnemyStateBase
{
    private float _idleTimer;
    private float _idleWaitTime;
    private bool _hasPatrolling;
    private bool _hasChasing;

    // References
    private Func<Animator> _getAnimator;

    // Conditions
    private Func<bool> _getIsPlayerNear;
    private Func<bool> _getOnMeleeAttackRange;
    private Func<bool> _getOnRangedAttackRange;

    public EnemyIdleState() 
    { 
        //Empty state                              
    }

    public EnemyIdleState(float idleWaitTime, Func<Animator> getAnimation, Func<bool> getIsPlayerNear, bool hasPatrollingState, bool hasChasingState, Func<bool> onMeleeRange, Func<bool> onRangedRange)
    {
        _idleWaitTime = idleWaitTime;
        _getAnimator = getAnimation;
        _getIsPlayerNear = getIsPlayerNear;
        _hasPatrolling = hasPatrollingState;
        _hasChasing = hasChasingState;
        _getOnMeleeAttackRange = onMeleeRange;
        _getOnRangedAttackRange = onRangedRange;
    }

    public override void OnEnterState()
    {
        _idleTimer = 0;
        _getAnimator()?.SetBool("Walking", false);
    }

    public override void OnExecute(float deltaTime, float turnSpeed = 1)
    {
        _idleTimer += deltaTime;

        bool isPlayerNear = _getIsPlayerNear();
        UnityEngine.Debug.Log("IdleTimer - is player near: " + isPlayerNear);

        if (isPlayerNear)
        {
            // Chase if not on range
            if (_hasChasing && !_getOnMeleeAttackRange() && !_getOnRangedAttackRange())
                OnStateChangePetitionHandler(EnemyStates.Persuit);

            // Do melee attack if not on ranged one
            if (_getOnMeleeAttackRange() && !_getOnRangedAttackRange())
                OnStateChangePetitionHandler(EnemyStates.Attack);

            // Do ranged attack if not on melee one
            if (!_getOnMeleeAttackRange() && _getOnRangedAttackRange())
                OnStateChangePetitionHandler(EnemyStates.RangedAttack);

            // TODO - random poss between attacks
            return;
        }

        if (_idleTimer >= _idleWaitTime) 
        {          
            if (_hasPatrolling)
                OnStateChangePetitionHandler(EnemyStates.Patrol);
            else
                _idleTimer = 0;
        }
    }

    public override void OnExitState()
    {
    }
}
