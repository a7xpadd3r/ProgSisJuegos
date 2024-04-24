using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKitchenKnife : MonoBehaviour, IWeapon
{
    private AudioSource _audioSource;
    private Animator _anim;

    public Transform forwardReference;
    public List<AudioClip> _missAttackClips;
    public List<AudioClip> _hitAttackClips;

    public float weaponRecoil = 0.35f;
    public float hitRecoil = 0.1f;
    public float range = 2;

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

        if (!_canAttackAgain && _currentrecoil < weaponRecoil)
            _currentrecoil += Time.deltaTime;

        else if (!_canAttackAgain && _currentrecoil >= weaponRecoil)
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

    public void AttackMeleeRay()
    {
    }

    public void AttackMissSound()
    {
        if (Physics.Raycast(
                forwardReference.position,
                forwardReference.forward,
                out _rHit,
                range
            ))
            if (_rHit.transform.gameObject.layer == LayerMask.NameToLayer("Damageable"))
            {
                IDamageable damageable = _rHit.transform.gameObject.GetComponent<IDamageable>();
                damageable?.AnyDamage(1);
                _audioSource.PlayOneShot(_hitAttackClips[Random.Range(0, _hitAttackClips.Count)]);
                _currentrecoil += hitRecoil;
                return;
            }

        _audioSource.PlayOneShot(_missAttackClips[Random.Range(0, _missAttackClips.Count)]);        
    }

    public void AttackHitSound()
    {
        throw new System.NotImplementedException();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color= Color.yellow;
        Gizmos.DrawRay(forwardReference.position, forwardReference.right * range);
    }

    public float recoil { get => 0; set => throw new System.NotImplementedException(); }
}
