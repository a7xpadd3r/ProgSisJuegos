using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Wheelchair, Spitter, Ghost, Faster
}

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Data/Monsters")]
public class MonsterDatabase : ScriptableObject
{
    [SerializeField] private MonsterType _type;
    [SerializeField] private EnemyBase _monsterPrefab;

    [Header("Generic settings")]
    [SerializeField, Range(1, 100)] private float _life = 1;
    [SerializeField] private bool _canMove;
    [SerializeField, Range(0, 5)] private float _movementSpeed = 1.35f;
    [SerializeField, Range(0f, 10f)] private float _damageStunChance = 3.5f;
    [SerializeField, Range(0.1f, 10f)] private float _maxStunTime = 1;

    [Header("IA Settings")]
    [SerializeField, Range(1, 100)] private float _visionDistance = 5;
    [SerializeField, Range(0.1f, 2f)] private float _maxIdleTime = 1;
    [SerializeField, Range(0.1f, 10)] private float _turnSpeed = 1;
    [SerializeField] private bool _canIdle = true;
    [SerializeField] private bool _canPatrol;

    [Header("Melee settings")]
    [SerializeField] private bool _canMeleeAttack;
    [SerializeField, Range(0f, 10f)] private float _meleeDamage = 2;
    [SerializeField, Range(0f, 10f)] private float _attackChance;
    [SerializeField, Range(0.1f, 50)] private float _attackRange;
    [SerializeField, Range(0.1f, 10)] private float _attackCooldown;

    [Header("Ranged settings")]
    [SerializeField] private bool _canRangedAttack;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField, Range(0f, 10f)] private float _rangedDamage = 3.5f;
    [SerializeField, Range(0f, 10f)] private float _rangedAttackChance;
    [SerializeField, Range(0.1f, 50)] private float _rangedAttackRange;
    [SerializeField, Range(0.1f, 10)] private float _rangedAttackCooldown;

    [Header("Sound settings")]
    [SerializeField] private List<AudioClip> _idleClips;
    [SerializeField] private List<AudioClip> _movementClips;
    [SerializeField] private List<AudioClip> _movementAdditionalClips;
    [SerializeField] private List<AudioClip> _meleeAttackClips;
    [SerializeField] private List<AudioClip> _meleeHitAttackClips;
    [SerializeField] private List<AudioClip> _rangedAttackClips;
    [SerializeField] private List<AudioClip> _rangedHitAttackClips;
    [SerializeField] private List<AudioClip> _damagedClips;
    [SerializeField] private List<AudioClip> _deathClips;


    public MonsterType EnemyType => _type;
    public EnemyBase MonsterPrefab => _monsterPrefab;

    // Pawn
    public float Life => _life;
    public float MovementSpeed => _movementSpeed;
    public float DamageStunChance => _damageStunChance;
    public float MaxStunTime => _maxStunTime;

    // IA
    public float IAVisionDistance => _visionDistance;
    public float IAMaxIdleTime => _maxIdleTime;
    public float IATurnSpeed => _turnSpeed;
    public bool IACanMove => _canMove;
    public bool IACanPatrol => _canPatrol;
    public bool IACanIdle => _canIdle;
    public bool IACanUseMeleeAttack => _canMeleeAttack;
    public bool IACanUseRangedAttack => _canRangedAttack;

    // Attack
    public float DamageMelee => _meleeDamage;
    public float DamageRanged => _rangedDamage;

    public float AttackChance => _attackChance;
    public float AttackRange => _attackRange;
    public float AttackCooldown => _attackCooldown;

    public float RangedAttackChance => _rangedAttackChance;
    public GameObject ProjectilePrefab => _projectilePrefab;
    public float RangedAttackRange => _rangedAttackRange;
    public float RangedAttackCooldown => _rangedAttackCooldown;


    // Sounds
    public List<AudioClip> SoundsIdle => _idleClips;
    public List<AudioClip> SoundsMovement => _movementClips;
    public List<AudioClip> SoundsMovementAdditional => _movementAdditionalClips;
    public List<AudioClip> SoundsMeleeAttack => _meleeAttackClips;
    public List<AudioClip> SoundsMeleeHitAttack => _meleeHitAttackClips;
    public List<AudioClip> SoundsRangedAttack => _rangedAttackClips;
    public List<AudioClip> SoundsRangedHitAttack => _rangedHitAttackClips;
    public List<AudioClip> SoundsGetDamage => _damagedClips;
    public List<AudioClip> SoundsDeath => _deathClips;

    public AudioClip GetRandomClip(EnemyStates state)
    {
        AudioClip value = null;

        switch (state)
        {
            case EnemyStates.Idle:
                if (SoundsIdle.Count > 0)
                    value = SoundsIdle[Random.Range(0, SoundsIdle.Count - 1)];
                break;

            case EnemyStates.Patrol:
                if (SoundsMovement.Count > 0)
                    value = SoundsMovement[Random.Range(0, SoundsMovement.Count - 1)];
                break;

            case EnemyStates.Persuit:
                if (SoundsMovement.Count > 0)
                    value = SoundsMovement[Random.Range(0, SoundsMovement.Count - 1)];
                break;

            case EnemyStates.Attack:
                if (SoundsMeleeAttack.Count > 0)
                    value = SoundsMeleeAttack[Random.Range(0, SoundsMeleeAttack.Count - 1)];
                break;

            case EnemyStates.RangedAttack:
                if (SoundsRangedAttack.Count > 0)
                    value = SoundsRangedAttack[Random.Range(0, SoundsRangedAttack.Count - 1)];
                break;

            case EnemyStates.Damaged:
                if (SoundsGetDamage.Count > 0)
                    value = SoundsGetDamage[Random.Range(0, SoundsGetDamage.Count - 1)];
                break;

            case EnemyStates.Death:
                if (SoundsDeath.Count > 0)
                    value = SoundsDeath[Random.Range(0, SoundsDeath.Count - 1)];
                break;
        }

        return value;
    }

    // the idea is to use another source for steps n' additional stuff
    public AudioClip GetRandomAdditionalClip(AdditionalSounds type)
    {
        AudioClip value = null;

        switch (type)
        {
            case AdditionalSounds.Movement:
                if (SoundsMovementAdditional.Count > 0)
                    value = SoundsMovementAdditional[Random.Range(0, SoundsMovementAdditional.Count - 1)];
                break;
        }

        return value;
    }
}
