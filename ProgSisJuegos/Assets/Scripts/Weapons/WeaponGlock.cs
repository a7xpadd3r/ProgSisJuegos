using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGlock : WeaponBase
{
    public float _currentRecoil;
    public int _currentBullets;

    public bool _canShootAgain = true;
    public bool _isReloading = false;

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
        }
        else
            Reload();        
    }

    public override void AttackRay()
    {
        
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && _canShootAgain)
            Attack();

        if (Input.GetKeyDown(KeyCode.R) && _currentBullets < WeaponData.Bullets)
            Reload();
        
        if (!_canShootAgain && _currentRecoil > 0)
            _currentRecoil -= Time.deltaTime;

        else if (!_canShootAgain && _currentRecoil <= 0 && !_isReloading)
            _canShootAgain = true;
    }

    public void Test()
    {

    }

}
