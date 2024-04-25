using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBase : MonoBehaviour, IDamageable
{
    public float _life = 5;
    public List<AudioClip> damagedAudios;

    private AudioSource _audioSource;


    public void AnyDamage(float amount)
    {
        _life -= amount;
        _audioSource.PlayOneShot(damagedAudios[Random.Range(0, damagedAudios.Count)]);
        if (_life <= 0) OnDeath();
    }

    public void OnDeath()
    {
        Destroy(this.gameObject);
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}
