using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    Idle, Patrol, Persuit, Attack, RangedAttack, Damaged, Death
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
    public List<EnemyStateBase> _enemyStates = new List<EnemyStateBase>();
    private EnemyStateBase _currentEnemyState;

    public GameObject _thePlayer;
    public Transform PlayerTransform => _thePlayer.transform;
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
        _currentEnemyState.OnExecute(deltaTime, MonsterData.IATurnSpeed);
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();

        _currentLife = MonsterData.Life;
        _enemyStates = new List<EnemyStateBase>();
        InitializeStates();
    }

    public virtual void InitializeStates()
    {
        // Generic empty state list
        // Idle, Patrol, Persuit, Attack, RangedAttack, Damaged, Death

        var idleState = new EnemyIdleState();
        var patrolState = new EnemyPatrolState();
        var chaseState = new EnemyChaseState();
        var meleeState = new EnemyAttackState();
        var rangedState = new EnemyRangedState();
        var damagedState = new EnemyDamagedState();
        var deathState = new EnemyDeathState();


        _enemyStates.Add(idleState);
        _enemyStates.Add(patrolState);
        _enemyStates.Add(chaseState);
        _enemyStates.Add(meleeState);
        _enemyStates.Add(rangedState);
        _enemyStates.Add(damagedState);
        _enemyStates.Add(deathState);

        _currentEnemyState = idleState;
        SubscribeToStateChange();
    }

    public void SetDefaultStateAndSubscribeToStates(EnemyStateBase state)
    {
        _currentEnemyState = state;
        SubscribeToStateChange();
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
            case EnemyStates.Attack:
                if (!TryGetState<EnemyAttackState>(out newState))
                    throw new Exception($"State {stateObj} not found");
                break;
            case EnemyStates.RangedAttack:
                if (!TryGetState<EnemyRangedState>(out newState))
                    throw new Exception($"State {stateObj} not found");
                break;
            case EnemyStates.Damaged:
                if (!TryGetState<EnemyDamagedState>(out newState))
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
        if (_thePlayer == null || (!MonsterData.IACanUseMeleeAttack && !MonsterData.IACanUseRangedAttack)) return false;

        Transform selfTransform = this.transform;
        Vector3 playerPos = _thePlayer.transform.position;
        Vector3 direction = playerPos - selfTransform.position;

        bool isInVisionDistance = direction.magnitude <= MonsterData.IAVisionDistance;
        if (!isInVisionDistance) return false;

        RaycastHit hit;
        if (isInVisionDistance) 
        {
            if (Physics.Raycast(selfTransform.position, direction, out hit, MonsterData.IAVisionDistance))
            {
                if (hit.transform.gameObject == _thePlayer)
                {
                    Debug.DrawRay(selfTransform.position, direction * MonsterData.IAVisionDistance, Color.green);
                    return true;
                }
                Debug.DrawRay(selfTransform.position, direction * MonsterData.IAVisionDistance, Color.red);
            }
        }
        return false;
    }

    public Animator AnimationState()
    {
        if (_animation == null) return null; 
        return _animation;
    }

    public CharacterController Move()
    {
        if (!MonsterData.IACanMove || MonsterData.MovementSpeed <= 0 || _characterController == null) return null;
        return _characterController;
    }

    public bool IsOnMeleeAttackRange()
    {
        if (!MonsterData.IACanUseMeleeAttack) return false;
        return (_thePlayer.transform.position - this.transform.position).magnitude <= MonsterData.AttackRange;
    }

    public bool IsOnRangedAttackRange()
    {
        if (!MonsterData.IACanUseRangedAttack) return false;
        return (_thePlayer.transform.position - this.transform.position).magnitude <= MonsterData.RangedAttackRange;
    }

    public Vector3 RequestingDirectionToPlayer()
    {
        Vector3 dir = _thePlayer.transform.position - transform.position;
        Vector3 dirNormalized = dir.normalized;
        Vector3 dirNoYaxis = new Vector3(dirNormalized.x, 0, dirNormalized.z);
        return dirNoYaxis;
    }

    public bool ExecRangedAttack()
    {
        print("ranged attack?");
        return true;
    }
    
}
