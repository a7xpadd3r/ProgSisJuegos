using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGlock : WeaponBase
{
    public Transform forwardReference;
    public float _currentRecoil;
    public int _currentBullets;

    public bool _canShootAgain = true;
    public bool _isReloading = false;

    [Range(0.01f, 0.1f)] public float muzzleDuration = 0.025f;
    private float _currentMuzzleDuration;

    [Header("Muzzle")]
    public GameObject muzzleLight;
    public GameObject muzzleSprite;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _currentBullets = WeaponData.Bullets;
    }

    public override void Attack()
    {
        if (_currentBullets > 0)
        {
            _canShootAgain = false;
            _currentRecoil = WeaponData.Recoil;
            _anim.SetTrigger("Fire");
            _currentBullets -= 1;
            _audioSource.PlayOneShot(WeaponData.SoundAttackFire[0]);
            AttackRay();
        }
        else
            _audioSource.PlayOneShot(WeaponData.SoundNoBullets[0]);
    }

    public override void AttackRay()
    {
        RaycastHit rHit;

        if (Physics.Raycast(
            forwardReference.position, forwardReference.forward, out rHit, 25))

            if (rHit.transform.gameObject.layer == LayerMask.NameToLayer("Damageable"))
            {
                IDamageable damageable = rHit.transform.gameObject.GetComponent<IDamageable>();
                damageable?.AnyDamage(WeaponData.Damage);


                //if (_rHit.transform.gameObject.CompareTag("Enemy")) AttackHitSound();
                //else if (_rHit.transform.gameObject.CompareTag("Breakable")) AttackMissSound();
                return;
            }
        //AttackMissSound();
    }

    public override void Reload()
    {
        if (!_isReloading)
        {
            _canShootAgain = false;
            _isReloading = true;
            _audioSource.PlayOneShot(WeaponData.SoundStartReload);
            _anim.SetTrigger("Reload");
        }
    }

    public void SFXGlockClipIn()
    {
        _audioSource.PlayOneShot(WeaponData.SoundEndReload);
    }

    public void AnimReloadFinished()
    {
        _isReloading = false;
        _currentBullets = WeaponData.Bullets;
        _canShootAgain = true;
    }

    void Update()
    {
        float delta = Time.deltaTime;
        Debug.DrawLine(forwardReference.position, forwardReference.position + forwardReference.forward * 20, Color.green);

        if (Input.GetKeyDown(KeyCode.Mouse0) && _canShootAgain)
            Attack();

        if (Input.GetKeyDown(KeyCode.R) && _currentBullets < WeaponData.Bullets)
            Reload();
        
        if (!_canShootAgain && _currentRecoil > 0)
            _currentRecoil -= delta;

        else if (!_canShootAgain && _currentRecoil <= 0 && !_isReloading)
            _canShootAgain = true;

        if (_currentMuzzleDuration > 0) _currentMuzzleDuration -= delta;
        else
        {
            muzzleLight.SetActive(false);
            muzzleSprite.SetActive(false);
        }
    }

    public void ActivateMuzzles()
    {
        muzzleLight.SetActive(true);
        muzzleSprite.SetActive(true);
        _currentMuzzleDuration = muzzleDuration;
    }

    public void Reset()
    {
        print("bullets: " + _currentBullets);
    }
}
