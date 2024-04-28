using System;
using UnityEngine;

public class EnemyPatrolState : EnemyStateBase
{
    private Transform _selfTransform;
    private Animator _animation;
    private float _movementSpeed;
    private Func<bool> _getIsPlayerNear;
    private CharacterController _cController;

    private Transform[] _patrolPoints;
    private Transform[] _playerSightLocations;
    private Transform _currentTravelTransform;
    private int _currentPatrolIndex;
    private bool _isReversePatrol;
    private float _travelMinDistance = 2;

    public EnemyStates caca = EnemyStates.Idle;

    public EnemyPatrolState(Transform selfTransform, Animator animation, float movementSpeed, Func<bool> getIsPlayerNear, CharacterController cController, Transform[] patrolPoints)
    {
        _selfTransform = selfTransform;
        _animation = animation;
        _movementSpeed = movementSpeed;
        _getIsPlayerNear = getIsPlayerNear;
        _cController = cController;
    }

    public override void OnEnterState()
    {
        if (_getIsPlayerNear())
            OnStateChangePetitionHandler(EnemyStates.Persuit);

        if (_patrolPoints == null || _patrolPoints.Length <= 0)
        {
            OnStateChangePetitionHandler(EnemyStates.Idle);
            return;
        }

        _currentTravelTransform = _patrolPoints[0];
    }

    public override void OnExecute(float deltaTime)
    {
        if (_patrolPoints == null || _patrolPoints.Length <= 0)
        {
            OnStateChangePetitionHandler(EnemyStates.Idle);
            return;
        }

        if (_getIsPlayerNear())
        {
            OnStateChangePetitionHandler(EnemyStates.Persuit);
            return;
        }

        Vector3 dir = _currentTravelTransform.position - _selfTransform.transform.position;
        Vector3 dirNormalized = dir.normalized;
        Vector3 dirNoYaxis = new Vector3(dirNormalized.x, 0, dirNormalized.z);

        // Move
        _animation.SetBool("Walking", true);
        _cController.Move(_movementSpeed * deltaTime * dirNoYaxis);

        // Look at
        _selfTransform.right = Vector3.LerpUnclamped(_selfTransform.right, dirNoYaxis, deltaTime);

        if (IsCloseToPoint(_currentTravelTransform.position, _travelMinDistance))
        {
            _currentTravelTransform = _patrolPoints[_currentPatrolIndex];

            // Normal patrol/ reverse patrol
            if (!_isReversePatrol)
            {
                if (_currentPatrolIndex >= _patrolPoints.Length - 1)
                {
                    _isReversePatrol = true;
                    _currentPatrolIndex = _patrolPoints.Length - 1;
                    OnStateChangePetitionHandler(EnemyStates.Idle);
                    return;
                }
                _currentPatrolIndex++;
            }

            else
            {
                if (_currentPatrolIndex <= 0)
                {
                    _isReversePatrol = false;
                    _currentPatrolIndex = 1;
                    OnStateChangePetitionHandler(EnemyStates.Idle);
                    return;
                }
                _currentPatrolIndex--;
            }

            // TODO - add player sight location to "investigate"
        }
    }

    public override void OnExitState()
    {
        
    }

    private bool IsCloseToPoint(Vector3 destination, float tolerance)
    {
        return (_selfTransform.position - destination).magnitude <= tolerance;
    }
}
