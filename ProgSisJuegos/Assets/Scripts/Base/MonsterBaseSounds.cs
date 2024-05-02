using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBaseSounds : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private EnemyBase _monsterScript;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(EnemyStates state)
    {
        if (state == EnemyStates.Idle && _audioSource.isPlaying) return;
        if (state == EnemyStates.Damaged || state == EnemyStates.Attack || state == EnemyStates.RangedAttack || state == EnemyStates.Death) _audioSource.Stop();      

        AudioClip clip = _monsterScript.MonsterData.GetRandomClip(state);

        if (clip != null)
            _audioSource.PlayOneShot(clip);
    }
}
