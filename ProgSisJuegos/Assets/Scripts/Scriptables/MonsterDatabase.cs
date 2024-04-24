using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterDatabase", menuName = "Data/Monsters")]
public class MonsterDatabase : ScriptableObject
{
    [Header("Generic settings")]
    [SerializeField] private float _life;
    [SerializeField, Range(0, 5)] private float _movementSpeed;
    [SerializeField, Range(0f, 10f)] private float _damageStunChance;

    [Header("Melee settings")]
    [SerializeField, Range(0f, 10f)] private float _attackChance;
    [SerializeField] private bool _canMeleeAttack;    
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackCooldown;

    [Header("Ranged settings")]
    [SerializeField, Range(0f, 10f)] private float _rangedAttackChance;
    [SerializeField] private bool _canRangedAttack;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _rangedAttackRange;
    [SerializeField] private float _rangedAttackCooldown;

    [Header("Sound settings")]
    [SerializeField] private List<AudioClip> _idleClips;
    [SerializeField] private List<AudioClip> _movementClips;
    [SerializeField] private List<AudioClip> _meleeAttackClips;
    [SerializeField] private List<AudioClip> _rangedAttackClips;
    [SerializeField] private List<AudioClip> _damagedClips;
    [SerializeField] private List<AudioClip> _deathClips;


    // Pawn
    public float Life => _life;
    public float MovementSpeed => _movementSpeed;
    public float DamageStunChance => _damageStunChance;

    // Main conditions
    public bool CanUseMeleeAttack => _canMeleeAttack;
    public bool CanUseRangedAttack => _canRangedAttack;

    // Attack
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
    public List<AudioClip> SoundsMeleeAttack => _meleeAttackClips;
    public List<AudioClip> SoundsRangedAttack => _rangedAttackClips;
    public List<AudioClip> SoundsGetDamage => _damagedClips;
    public List<AudioClip> SoundsDeath => _deathClips;
}
