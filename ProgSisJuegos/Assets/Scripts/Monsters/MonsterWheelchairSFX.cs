using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWheelchairSFX : MonoBehaviour
{
    // Wheelchair monsters uses two audio sources for wheelchair squeaks
    [SerializeField] private MonsterWheelchairBase _monsterScript;
    [SerializeField] private AudioSource _movementAudioSource;
    [SerializeField] private AudioSource _otherAudioSource;

    private bool _isStunned;
    public bool IsStunned => _isStunned;

    public void PlayWalkSound()
    {
        if (!_movementAudioSource.isPlaying)
            _movementAudioSource.PlayOneShot(_monsterScript.MonsterData.SoundsMovement[Random.Range(0, _monsterScript.MonsterData.SoundsMovement.Count)]);
    }

    public void PlayAttackSound()
    {
        _otherAudioSource.Stop();
        _otherAudioSource.PlayOneShot(_monsterScript.MonsterData.SoundsMeleeAttack[0]);
    }

    public void StartStun()
    {
        _isStunned = true;
    }

    public void EndStun()
    {
        _isStunned = false;
    }

    public void PlayAnyDamageSound()
    {
        if (!_otherAudioSource.isPlaying)
            _otherAudioSource.PlayOneShot(_monsterScript.MonsterData.SoundsGetDamage[Random.Range(0, _monsterScript.MonsterData.SoundsGetDamage.Count)]);
    }

    public void PlayDeathSound()
    {
        _movementAudioSource.Stop();
        _otherAudioSource.Stop();
        _otherAudioSource.PlayOneShot(_monsterScript.MonsterData.SoundsDeath[0]);
    }

}
