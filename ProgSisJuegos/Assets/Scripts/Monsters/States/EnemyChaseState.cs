using System;
using UnityEngine;

public class EnemyChaseState : EnemyStateBase
{
    private float _movementSpeed;

    private bool _facePlayerBeforeMoving;
    private float _facingTolerance;

    private Transform _selfTransform;
    private EnemyBase _controller;
    Func<Vector3> _getPlayerDirection;
    private Func<Animator> _getAnimator;
    private Func<CharacterController> _getController;


    public EnemyChaseState(Transform selfTransform, EnemyBase baseScript, bool mustFacePlayerBeforeMoving = false, float facingTolerance = 0.5f)
    {
        _selfTransform = selfTransform;
        _controller = baseScript;
        _facePlayerBeforeMoving = mustFacePlayerBeforeMoving;
        _facingTolerance = facingTolerance;

        _getAnimator = _controller.AnimationState;
        _getPlayerDirection = _controller.RequestingDirectionToPlayer;
        _getController = _controller.Move;

        _movementSpeed = _controller.MonsterData.MovementSpeed;
    }

    public override void OnEnterState()
    {
        if (_getPlayerDirection() == null || !_controller.PlayerNearAndLoS)
            OnStateChangePetitionHandler(EnemyStates.Idle);
    }

    public override void OnExecute(float deltaTime, float turnSpeed = 1)
    {
        if (_facePlayerBeforeMoving)
        {
            if ((_selfTransform.right - _getPlayerDirection()).magnitude <= _facingTolerance)
            {
                _getAnimator().SetBool("Walking", true);
                _getController().Move(_movementSpeed * deltaTime * _getPlayerDirection());
            }
        }
        
        else
        {
            _getController().Move(_movementSpeed * deltaTime * _getPlayerDirection());
            _getAnimator().SetBool("Walking", true);
        }

        // Look at
        _selfTransform.right = Vector3.LerpUnclamped(_selfTransform.right, _getPlayerDirection(), deltaTime * turnSpeed);
        _getAnimator().SetBool("Walking", true);

        if (_controller.IsOnMeleeAttackRange() && !_controller.IsOnRangedAttackRange()) 
            OnStateChangePetitionHandler(EnemyStates.Attack);

        else if (!_controller.IsOnMeleeAttackRange() && _controller.IsOnRangedAttackRange()) 
            OnStateChangePetitionHandler(EnemyStates.RangedAttack);
    }

    public override void OnExitState()
    {
        _getAnimator().SetBool("Walking", false);
    }
}
