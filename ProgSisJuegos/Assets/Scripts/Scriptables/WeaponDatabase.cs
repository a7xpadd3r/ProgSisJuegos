using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Data/Weapons")]
public class WeaponDatabase : ScriptableObject
{
    [SerializeField] private WeaponTypes _weaponType;
    [SerializeField] private Sprite _weaponSprite;

    [Header("Weapon settings")]
    [SerializeField] private float _damage;
    [SerializeField] private float _range;
    [SerializeField] private float _recoil;
    [SerializeField] private float _hitRecoil;
    [SerializeField] private int _bullets;

    [Header("Sound settings")]
    [SerializeField] private AudioClip _weaponInSound;
    [SerializeField] private AudioClip _weaponOutSound;
    [SerializeField] private List<AudioClip> _attackFireSound;
    [SerializeField] private List<AudioClip> _noBulletsSound;
    [SerializeField] private List<AudioClip> _missAttackFireSound;
    [SerializeField] private List<AudioClip> _hitAttackFireSound;
    [SerializeField] private List<AudioClip> _hitWallAttackFireSound;
    [SerializeField] private AudioClip _reloadStartSound;
    [SerializeField] private AudioClip _reloadEndSound;

    // Values
    public WeaponTypes Type => _weaponType;
    public float Damage => _damage;
    public float Range => _range;
    public float Recoil => _recoil;
    public float HitRecoil => _hitRecoil;
    public int Bullets => _bullets;

    // Sounds
    public AudioClip SoundWeaponUp => _weaponInSound;
    public AudioClip SoundWeaponOut => _weaponOutSound;
    public List<AudioClip> SoundAttackFire => _attackFireSound;
    public List<AudioClip> SoundNoBullets => _noBulletsSound;
    public List<AudioClip> SoundMissAttackFire => _missAttackFireSound;
    public List<AudioClip> SoundHitAttackFire => _hitAttackFireSound;
    public AudioClip SoundStartReload => _reloadStartSound;
    public AudioClip SoundEndReload => _reloadEndSound;

    // Events
    public Action Attack;

}
