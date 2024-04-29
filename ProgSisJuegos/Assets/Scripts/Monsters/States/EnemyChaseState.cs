using System;
using UnityEngine;

public class EnemyChaseState : EnemyStateBase
{
    private Transform _selfTransform;
    private float _movementSpeed;

    private Func<bool> _getIsPlayerNear;
    Func<Vector3> _getPlayerDirection;
    private Func<Animator> _getAnimator;
    private Func<CharacterController> _getController;

    public EnemyChaseState()
    {
        // Empty state
    }

    public EnemyChaseState(Transform selfTransform, Func<Animator> animation, Func<Vector3> playerDirection, float movementSpeed, Func<bool> getIsPlayerNear, Func<CharacterController> cController)
    {
        _selfTransform = selfTransform;
        _getAnimator = animation;
        _getPlayerDirection = playerDirection;
        _movementSpeed = movementSpeed;
        _getIsPlayerNear = getIsPlayerNear;
        _getController = cController;
    }

    public override void OnEnterState()
    {
        if (_getPlayerDirection() == null || !_getIsPlayerNear())
            OnStateChangePetitionHandler(EnemyStates.Idle);
    }

    public override void OnExecute(float deltaTime, float turnSpeed = 1)
    {
        // Move
        _getAnimator().SetBool("Walking", true);
        _getController().Move(_movementSpeed * deltaTime * _getPlayerDirection());

        // Look at
        _selfTransform.right = Vector3.LerpUnclamped(_selfTransform.right, _getPlayerDirection(), deltaTime * turnSpeed);
    }

    public override void OnExitState()
    {
        
    }
}
