using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    Idle, Patrol, Persuit, Attack, Death
}

public class EnemyBase : MonoBehaviour, IDamageable
{
    [Header("Monster settings")]
    [SerializeField] private MonsterDatabase _monsterData;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private Animator _animation;

    private CharacterController _characterController;
    private AudioSource _audioSource;

    private float _currentLife;
    private float _currentIdleTime;
    private int _currentPatrolIndex;
    private List<EnemyStateBase> _enemyStates = new List<EnemyStateBase>();
    private EnemyStateBase _currentEnemyState;

    public GameObject _thePlayer;

    public MonsterDatabase MonsterData => _monsterData;
    public Transform[] PatrolPoints => _patrolPoints;
    public Animator Animation => _animation;

    public AudioSource SoundMainAudio => _audioSource;


    public virtual void AnyDamage(float amount)
    {
        _currentLife -= amount;
        print("New life: " + _currentLife);
    }

    public virtual void OnDeath()
    {
        UnsubscribeToStateChanges();
        print("ded");
        //Destroy(gameObject);
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;
        _currentEnemyState.OnExecute(deltaTime);
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();

        _currentLife = _monsterData.Life;
        _enemyStates = new List<EnemyStateBase>();
        InitializeStates();
    }

    public virtual void InitializeStates()
    {
        var idleState = new EnemyIdleState(_monsterData.IAMaxIdleTime, _animation, IsPlayerNear, _monsterData.IACanPatrol, _monsterData.IACanMove);
        var patrolState = new EnemyPatrolState(transform, _animation, _monsterData.MovementSpeed, IsPlayerNear, _characterController, _patrolPoints);
        var chaseState = new EnemyChaseState(transform, _animation, _thePlayer, _monsterData.MovementSpeed, IsPlayerNear, _characterController);
        //var deadState

        _enemyStates.Add(idleState);
        _enemyStates.Add(patrolState);
        _enemyStates.Add(chaseState);

        _currentEnemyState = idleState;
        SubscribeToStateChange();
    }

    public void CustomAddEnemyState(EnemyStateBase state, bool setAsDefault)
    {
        _enemyStates.Add(state);

        if (setAsDefault)
        {
            _currentEnemyState = state;
            SubscribeToStateChange();
        }
    }

    private void SubscribeToStateChange()
    {
        foreach (var state in _enemyStates)
            state.OnStateChangePetition += HandleStateChange;
    }

    private void UnsubscribeToStateChanges()
    {
        foreach (var state in _enemyStates)
            state.OnStateChangePetition -= HandleStateChange;
    }

    private void HandleStateChange(EnemyStates stateObj)
    {
        print("new state " + stateObj.ToString());
        EnemyStateBase newState;

        switch (stateObj)
        {
            case EnemyStates.Idle:
                if (!TryGetState<EnemyIdleState>(out newState))
                    throw new Exception($"State {stateObj} not found");
                break;
            case EnemyStates.Patrol:
                if (!TryGetState<EnemyPatrolState>(out newState))
                    throw new Exception($"State {stateObj} not found");
                break;
            case EnemyStates.Persuit:
                if (!TryGetState<EnemyChaseState>(out newState))
                    throw new Exception($"State {stateObj} not found");
                break;
            case EnemyStates.Death:
                if (!TryGetState<EnemyIdleState>(out newState))
                    throw new Exception($"State {stateObj} not found");
                break;
            default:
                throw new Exception($"Invalid state {stateObj}");
        }

        SetNewState(newState);
    }

    private bool TryGetState<T>(out EnemyStateBase newState) where T : EnemyStateBase
    {
        for (int i = 0; i < _enemyStates.Count; i++)
        {
            var state = _enemyStates[i];
            if (state is T enemyState)
            {
                newState = enemyState;
                return true;
            }
        }

        newState = default;
        return false;
    }

    private void SetNewState(EnemyStateBase newState)
    {
        _currentEnemyState?.OnExitState();
        _currentEnemyState = newState;
        _currentEnemyState.OnEnterState();
    }

    // add some attack melee and ranged stuff now

    public bool IsPlayerNear()
    {
        if (_thePlayer == null) return false;

        Transform selfTransform = this.transform;
        Vector3 playerPos = _thePlayer.transform.position;
        Vector3 direction = playerPos - selfTransform.position;

        bool isInVisionDistance = direction.magnitude <= _monsterData.VisionDistance;
        if (!isInVisionDistance) return false;

        RaycastHit hit;
        if (isInVisionDistance) 
        {
            if (Physics.Raycast(selfTransform.position, direction, out hit, _monsterData.VisionDistance))
            {
                if (hit.transform.gameObject == _thePlayer)
                {
                    Debug.DrawRay(selfTransform.position, direction * _monsterData.VisionDistance, Color.green);
                    return true;
                }
                Debug.DrawRay(selfTransform.position, direction * _monsterData.VisionDistance, Color.red);
            }
        }
        return false;
    }
}
