using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponDatabase _weaponData;
    public AudioSource _audioSource;
    public Animator _anim;

    public WeaponDatabase WeaponData => _weaponData;

    public abstract void Attack();
    public virtual void AttackHitSound() { }
    public virtual void AttackMissSound() { }
    public abstract void AttackRay();
    public virtual void Reload() { }

}
