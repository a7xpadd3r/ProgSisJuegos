using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string deathScene = "Death";
    private static AudioSource _audioSource;
    [SerializeField] private PlayerController thePlayer;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        thePlayer.OnPlayerDeath += OnPlayerDeath;
    }

    public void PlayUISound(AudioClip clip, float volumeScale = 1)
    {
        _audioSource.PlayOneShot(clip, volumeScale);
    }

    public void GiveWeaponToPlayer(WeaponTypes type)
    {
        thePlayer.OnGiveWeapon?.Invoke(type);
    }

    private void OnPlayerDeath()
    {
        thePlayer.OnPlayerDeath -= OnPlayerDeath;
        StartCoroutine(GotoLevel(deathScene));
    }

    IEnumerator GotoLevel(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
            yield return null;
    }
}
