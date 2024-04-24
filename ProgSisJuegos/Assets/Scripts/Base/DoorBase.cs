using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBase : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField, Range(-360, 360)] private float _openingAngle = 90;
    [SerializeField, Range(0.1f, 5)] private float _spamDelay = 2;
    [SerializeField] private bool _isLocked;

    [Header("SFX")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _openingSfx;
    [SerializeField] private AudioClip _closingSfx;
    [SerializeField] private AudioClip _lockedSfx;    

    private bool _isClosed = true;
    private bool _transitioning;
    private Quaternion _originalRotation;
    private float _currentSpamDelay;

    private void Start()
    {
        _originalRotation = transform.rotation;
    }

    public void Interact()
    {
        if (!_transitioning && !_isLocked)
        {
            if (_isClosed)
            {
                _audioSource.PlayOneShot(_openingSfx);
                this.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + _openingAngle, transform.rotation.z);
            }
            else
            {
                _audioSource.PlayOneShot(_closingSfx);
                this.transform.rotation = _originalRotation;
            }

            _transitioning = true;
            _currentSpamDelay = _spamDelay;
            _isClosed = !_isClosed;
        }

        else if (_isLocked && !_audioSource.isPlaying)
            _audioSource.PlayOneShot(_lockedSfx);
    }

    public void UnlockLockDoor(bool isLocked)
    {
        _isLocked = isLocked;
    }

    private void Update()
    {
        if (_transitioning)
        {
            float delta = Time.deltaTime;

            if (_currentSpamDelay > 0) _currentSpamDelay -= delta;
            else _transitioning = false;
        }
    }
}
