using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKitchenKnife : WeaponBase
{
    public Transform forwardReference;

    public RaycastHit _rHit;
    public LayerMask damageableMask;

    private float _currentrecoil;
    private bool _canAttackAgain = true;

    public bool CanAttackAgain => _canAttackAgain;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!_canAttackAgain && _currentrecoil < WeaponData.Recoil) _currentrecoil += Time.deltaTime;
        else if (!_canAttackAgain && _currentrecoil >= WeaponData.Recoil) _canAttackAgain = true;

        if (IsSwitchingWeapon) return;
        if (Input.GetKeyDown(KeyCode.Mouse0) && _canAttackAgain) Attack();
    }

    public override void Attack()
    {
        if (_canAttackAgain)
        {
            _anim.SetTrigger("Attack0" + Random.Range(1, 3).ToString());

            _canAttackAgain = false;
            _currentrecoil = 0;
        }
    }

    public override void AttackRay()
    {
        if (Physics.Raycast(
        forwardReference.position,
        forwardReference.forward,
        out _rHit,
        WeaponData.Range
        ))
            if (_rHit.transform.gameObject.layer == LayerMask.NameToLayer("Damageable"))
            {
                IDamageable damageable = _rHit.transform.gameObject.GetComponent<IDamageable>();
                damageable?.AnyDamage(1);
                _currentrecoil += WeaponData.HitRecoil;

                if (_rHit.transform.gameObject.CompareTag("Enemy")) AttackHitSound();
                else if (_rHit.transform.gameObject.CompareTag("Breakable")) AttackMissSound();
                return;
            }
        AttackMissSound();
    }

    public override void AttackMissSound()
    {
        _audioSource.PlayOneShot(WeaponData.SoundMissAttackFire[Random.Range(0, WeaponData.SoundMissAttackFire.Count)]);        
    }

    public override void AttackHitSound()
    {
        _audioSource.PlayOneShot(WeaponData.SoundHitAttackFire[Random.Range(0, WeaponData.SoundHitAttackFire.Count)]);
    }
}
