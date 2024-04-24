using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimSfx : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSrc;
    [SerializeField] private AudioClip[] _stepSounds;
    public void PlayStepSfx()
    {
        if (_stepSounds.Length > 0) _audioSrc.PlayOneShot(_stepSounds[Random.Range(0, _stepSounds.Length)]);
    }
}
