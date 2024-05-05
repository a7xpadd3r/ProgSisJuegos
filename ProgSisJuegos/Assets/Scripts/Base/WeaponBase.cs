using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] private WeaponDatabase _weaponData;
    public AudioSource _audioSource;
    public Animator _anim;

    private bool _isSwitchingWeapon = false;

    public WeaponDatabase WeaponData => _weaponData;
    public bool IsSwitchingWeapon => _isSwitchingWeapon;

    public abstract void Attack();
    public virtual void AttackHitSound() { }
    public virtual void AttackMissSound() { }
    public abstract void AttackRay();
    public virtual void Reload() { }
    public virtual void WeaponIn()
    {
        _isSwitchingWeapon = false;
    }
    public virtual void WeaponOut()
    {
        _isSwitchingWeapon = true;
        _anim.SetTrigger("Out");
    }
}
