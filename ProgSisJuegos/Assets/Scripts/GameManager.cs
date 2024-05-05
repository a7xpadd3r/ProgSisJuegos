using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string deathScene = "Death";    
    public List<AudioClip> _deathClips;
    public GameObject deathCam;

    [SerializeField] private PlayerController _thePlayer;
    [SerializeField] private UIManager _uiManager;
    private static AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _thePlayer.OnPlayerDeath += OnPlayerDeath;
    }

    public void PlayUISound(AudioClip clip, float volumeScale = 1)
    {
        _audioSource.PlayOneShot(clip, volumeScale);
    }

    public void GiveWeaponToPlayer(WeaponTypes type)
    {
        _thePlayer.OnGiveWeapon?.Invoke(type);
    }

    private void OnPlayerDeath()
    {
        _audioSource.PlayOneShot(_deathClips[Random.Range(0, _deathClips.Count - 1)]);
        _uiManager.DeathFade();

        _thePlayer.OnPlayerDeath -= OnPlayerDeath;
        _thePlayer.gameObject.SetActive(false);
        deathCam.SetActive(true);

        StartCoroutine(DeathSwitchLevel());
    }

    IEnumerator DeathSwitchLevel()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(GotoLevel(deathScene));
    }

    IEnumerator GotoLevel(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
            yield return null;
    }
}
