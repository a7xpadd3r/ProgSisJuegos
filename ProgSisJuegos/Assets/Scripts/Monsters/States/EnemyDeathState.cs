using System;
using UnityEngine;

public class EnemyDeathState : EnemyStateBase
{
    private Func<Animator> _getAnimator;

    public EnemyDeathState(Func<Animator> animation)
    {
        _getAnimator = animation;
    }

    public override void OnEnterState()
    {
        _getAnimator().SetTrigger("Death");
    }

    public override void OnExecute(float deltaTime, float turnSpeed = 1)
    {
    }

    public override void OnExitState()
    {
    }
}
