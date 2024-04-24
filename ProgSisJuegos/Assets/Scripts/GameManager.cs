using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static AudioSource _audioSource;
    public PlayerController thePlayer;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayUISound(AudioClip clip, float volumeScale = 1)
    {
        _audioSource.PlayOneShot(clip, volumeScale);
    }

    public void KitchenKnife()
    {
        thePlayer.TheKK();
    }
}
