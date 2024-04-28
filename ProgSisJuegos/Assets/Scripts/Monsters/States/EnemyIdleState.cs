using System;
using System.Diagnostics;
using System.Drawing.Printing;
using UnityEngine;

public class EnemyIdleState : EnemyStateBase
{
    private float _idleTimer;
    private float _idleWaitTime;
    private Animator _animation;
    private Func<bool> _getIsPlayerNear;
    private bool _hasPatrolling;
    private bool _hasChasing;

    public EnemyIdleState(float idleWaitTime, Animator animation, Func<bool> getIsPlayerNear, bool hasPatrollingState, bool hasChasingState)
    {
        _idleWaitTime = idleWaitTime;
        _animation = animation;
        _getIsPlayerNear = getIsPlayerNear;
        _hasPatrolling = hasPatrollingState;
        _hasChasing = hasChasingState;
    }

    public override void OnEnterState()
    {
        _idleTimer = 0;
        _animation.SetBool("Walking", false);
    }

    public override void OnExecute(float deltaTime)
    {
        _idleTimer += deltaTime;

        if (_idleTimer >= _idleWaitTime) 
        { 
            bool isPlayerNear = _getIsPlayerNear();

            if (isPlayerNear) 
            {
                if (_hasChasing)
                    OnStateChangePetitionHandler(EnemyStates.Persuit);
                else
                    OnStateChangePetitionHandler(EnemyStates.Attack);

                return;
            }

            if (_hasPatrolling)
                OnStateChangePetitionHandler(EnemyStates.Patrol);
        }
    }

    public override void OnExitState()
    {
    }
}
