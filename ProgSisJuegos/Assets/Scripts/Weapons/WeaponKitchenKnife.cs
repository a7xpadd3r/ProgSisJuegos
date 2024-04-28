using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKitchenKnife : MonoBehaviour
{
    [SerializeField] private WeaponDatabase _weaponData;

    private AudioSource _audioSource;
    private Animator _anim;
    public Transform forwardReference;

    public RaycastHit _rHit;
    public LayerMask damageableMask;

    private float _currentrecoil;
    private bool _canAttackAgain = true;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _canAttackAgain)
            Attack();

        if (!_canAttackAgain && _currentrecoil < _weaponData.Recoil)
            _currentrecoil += Time.deltaTime;

        else if (!_canAttackAgain && _currentrecoil >= _weaponData.Recoil)
            _canAttackAgain = true;
    }

    public void Attack()
    {
        if (_canAttackAgain)
        {
            _anim.SetTrigger("Attack0" + Random.Range(1, 3).ToString());

            _canAttackAgain = false;
            _currentrecoil = 0;
        }
    }

    public void AttackRay()
    {
        if (Physics.Raycast(
        forwardReference.position,
        forwardReference.forward,
        out _rHit,
        _weaponData.Range
        ))
            if (_rHit.transform.gameObject.layer == LayerMask.NameToLayer("Damageable"))
            {
                IDamageable damageable = _rHit.transform.gameObject.GetComponent<IDamageable>();
                damageable?.AnyDamage(1);
                _currentrecoil += _weaponData.HitRecoil;

                if (_rHit.transform.gameObject.CompareTag("Enemy")) AttackHitSound();
                else if (_rHit.transform.gameObject.CompareTag("Breakable")) AttackMissSound();
                return;
            }
        AttackMissSound();
    }

    public void AttackMissSound()
    {
        _audioSource.PlayOneShot(_weaponData.SoundMissAttackFire[Random.Range(0, _weaponData.SoundMissAttackFire.Count)]);        
    }

    public void AttackHitSound()
    {
        _audioSource.PlayOneShot(_weaponData.SoundHitAttackFire[Random.Range(0, _weaponData.SoundHitAttackFire.Count)]);
    }

    public void Reload() { }
}
