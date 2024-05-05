using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    Idle, Patrol, Persuit, Attack, RangedAttack, Damaged, Death
}

public class EnemyBase : MonoBehaviour, IDamageable, IProduct
{
    [Header("Monster settings")]
    [SerializeField] private MonsterDatabase _monsterData;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private Animator _animation;
    [SerializeField] private Transform _projectileOut;

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
    public bool PlayerNearAndLoS => IsPlayerNear();
    public bool IsAlive => _currentLife > 0;

    public virtual void AnyDamage(float amount)
    {
        _currentLife -= amount;

        if (_currentLife <= 0)
        {
            OnDeath();
            return;
        }

        if (_currentLife > 0 && UnityEngine.Random.Range(0.1f, 10) <= _monsterData.DamageStunChance)
            OnStun();
    }

    public virtual void OnStun()
    {
        HandleStateChange(EnemyStates.Damaged);
    }

    public virtual void OnDeath()
    {
        UnsubscribeToStateChanges();
        HandleStateChange(EnemyStates.Death);
        _characterController.enabled = false;
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
        /*
         * Generic empty state list example
         * Idle, Patrol, Persuit, Attack, RangedAttack, Damaged, Death

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
        
      */
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
                if (!TryGetState<EnemyDeathState>(out newState))
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

    private bool IsPlayerNear()
    {
        // If this is a "neutral pawn"
        if (_thePlayer == null || (!MonsterData.IACanUseMeleeAttack && !MonsterData.IACanUseRangedAttack)) return false;

        // Check distance
        Transform selfTransform = this.transform;
        Vector3 playerPos = new Vector3(_thePlayer.transform.position.x, _thePlayer.transform.position.y + 0.25f, _thePlayer.transform.position.z);
        Vector3 direction = playerPos - selfTransform.position;

        bool isInVisionDistance = direction.magnitude <= MonsterData.IAVisionDistance;
        if (!isInVisionDistance) return false;

        // Check LoS
        return IsPlayerOnLoS(selfTransform.position, direction, MonsterData.IAVisionDistance);
    }

    private bool IsPlayerOnLoS(Vector3 rayStartingPos, Vector3 direction, float maxRange)
    {
        RaycastHit hit;

        if (Physics.Raycast(rayStartingPos, direction, out hit, maxRange))
        {
            if (hit.transform.gameObject == _thePlayer)
                return true;
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
        return new Vector3(dirNormalized.x, 0, dirNormalized.z);
    }

    public bool ExecMeleeAttack()
    {
        if (UnityEngine.Random.Range(0.1f, 10) > MonsterData.AttackChance) return false;

        // Find player and apply damage (maybe TODO damage everyone?)
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, MonsterData.AttackRange);
        for (int i = 0; i < objectsInRange.Length; i++)
        {
            if (objectsInRange[i].CompareTag("Player"))
                objectsInRange[i].GetComponent<IDamageable>()?.AnyDamage(MonsterData.DamageMelee);
        }

        return true;
    }

    public bool ExecRangedAttack()
    {
        if (UnityEngine.Random.Range(0.1f, 10) > MonsterData.RangedAttackChance) return false;

        var projectile = Instantiate(MonsterData.ProjectilePrefab, _projectileOut.position, _projectileOut.rotation);
        projectile.TryGetComponent(out ProjectileBase projectileScript);

        if (projectileScript != null)
        {
            projectileScript.direction = (_thePlayer.transform.position - transform.position).normalized;
            projectileScript.damage = MonsterData.DamageRanged;
            return true;
        }

        return false;
    }
    
}
