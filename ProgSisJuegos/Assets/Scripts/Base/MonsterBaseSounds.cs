using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AdditionalSounds
{
    None, Movement, 
}

public class MonsterBaseSounds : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioSource footstepsAudioSource;

    [SerializeField] private EnemyBase _monsterScript;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(EnemyStates state)
    {
        if (_audioSource == null || (state == EnemyStates.Idle && _audioSource.isPlaying)) return;
        if (state == EnemyStates.Damaged || state == EnemyStates.Attack || state == EnemyStates.RangedAttack || state == EnemyStates.Death) _audioSource.Stop();      

        AudioClip clip = _monsterScript.MonsterData.GetRandomClip(state);

        if (clip != null)
            _audioSource?.PlayOneShot(clip);
    }


    public void PlayStepSound(AdditionalSounds type)
    {
        if (type == AdditionalSounds.None)
        {
            footstepsAudioSource?.Stop();
            return;
        }

        if (footstepsAudioSource != null && footstepsAudioSource.isPlaying) return;
        AudioClip clip = _monsterScript.MonsterData.GetRandomAdditionalClip(AdditionalSounds.Movement);
        footstepsAudioSource?.PlayOneShot(clip);
    }

}
