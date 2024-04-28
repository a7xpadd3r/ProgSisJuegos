using System;
using UnityEngine;

public class EnemyChaseState : EnemyStateBase
{
    private Transform _selfTransform;
    private Animator _animation;
    private GameObject _thePlayer;
    private float _movementSpeed;
    private Func<bool> _getIsPlayerNear;
    private CharacterController _cController;

    public EnemyChaseState(Transform selfTransform, Animator animation, GameObject thePlayer, float movementSpeed, Func<bool> getIsPlayerNear, CharacterController cController)
    {
        _selfTransform = selfTransform;
        _animation = animation;
        _thePlayer = thePlayer;
        _movementSpeed = movementSpeed;
        _getIsPlayerNear = getIsPlayerNear;
        _cController = cController;
    }

    public override void OnEnterState()
    {
        if (_thePlayer == null || !_getIsPlayerNear())
            OnStateChangePetitionHandler(EnemyStates.Idle);
    }

    public override void OnExecute(float deltaTime)
    {
        Vector3 dir = _thePlayer.transform.position - _selfTransform.position;
        Vector3 dirNormalized = dir.normalized;
        Vector3 dirNoYaxis = new Vector3(dirNormalized.x, 0, dirNormalized.z);

        // Move
        _animation.SetBool("Walking", true);
        _cController.Move(_movementSpeed * deltaTime * dirNoYaxis);

        // Look at
        _selfTransform.right = Vector3.LerpUnclamped(_selfTransform.right, dirNoYaxis, deltaTime);
    }

    public override void OnExitState()
    {
        
    }
}
